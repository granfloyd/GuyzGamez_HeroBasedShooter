using Newtonsoft.Json.Bson;
using System;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Villain : HeroBase
{
    [SerializeField] private const int PRIMARY_FIRE_DAMAGE = 10;
    [SerializeField] private const int SECONDARY_FIRE_DAMAGE = 50;
    [SerializeField] private const float PRIMARY_FIRE_SPEED = 100;
    [SerializeField] private const float SECONDARY_FIRE_SPEED = 130;

    [SerializeField] private AudioSource chargeupbeepSource;//charge sound
    [SerializeField] private AudioSource rightclickSound;
    [SerializeField] private AudioSource leftclickSound;

    public ParticleSystem ps;

    [SerializeField] private int Rage;
    [SerializeField] private int maxRage;
    [SerializeField] private double roundedPing;

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

            HeroUI.Instance.UpdateAbilityCD(player.BaseAbility1, HeroUI.Instance.ability1Text);
            HeroUI.Instance.UpdateAbilityCD(player.BaseAbility2, HeroUI.Instance.ability2Text);
        }
    }
    public override void PrimaryFire()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
            SpawnObject spawnObject = player.GetComponent<SpawnObject>();
            leftclickSound.Play();

            spawnObject.SpawnObjectLocal(
                NetworkManager.Singleton.LocalClientId,
                roundedPing,
                player.primaryFireSpawnPos.position,
                player.orientation.localRotation.normalized);

            spawnObject.SpawnObjectServerRpc(
                NetworkManager.Singleton.LocalClientId,
                player.primaryFireSpawnPos.position,
                player.orientation.localRotation.normalized);

            spawnObject.ObjectConfig(
                player.tempGunAngle.normalized,
                PRIMARY_FIRE_SPEED,
                PRIMARY_FIRE_DAMAGE);

            spawnObject.ObjectConfigServerRpc(
                player.tempGunAngle.normalized,
                PRIMARY_FIRE_SPEED,
                PRIMARY_FIRE_DAMAGE); 
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
    public override void SecondaryFire()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
            SpawnObject spawnObject = player.GetComponent<SpawnObject>();
            rightclickSound.Play();
            UseRage();
            spawnObject.SpawnObjectLocal(
                NetworkManager.Singleton.LocalClientId,
                roundedPing,
                player.primaryFireSpawnPos.position,
                player.orientation.localRotation.normalized);

            spawnObject.SpawnObjectServerRpc(
                NetworkManager.Singleton.LocalClientId,
                player.primaryFireSpawnPos.position,
                player.orientation.localRotation.normalized);

            spawnObject.ObjectConfig(
                player.tempGunAngle.normalized,
                SECONDARY_FIRE_SPEED,
                SECONDARY_FIRE_DAMAGE);

            spawnObject.ObjectConfigServerRpc(
                player.tempGunAngle.normalized,
                SECONDARY_FIRE_SPEED,
                SECONDARY_FIRE_DAMAGE);

            ResetRage();//after adding rage to thing set it to 0

            if (NetworkManager.Singleton.LocalClientId != 0)
            {
                ClientResetRageClientRpc(NetworkManager.Singleton.LocalClientId);
                //send this to client 
            }

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
                maxRage = 200;
                Invoke("Ability3End", player.BaseAbility3.duration);

            }
        }
        
            
    }

    public override void Ability3End()
    {
        if (IsOwner)
        {
            base.Ability3End();
            maxRage = 100;
        }
    }

}
