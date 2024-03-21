using Unity.Netcode;
using UnityEngine;
using static PlayerController;

public class Aerial : HeroBase
{
    //public float boostForce;
    //
    //private void Start()
    //{
    //    if (IsOwner)
    //    {
    //        HeroBase player = PlayerController.Player;
    //        if (player == null)
    //        {
    //            Debug.LogError("Player is not set yet.");
    //            return;
    //        }
    //        player.ability3Charge = 0;
    //        player.ability3MaxCharge = 20;
    //        boostForce = 40f;
    //        player.SetUltSlider(ability3MaxCharge);
    //        Cursor.lockState = CursorLockMode.Locked;
    //        player.weaponPos = player.gameObject.transform.GetChild(0);
    //        player.weaponInstance = Instantiate(heroWeaponPrefab, player.weaponPos);
    //        player.primaryFireSpawnPos = player.weaponInstance.transform.GetChild(0);
    //    }
    //    
    //}
    //public override void PrimaryFire(ulong clientId)
    //{
    //    if (IsOwner)
    //    {
    //        HeroBase player = PlayerController.Player;
    //        GameObject spawnedPrimaryFire = Instantiate(heroPrimaryFirePrefab,
    //           player.primaryFireSpawnPos.position,
    //           player.orientation.localRotation);
    //        
    //
    //        // Spawn the bullet's NetworkObject
    //        NetworkObject bulletNetworkObject = spawnedPrimaryFire.GetComponent<NetworkObject>();
    //        bulletNetworkObject.SpawnWithOwnership(clientId); 
    //        
    //        Rigidbody rb = spawnedPrimaryFire.GetComponent<Rigidbody>();
    //        rb.velocity = tempGunAngle * 50f;
    //    }
    //}
    //
    //public override void SecondaryFire()
    //{
    //    
    //}
    //
    //public override void Ability1()
    //{
    //    if (IsOwner)
    //    {
    //        Boost();
    //        SetDurationSlider(ability1Duration);
    //        Invoke("Ability1Duration", ability1Duration);
    //    }
    //}
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
    //    player.rb.AddForce(Vector3.up * boostForce * player.rb.mass * 4, ForceMode.Impulse);
    //    player.rb.useGravity = false;
    //}
    //public override void Ability2()
    //{
    //    HeroBase player = PlayerController.Player;
    //}
    //
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
