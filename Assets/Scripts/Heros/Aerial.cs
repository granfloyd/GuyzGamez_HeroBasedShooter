using UnityEngine;

public class Aerial : HeroBase
{
    public float boostForce;
   
    private void Start()
    {
        HeroBase player = PlayerController.Player;
        boostForce = 40f;
        Cursor.lockState = CursorLockMode.Locked;
        player.weaponPos = player.gameObject.transform.GetChild(0);
        player.weaponInstance = Instantiate(heroWeaponPrefab, player.weaponPos);
        player.primaryFireSpawnPos = player.weaponInstance.transform.GetChild(0);
    }
    public override void PrimaryFire()
    {
        HeroBase player = PlayerController.Player;
        GameObject spawnedPrimaryFire = Instantiate(heroPrimaryFirePrefab,
            PlayerController.Player.primaryFireSpawnPos.position,
            PlayerController.Player.orientation.localRotation);
        Rigidbody rb = spawnedPrimaryFire.GetComponent<Rigidbody>();
        rb.velocity = tempGunAngle * 10f;
    }
    
    public override void SecondaryFire()
    {
        
    }
    
    public override void Ability1()
    {
        Boost();
        Invoke("Ability1Duration", ability1Duration);
    }
    public override void Ability1Duration()
    {
        HeroBase player = PlayerController.Player;
        if (player.isFlying)
        {
            player.rb.velocity = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);
            player.isFlying = false;
            player.rb.useGravity = true;           
        }        
    }
    void Boost()
    {
        HeroBase player = PlayerController.Player;
        player.rb.velocity = new Vector3(player.rb.velocity.x, 0, player.rb.velocity.z);
        player.isFlying = true;
        player.rb.AddForce(Vector3.up * boostForce * player.rb.mass * 4, ForceMode.Impulse);
        player.rb.useGravity = false;


    }
    public override void Ability2()
    {
        //HeroBase player = PlayerController.Player;
        //player.ability2Instance = Instantiate(ability2Prefab, player.transform.position, player.orientation.localRotation);
    }
    
    public override void Ability3()
    {
        Debug.Log("Q: Ability 3: DamageMain");
    }
}
