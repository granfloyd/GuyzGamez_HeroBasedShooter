using Newtonsoft.Json.Bson;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Merlin : HeroBase
{
    private float dashSpeed = 15f;
    private float dashDuration;
    private float eDuration;
    private Vector3 dashDirection;
    private bool isDashing;
    private const int PRIMARY_FIRE_DAMAGE = 10;
    private void Start()
    {
        if (IsOwner)
        {
            dashDuration = 0.5f;
            eDuration = 3f;
            PlayerCamera.iscamset = false;
            PlayerController.Player.baseAbility1 = new Ability(3f, dashDuration);
            PlayerController.Player.baseAbility2 = new Ability(7f, eDuration); 
            PlayerController.Player.baseAbility3 = new Ability(20f, 10f);
            HeroBase player = PlayerController.Player;
            HeroUI.Instance.SetUltSlider();
            if (player == null)
            {
                Debug.LogError("Player is not set yet.");
                return;
            }
            isDashing = false;
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
            SpawnBulletServerRpc(player.primaryFireSpawnPos.position, player.orientation.localRotation, tempGunAngle);
        }
    }

    [ServerRpc]
    void SpawnBulletServerRpc(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        GameObject spawnedPrimaryFire = Instantiate(heroPrimaryFirePrefab, position, rotation);

        spawnedPrimaryFire.GetComponent<MerlinProjectile>().SetDamage(PRIMARY_FIRE_DAMAGE);
        // Spawn the bullet's NetworkObject
        NetworkObject bulletNetworkObject = spawnedPrimaryFire.GetComponent<NetworkObject>();
        bulletNetworkObject.SpawnWithOwnership(NetworkManager.LocalClientId);

        Rigidbody rb = spawnedPrimaryFire.GetComponent<Rigidbody>();
        rb.velocity = velocity * 50f;

    }

    public override void SecondaryFire()
    {
        
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
            player.rb.velocity = dashDirection * dashSpeed;
        }
    }
    public override void Ability2()
    {
        if (IsOwner)
        {
            Debug.Log("using ability 2");
            HeroBase player = PlayerController.Player;
            player.gameObject.GetComponent<Modifiers>().isBonk = true;
            Invoke("UnnamedAbility2", eDuration);
            HeroUI.Instance.SetDurationSlider(PlayerController.Player.baseAbility2);
        }
    }
    void UnnamedAbility2()
    {
        if(IsOwner)
        {
            HeroBase player = PlayerController.Player;
            player.gameObject.GetComponent<Modifiers>().isBonk = false;
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
