using Newtonsoft.Json.Bson;
using System;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Villain : HeroBase
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
    [SerializeField] private const float PRIMARY_FIRE_SPEED = 70;
    [SerializeField] private const float SECONDARY_FIRE_SPEED = 90;

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
            HeroBase player = PlayerController.Player;
            dashDuration = 0.7f;
            eDuration = 5f;
            ultDuration = 10f;
            Rage = 0;
            maxRage = 100;
            ultMultiplier = 2;
            PlayerCamera.iscamset = false;
            player.baseAbility1 = new Ability(3f, dashDuration);
            player.baseAbility2 = new Ability(7f, eDuration);
            player.baseAbility3 = new Ability(1f, ultDuration);            
            HeroUI.Instance.SetUltSlider();
            HeroUI.Instance.SetSomethingText(Rage.ToString());
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
            HeroBase player = PlayerController.Player;
            NetworkTime ping = NetworkManager.Singleton.LocalTime - NetworkManager.Singleton.ServerTime;
            double pingInMilliseconds = ping.Time * 1000;
            roundedPing = Math.Round(pingInMilliseconds, 1);
            player.baseAbility1.UpdateTimer();
            player.baseAbility2.UpdateTimer();
            player.baseAbility3.UpdateTimer();
            HeroUI.Instance.UpdateAbilityCD(player.baseAbility1, HeroUI.Instance.ability1Text);
            HeroUI.Instance.UpdateAbilityCD(player.baseAbility2, HeroUI.Instance.ability2Text);
            if (player.baseAbility2.duration >= 0)
            {
                HeroUI.Instance.UpdateDurationSlider(player.baseAbility2);
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
                player.orientation.localRotation.normalized,
                player.tempGunAngle.normalized,
                0);

            //Request the server to spawn the bullet.
            SpawnBulletServerRpc(NetworkManager.Singleton.LocalClientId,
                roundedPing,
                Type.primary,
                player.primaryFireSpawnPos.position,
                player.orientation.localRotation.normalized,
                player.tempGunAngle.normalized,
                0);//no bonus dmg 
        }
    }
    
    void SpawnBulletClient(ulong clientid ,double ping ,Type bulletType ,Vector3 position ,Quaternion rotation ,Vector3 velocity ,int rageValue)
    {
        if (IsServer) return;

        switch(bulletType)
        {
            case Type.primary:
                GameObject clientProjectile = Instantiate(ClientProjectilePrefab, position, rotation);
                clientProjectile.GetComponent<VillainProjectile>().ownerID = clientid;
                clientProjectile.GetComponent<VillainProjectile>().SetLifeSpan(2);
                clientProjectile.GetComponent<VillainProjectile>().SetDamage(PRIMARY_FIRE_DAMAGE);
                clientProjectile.GetComponent<VillainProjectile>().SetMovement(velocity, PRIMARY_FIRE_SPEED);
                break;
            case Type.secondary:
                GameObject clientSecondaryProjectile = Instantiate(ClientProjectilePrefab, position, rotation);
                clientSecondaryProjectile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                clientSecondaryProjectile.GetComponent<VillainProjectile>().isSecondaryFire = true;
                clientSecondaryProjectile.GetComponent<VillainProjectile>().ownerID = clientid;
                clientSecondaryProjectile.GetComponent<VillainProjectile>().SetLifeSpan(2);
                clientSecondaryProjectile.GetComponent<VillainProjectile>().SetDamage(SECONDARY_FIRE_DAMAGE + rageValue);
                clientSecondaryProjectile.GetComponent<VillainProjectile>().SetMovement(velocity, SECONDARY_FIRE_SPEED);
                break;
        }
        
    }

    [ServerRpc]
    void SpawnBulletServerRpc(ulong clientId,double ping, Type bulletType,Vector3 position, Quaternion rotation, Vector3 velocity, int rageValue)
    {
        GameObject serverProjectile;
        GameObject serverSecondaryProjectile;
        switch (bulletType)
        {
            
            case Type.primary:
                leftclickSound.Play();
                serverProjectile = Instantiate(heroPrimaryFirePrefab, position, rotation);
                serverProjectile.GetComponent<VillainProjectile>().ownerID = clientId;
                serverProjectile.GetComponent<VillainProjectile>().SetLifeSpan(2);
                AssignBulletToPlayer(serverProjectile, clientId);
                serverProjectile.GetComponent<VillainProjectile>().SetDamage(PRIMARY_FIRE_DAMAGE);
                serverProjectile.GetComponent<VillainProjectile>().SetMovement(velocity, PRIMARY_FIRE_SPEED);
                break;
            case Type.secondary:
                rightclickSound.Play();
                serverSecondaryProjectile = Instantiate(heroPrimaryFirePrefab, position, rotation);
                serverSecondaryProjectile.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                serverSecondaryProjectile.GetComponent<VillainProjectile>().isSecondaryFire = true;
                serverSecondaryProjectile.GetComponent<VillainProjectile>().ownerID = clientId;
                serverSecondaryProjectile.GetComponent<VillainProjectile>().SetLifeSpan(2);
                AssignBulletToPlayer(serverSecondaryProjectile, clientId);
                //use rage bonus regardless of hitting enemy
                serverSecondaryProjectile.GetComponent<VillainProjectile>().SetDamage(SECONDARY_FIRE_DAMAGE + rageValue);
                serverSecondaryProjectile.GetComponent<VillainProjectile>().SetMovement(velocity, SECONDARY_FIRE_SPEED);
                ResetRage();//after adding rage to thing set it to 0
                
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
            HeroBase player = PlayerController.Player;
            UseRage();

            // Spawn the bullet immediately on the client side.
            SpawnBulletClient(NetworkManager.Singleton.LocalClientId,//owner id
                roundedPing,//ping
                Type.secondary,//type
                player.primaryFireSpawnPos.position,
                player.orientation.localRotation.normalized,
                player.tempGunAngle.normalized,//direction
                Rage);//bonusdmg

            SpawnBulletServerRpc(NetworkManager.Singleton.LocalClientId,
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
        }
        Rage += addTo;

        chargeupbeepSource.pitch = 0.6f + ((float)Rage / maxRage) * (1.5f - 0.6f);

        // Ensure the pitch does not exceed the maximum.
        if (chargeupbeepSource.pitch > 1.5f)
        {
            chargeupbeepSource.pitch = 1.5f;
        }

        if (!chargeupbeepSource.isPlaying)
        {
            chargeupbeepSource.Play();
        }

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
            
            if (horizontalInput != 0)
            {
                dashDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
                
            }
            else
            {
                if (verticalInput > 0 && horizontalInput == 0)
                {
                    float cameraPosY = Camera.main.gameObject.transform.forward.y;
                    dashDirection.y = cameraPosY;
                }
                dashDirection = Camera.main.gameObject.transform.forward;
            }
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
            AddToRage(1);
        }

        if(isBoosting)
        {
            HeroBase player = PlayerController.Player;
            Vector3 boostMovement = boostDirection * dashSpeed * Time.deltaTime;
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
                player.canGainUltCharge = false;
                isUltForm = true;
                SetDash(false, true);
                player.recovery /= ultMultiplier; 
                player.recovery2 /= ultMultiplier;
                maxRage *= ultMultiplier;
                eDuration *= ultMultiplier;                
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
            player.canGainUltCharge = true;
            isUltForm = false;
            player.recovery *= ultMultiplier;
            player.recovery2 *= ultMultiplier; 
            maxRage /= ultMultiplier;
            eDuration /= ultMultiplier;            
            ps.Stop();
        }
    }

}
