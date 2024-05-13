using Newtonsoft.Json.Bson;
using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class Merlin : HeroBase
{
    public enum Type
    {
        primary,
        secondary
    }
    [SerializeField] private float speedMultiplier = 3;
    [SerializeField] private float secondaryFireSpeed = 30;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration;
    [SerializeField] private float eDuration;
    [SerializeField] private float ultDuration;
    [SerializeField] private Vector3 dashDirection;
    [SerializeField] private Vector3 boostDirection;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isBoosting;
    [SerializeField] private bool isEActive;
    [SerializeField] private bool isUltForm;

    [SerializeField] private const int PRIMARY_FIRE_DAMAGE = 10;
    [SerializeField] private const int SECONDARY_FIRE_DAMAGE = 50;
    [SerializeField] private AudioSource chargeupbeepSource;//charge sound
    [SerializeField] private AudioSource rightclickSound;
    [SerializeField] private AudioSource leftclickSound;

    public ParticleSystem ps;

    [SerializeField] private int Rage;
    [SerializeField] private int maxRage;
    [SerializeField] private int ultMultiplier;
    [SerializeField] private double roundedPing;

    public GameObject ClientProjectilePrefab;
    private void Start()
    {
        if (IsOwner)
        {
            dashDuration = 0.7f;
            eDuration = 5f;
            ultDuration = 10f;
            Rage = 0;
            maxRage = 100;
            ultMultiplier = 2;
            PlayerCamera.iscamset = false;
            PlayerController.Player.baseAbility1 = new Ability(3f, dashDuration);
            PlayerController.Player.baseAbility2 = new Ability(7f, eDuration); 
            PlayerController.Player.baseAbility3 = new Ability(1f, ultDuration);
            HeroBase player = PlayerController.Player;
            HeroUI.Instance.SetUltSlider();
            HeroUI.Instance.SetSomethingText(Rage.ToString());
            if (player == null)
            {
                Debug.LogError("Player is not set yet.");
                return;
            }
            isDashing = false;
            isBoosting = false;
            isEActive = false;
            isUltForm = false;

            if (!isUltForm && ps.isPlaying)
            {
                ps.Stop();
            }

            Cursor.lockState = CursorLockMode.Locked;
        }        
    }
    protected new void Update()
    {
        base.Update();
        if (IsOwner)
        {
            NetworkTime ping = NetworkManager.Singleton.LocalTime - NetworkManager.Singleton.ServerTime;
            double pingInMilliseconds = ping.Time * 1000;
            roundedPing = Math.Round(pingInMilliseconds, 1);
            PlayerController.Player.baseAbility1.UpdateTimer();
            PlayerController.Player.baseAbility2.UpdateTimer();
            PlayerController.Player.baseAbility3.UpdateTimer();
            HeroUI.Instance.UpdateAbilityCD(PlayerController.Player.baseAbility1, HeroUI.Instance.ability1Text);
            HeroUI.Instance.UpdateAbilityCD(PlayerController.Player.baseAbility2, HeroUI.Instance.ability2Text);
            if (PlayerController.Player.baseAbility2.duration >= 0)
            {
                HeroUI.Instance.UpdateDurationSlider(PlayerController.Player.baseAbility2);
            }
            DashMovement();            
        }
    }
    public override void PrimaryFire()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
            // Spawn the bullet immediately on the client side.
            SpawnBulletClient(NetworkManager.Singleton.LocalClientId,
                roundedPing,
                Type.primary,
                player.primaryFireSpawnPos.position,
                player.orientation.localRotation,
                player.tempGunAngle,
                0);
            //Request the server to spawn the bullet.
            SpawnBulletServerRpc(NetworkManager.Singleton.LocalClientId,
                roundedPing,
                Type.primary,
                player.primaryFireSpawnPos.position,
                player.orientation.localRotation,
                player.tempGunAngle,
                0);//no bonus dmg 
        }
    }
    
    void SpawnBulletClient(ulong clientid ,double ping ,Type bulletType ,Vector3 position ,Quaternion rotation ,Vector3 velocity ,int rageValue)
    {
        if (IsServer) return;//host has  0 ping

        switch(bulletType)
        {
            case Type.primary:
                GameObject clientProjectile = Instantiate(ClientProjectilePrefab, position, rotation);
                clientProjectile.GetComponent<ClientProjectile>().ownerID = clientid;
                clientProjectile.GetComponent<ClientProjectile>().SetDamage(PRIMARY_FIRE_DAMAGE);
                float speed = 10.0f;
                Rigidbody rb = clientProjectile.GetComponent<Rigidbody>();
                rb.velocity = velocity * speed;
                break;
            case Type.secondary:
                break;
        }
        
    }

    [ServerRpc]
    void SpawnBulletServerRpc(ulong clientId,double ping, Type bulletType,Vector3 position, Quaternion rotation, Vector3 velocity, int rageValue)
    { 
        switch (bulletType)
        {
            case Type.primary:
                leftclickSound.Play();
                float primaryBulletSpeed = 10.0f;
                GameObject spawnedPrimaryFire = Instantiate(heroPrimaryFirePrefab, position, rotation);
                spawnedPrimaryFire.GetComponent<MerlinProjectile>().ownerID = clientId;
                spawnedPrimaryFire.GetComponent<MerlinProjectile>().SetDamage(PRIMARY_FIRE_DAMAGE);                
                AssignBulletToPlayer(spawnedPrimaryFire, clientId);
                Rigidbody rb = spawnedPrimaryFire.GetComponent<Rigidbody>();
                rb.velocity = velocity * primaryBulletSpeed;
                break;
            case Type.secondary:
                rightclickSound.Play();
                float secondaryBulletSpeed = 90;
                GameObject spawnedSecondaryFire = Instantiate(heroPrimaryFirePrefab, position, rotation);
                spawnedSecondaryFire.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                spawnedSecondaryFire.GetComponent<MerlinProjectile>().isSecondaryFire = true;
                spawnedSecondaryFire.GetComponent<MerlinProjectile>().ownerID = clientId;
                //use rage bonus regardless of hitting enemy
                spawnedSecondaryFire.GetComponent<MerlinProjectile>().SetDamage(SECONDARY_FIRE_DAMAGE + rageValue);
                ResetRage();//after adding rage to thing set it to 0
                AssignBulletToPlayer(spawnedSecondaryFire,clientId);
                rb = spawnedSecondaryFire.GetComponent<Rigidbody>();
                rb.velocity = velocity * secondaryBulletSpeed;
                if(clientId != 0)
                {
                    ClientResetRageClientRpc(clientId);
                    //send this to client 
                }
                break;

        }

    }

    [ClientRpc]
    private void ClientResetRageClientRpc(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            ResetRage();
        }
    }
    void AssignBulletToPlayer(GameObject spawnedGameObject, ulong clientid)
    {
        NetworkObject bulletNetworkObject = spawnedGameObject.GetComponent<NetworkObject>();
        bulletNetworkObject.SpawnWithOwnership(clientid);
        if(!IsOwnedByServer)
        bulletNetworkObject.NetworkHide(clientid);

    }

    public override void SecondaryFire()
    {
        if (IsOwner)
        {            
            UseRage();
            HeroBase player = PlayerController.Player;
            SpawnBulletServerRpc(NetworkManager.Singleton.LocalClientId,//owner id
                roundedPing,//ping
                Type.secondary,//type
                player.primaryFireSpawnPos.position,
                player.orientation.localRotation,
                player.tempGunAngle,//direction
                Rage);//bonusdmg
        }
    }
    public void AddToRage(int addTo)
    {
        if (isEActive)
        {
            addTo *= 2;
            if(chargeupbeepSource.pitch < 1.50f)
                chargeupbeepSource.pitch += 0.10f;
        }
        Rage += addTo;
        if (chargeupbeepSource.pitch < 1.50f)
            chargeupbeepSource.pitch += 0.10f;

        chargeupbeepSource.Play();
        if (Rage >= maxRage)
        {
            Rage = maxRage;
            HeroUI.Instance.SetSomethingText(Rage.ToString());
        }
        else
        {
            HeroUI.Instance.SetSomethingText(Rage.ToString());
        }
    }
    private int UseRage()
    {
        if (Rage > 0)
        {
            if (isEActive)
            {
                return Rage *= 2;
            }
            else
            {
                return Rage;
            }
        } 
        return Rage;
    }
    private void ResetRage()
    {
        chargeupbeepSource.pitch = 0.60f;
        Rage = 0;
        HeroUI.Instance.SetSomethingText(Rage.ToString());
    }
    public override void Ability1()
    {
        if (IsOwner)
        {
            SetDash(true,false);            
        }
    }
    void SetDash(bool isLshift, bool isforult)
    {
        if(isLshift)//Lshift
        {
            isDashing = true;
            dashDirection = Camera.main.gameObject.transform.forward;
            Invoke("StopDashing", dashDuration);
        }
        else
        {
            isDashing = false;
        }

        if(isforult)
        {
            isBoosting = true;
            boostDirection = Camera.main.gameObject.transform.up;
            Invoke("StopDashing", 0.5f);
        }
        else
        {
            isBoosting = false;
        }
    }
    void DashMovement()
    {
        if (isDashing)
        {
            HeroBase player = PlayerController.Player;
            Vector3 dashMovement = dashDirection * dashSpeed * Time.deltaTime;
            player.verticalVelocity = 0;
            player.controller.Move(dashMovement);
        }

        if(isBoosting)
        {
            HeroBase player = PlayerController.Player;
            Vector3 boostMovement = boostDirection * dashSpeed * ultMultiplier * Time.deltaTime;
            player.controller.Move(boostMovement);
        }
    }

    void StopDashing()
    {
        SetDash(false, false);
    }
    
    public override void Ability2()
    {
        if (IsOwner)
        {
            isEActive = true;
            HeroBase player = PlayerController.Player;
            Modifiers mod = player.gameObject.GetComponent<Modifiers>();
            mod.AddModifier(Modifiers.ModifierIndex.MERLIN_EDMG_MULTIPLIER);
            Invoke("UnnamedAbility2", eDuration);
            HeroUI.Instance.SetDurationSlider(PlayerController.Player.baseAbility2);
        }
    }
    void UnnamedAbility2()
    {
        if(IsOwner)
        {
            HeroBase player = PlayerController.Player;
            isEActive = false;
            Modifiers mod = player.gameObject.GetComponent<Modifiers>();
            mod.RemoveModifier(Modifiers.ModifierIndex.MERLIN_EDMG_MULTIPLIER);
        }
        
    }

    public override void Ability3()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
            if (player.ability3Charge >= player.ability3MaxCharge)
            {
                HeroUI.Instance.ResetUltSlider();
                player.isAffectedByGravity = false;
                player.isFlying = true;
                isUltForm = true;
                SetDash(false, true);
                player.recovery /= ultMultiplier; 
                player.recovery2 /= ultMultiplier;
                maxRage *= ultMultiplier;
                ps.Play();
                Invoke("UnnamedAbility3", ultDuration);
            }
        }
        
            
    }

    void UnnamedAbility3()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
            player.isAffectedByGravity = true;
            player.isFlying = false;
            isUltForm = false;
            player.recovery *= ultMultiplier;
            player.recovery2 *= ultMultiplier; 
            maxRage /= ultMultiplier;
            ps.Stop();
        }
    }

}
