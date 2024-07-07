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
    [SerializeField] private double roundedPing;

    public GameObject ClientProjectilePrefab;
    private void Start()
    { 
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;            
            Rage = 0;
            maxRage = 100;
            PlayerCamera.iscamset = false;
            player.BaseAbility1 = new ChargeupDash();
            player.BaseAbility2 = new Surge();
            player.BaseAbility3 = new Overclock();         
            HeroUI.Instance.SetUltSlider();
            HeroUI.Instance.DisplayText(Rage.ToString());
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
            player.BaseAbility1.AbilityUpdate();
            player.BaseAbility2.AbilityUpdate();
            player.BaseAbility3.AbilityUpdate();
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
        if (PlayerController.Player.BaseAbility2.isActive)
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
            HeroUI.Instance.DisplayText(Rage.ToString());
        }                   
        else                
        {                   
            HeroUI.Instance.DisplayText(Rage.ToString());
        }
    }
    private int UseRage()
    {
        if (Rage > 0)
        {
            if (PlayerController.Player.BaseAbility2.isActive)
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
        HeroUI.Instance.DisplayText(Rage.ToString());
    }
    public override void Ability1()
    {
        HeroBase player = PlayerController.Player;
        if (IsOwner)
        {
            player.BaseAbility1.Use();
            if(player.BaseAbility1.duration > 0)
            {
                Invoke("Ability1End", player.BaseAbility1.duration);
            }           
        }
    }

    public override void Ability1End()
    {
        base.Ability1End();
    }
    
    public override void Ability2()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
            player.BaseAbility2.Use();
            Modifiers mod = player.gameObject.GetComponent<Modifiers>();
            mod.AddModifier(Modifiers.ModifierIndex.MERLIN_EDMG_MULTIPLIER);
            Invoke("Ability2End", player.BaseAbility2.duration);
            
        }
    }
    public override void Ability2End()
    {
        if(IsOwner)
        {
            base.Ability2End();
            HeroBase player = PlayerController.Player;
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
                player.BaseAbility3.Use();
                Invoke("Ability3End", player.BaseAbility3.duration);
            }
        }
        
            
    }

    public override void Ability3End()
    {
        if (IsOwner)
        {
            base.Ability3End();
            
        }
    }

}
