using Newtonsoft.Json.Bson;
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
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration;
    [SerializeField] private float eDuration;
    [SerializeField] private Vector3 dashDirection;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isEActive;
    [SerializeField] private const int PRIMARY_FIRE_DAMAGE = 10;
    [SerializeField] private const int SECONDARY_FIRE_DAMAGE = 50;

    [SerializeField] private int Rage = 0;
    private void Start()
    {
        if (IsOwner)
        {
            Debug.Log("calling start");
            speedMultiplier = 15f;
            dashDuration = 0.7f;
            eDuration = 5f;
            PlayerCamera.iscamset = false;
            PlayerController.Player.baseAbility1 = new Ability(3f, dashDuration);
            PlayerController.Player.baseAbility2 = new Ability(7f, eDuration); 
            PlayerController.Player.baseAbility3 = new Ability(20f, 10f);
            HeroBase player = PlayerController.Player;
            HeroUI.Instance.SetUltSlider();
            HeroUI.Instance.SetSomethingText(Rage.ToString());
            if (player == null)
            {
                Debug.LogError("Player is not set yet.");
                return;
            }
            isDashing = false;
            isEActive = false;

            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }
    protected new void Update()
    {
        base.Update();
        if (IsOwner)
        {
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
            SpawnBulletServerRpc(NetworkManager.Singleton.LocalClientId, Type.primary,player.primaryFireSpawnPos.position, player.orientation.localRotation, player.tempGunAngle);
        }
    }

    [ServerRpc]
    void SpawnBulletServerRpc(ulong clientId, Type bulletType,Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        switch(bulletType)
        {
            case Type.primary:
                float primaryBulletSpeed = 70f;
                Debug.Log("coming from" + clientId);
                GameObject spawnedPrimaryFire = Instantiate(heroPrimaryFirePrefab, position, rotation);
                spawnedPrimaryFire.GetComponent<MerlinProjectile>().ownerID = clientId;
                spawnedPrimaryFire.GetComponent<MerlinProjectile>().SetDamage(PRIMARY_FIRE_DAMAGE);                
                AssignBulletToPlayer(spawnedPrimaryFire);
                Rigidbody rb = spawnedPrimaryFire.GetComponent<Rigidbody>();
                rb.velocity = velocity * primaryBulletSpeed;
                break;
            case Type.secondary:
                float secondaryBulletSpeed = 50f;
                GameObject spawnedSecondaryFire = Instantiate(heroPrimaryFirePrefab, position, rotation);
                spawnedSecondaryFire.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                spawnedSecondaryFire.GetComponent<MerlinProjectile>().isSecondaryFire = true;
                spawnedSecondaryFire.GetComponent<MerlinProjectile>().ownerID = clientId;
                UseRage();//use rage bonus regardless of hitting enemy
                spawnedSecondaryFire.GetComponent<MerlinProjectile>().SetDamage(SECONDARY_FIRE_DAMAGE + Rage);
                ResetRage();//after adding rage to thing set it to 0
                AssignBulletToPlayer(spawnedSecondaryFire);
                rb = spawnedSecondaryFire.GetComponent<Rigidbody>();
                rb.velocity = velocity * secondaryBulletSpeed;
                break;
        }
    }

    void AssignBulletToPlayer(GameObject spawnedGameObject)
    {
        NetworkObject bulletNetworkObject = spawnedGameObject.GetComponent<NetworkObject>();
        bulletNetworkObject.SpawnWithOwnership(NetworkManager.LocalClientId);
    }

    public override void SecondaryFire()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
            SpawnBulletServerRpc(NetworkManager.Singleton.LocalClientId, Type.secondary,player.primaryFireSpawnPos.position, player.orientation.localRotation, player.tempGunAngle);
        }
    }
    public void AddToRage(int addTo)
    {
            if (isEActive)
            addTo *= 2;

        Rage += addTo;
        
        if (Rage >= 100)
        {
            Rage = 100;
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
        //feels bad you have no rage
        return Rage;
    }
    private void ResetRage()
    {
        Rage = 0;
        HeroUI.Instance.SetSomethingText(Rage.ToString());
    }
    public override void Ability1()
    {
        if (IsOwner)
        {
            SetDash(true);            
        }
    }
    void SetDash(bool state)
    {
        if(state)
        {
            isDashing = true;
            Vector3 rayOrigin = PlayerController.Player.orientation.position;
            dashDirection = Camera.main.gameObject.transform.forward;
            Invoke("StopDashing", dashDuration);
        }
        else
        {
            isDashing = false;
        }
    }
    void StopDashing()
    {
        SetDash(false);
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
    }
    public override void Ability2()
    {
        if (IsOwner)
        {
            Debug.Log("using ability 2");
            isEActive = true;
            HeroBase player = PlayerController.Player;
            Modifiers mod = player.gameObject.GetComponent<Modifiers>();
            mod.AddModifier(Modifiers.ModifierIndex.MERLIN_EDMG_MULTIPLIER);
            speedMultiplier = 50f;
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
            speedMultiplier = 15f;
        }
        
    }

    //public override void Ability3()
    //{
    //    if (IsOwner)
    //    {
    //        HeroBase player = PlayerController.Player;
    //        if (player.ability3Charge >= player.ability3MaxCharge)
    //        {
    //            player.SetUltSlider(player.ability3MaxCharge);
    //        }
    //    }
    //}

}
