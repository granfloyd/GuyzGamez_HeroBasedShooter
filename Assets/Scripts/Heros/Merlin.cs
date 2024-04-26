using Newtonsoft.Json.Bson;
using Unity.Netcode;
using UnityEngine;

public class Merlin : HeroBase
{
    public float boostForce;
    
    private void Start()
    {
        if (IsOwner)
        {
            PlayerCamera.iscamset = false;
            PlayerController.Player.baseAbility1 = new Ability(5f, 0f);
            PlayerController.Player.baseAbility2 = new Ability(15f, 3f); 
            PlayerController.Player.baseAbility3 = new Ability(20f, 10f);
            HeroBase player = PlayerController.Player;
            HeroUI.Instance.SetUltSlider();
            if (player == null)
            {
                Debug.LogError("Player is not set yet.");
                return;
            }
            boostForce = 70f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }
    protected new void Update()
    {
        base.Update();
        if (IsOwner)
        {
            if (PlayerController.Player.baseAbility1.duration >= 0)
            {
                HeroUI.Instance.UpdateDurationSlider(PlayerController.Player.baseAbility1);
            }
        }
        if (IsOwner)
        {
            HeroUI.Instance.UpdateAbilityCD(PlayerController.Player.baseAbility1, HeroUI.Instance.ability1Text);
            HeroUI.Instance.UpdateAbilityCD(PlayerController.Player.baseAbility2, HeroUI.Instance.ability2Text);
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
            Dash();
            //Boost();
            //HeroUI.Instance.SetDurationSlider(PlayerController.Player.baseAbility1);
            //Invoke("Ability1Duration", PlayerController.Player.baseAbility1.duration);
        }
    }
    void Dash()
    {
        HeroBase player = PlayerController.Player;
        player.rb.AddForce(player.orientation.forward * boostForce * player.rb.mass , ForceMode.Impulse);
    }
    //scraping the flying ability for this character 
    //public override void Ability1Duration()
    //{
    //    HeroBase player = PlayerController.Player;
    //    if (player.isFlying)
    //    {
    //        player.rb.velocity = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);
    //        player.isFlying = false;
    //        player.rb.useGravity = true;           
    //    }        
    //}
    //void Boost()
    //{
    //    HeroBase player = PlayerController.Player;
    //    player.rb.velocity = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);
    //    player.isFlying = true;
    //    player.rb.AddForce(Vector3.up * boostForce * player.rb.mass, ForceMode.Impulse);
    //    player.rb.useGravity = false;
    //}
    public override void Ability2()
    {

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
