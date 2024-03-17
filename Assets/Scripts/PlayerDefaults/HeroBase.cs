using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroBase : PlayerMovement
{
    [Header("   HeroBase")]
    [SerializeField] protected int health;
    [SerializeField] public float recovery;    
    public bool isFlying = false;

    public Vector3 crosshairPos;
    public Vector3 tempGunAngle;    
    public GameObject heroWeaponPrefab;
    public Transform weaponPos;
    public GameObject heroPrimaryFirePrefab;
    public Transform primaryFireSpawnPos;
    [HideInInspector] public GameObject weaponInstance = null;
    [HideInInspector] public GameObject bulletInstance = null;
    [SerializeField] public Slider durationSlider;
    [SerializeField] public Slider ability3Slider;

    [Header("PrimaryFire")]
    [SerializeField] public float primaryFireTimer;    

    [Header("SecondaryFire")]
    [SerializeField] public float secondaryFireTimer;

    [Header("Ability1")]    
    [SerializeField] public float ability1Cooldown;
    [SerializeField] public float ability1Duration;
    public float ability1Timer;

    [Header("Ability2")] 
    [SerializeField] public float ability2Cooldown;
    [SerializeField] public float ability2Duration;
    public float ability2Timer;

    [Header("Ability3")]
    [SerializeField] public float ability3Charge;//called in heros damage scripts
    [SerializeField] public float ability3MaxCharge;
    [SerializeField] public float ability3Cooldown;
    [SerializeField] public float ability3Duration;
    public float ability3Timer;
    public void SetDurationSlider(float duration)
    {
        HeroBase player = PlayerController.Player;
        player.durationSlider.gameObject.SetActive(true);
        player.durationSlider.maxValue = duration;
        player.durationSlider.value = duration;
    }
    public void UpdateDurationSlider()
    {
        HeroBase player = PlayerController.Player;
        if (player.durationSlider.value > 0)
        {
            player.durationSlider.value -= Time.deltaTime;
        }
        else
        {
            player.durationSlider.gameObject.SetActive(false);
        }
    }

    public void SetUltSlider(float maxcharge)
    {
        HeroBase player = PlayerController.Player;
        player.ability3Slider.maxValue = maxcharge;
        player.ability3Slider.value = 0;
        player.ability3Charge = player.ability3Slider.value;
    }   
    public void UpdateUltSlider(float howmuch)
    {
        HeroBase player = PlayerController.Player;
        Debug.Log(player.ability3Slider.value);
        if (player.ability3Slider.value < player.ability3Slider.maxValue)
        {
            player.ability3Slider.value += howmuch;
            player.ability3Charge = player.ability3Slider.value;
            Debug.Log(player.ability3Slider.value);
        }
    }
    void Start()
    { 
        if (heroWeaponPrefab != null)
        {
            primaryFireSpawnPos = heroWeaponPrefab.transform.GetChild(0);
        }
    }
    protected void Update()
    {
        base.Update();
        UpdateDurationSlider();
        if (ability1Timer < ability1Cooldown)
        {
            ability1Timer += Time.deltaTime;
        }
        if(ability2Timer < ability2Cooldown)
        {
            ability2Timer += Time.deltaTime;
        }
        else if(ability3Timer < ability3Cooldown)
        {
            ability3Timer += Time.deltaTime;
        }

        if (primaryFireTimer < recovery)
        {
            primaryFireTimer += Time.deltaTime;
        }
        if (secondaryFireTimer < recovery)
        {
            secondaryFireTimer += Time.deltaTime;
        }

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 bulletSpawnPos = PlayerController.Player.primaryFireSpawnPos.position;

        crosshairPos = Camera.main.ScreenToWorldPoint(screenCenter);

        Vector3 cameraPos = Camera.main.gameObject.transform.position;
        Vector3 cameraDirection = Camera.main.gameObject.transform.forward;

        //Vector3 gunAngle;
        //Debug.DrawLine(bulletSpawnPos, cameraPos + cameraDirection * 10.0f, Color.white);

        int layerMask = 2;
        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraDirection, out hit, 100.0f, layerMask))
        {
            Vector3 temp = hit.point - bulletSpawnPos;

            tempGunAngle = temp.normalized;

            //Debug.DrawLine(crosshairPos, hit.point, Color.red);

            //Debug.DrawLine(bulletSpawnPos, hit.point, Color.red);
            //Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
        else
        {
            Vector3 endpointPosition = cameraPos + cameraDirection * 100.0f;
            Vector3 bulletEndPointDistance = endpointPosition - bulletSpawnPos;
            tempGunAngle = bulletEndPointDistance.normalized;

        }
        if (tempGunAngle != Vector3.zero)
            Debug.DrawRay(bulletSpawnPos, tempGunAngle * 5, Color.yellow);
    }
    public virtual void PrimaryFire()
    {
        Debug.Log("M1");
    }

    public virtual void SecondaryFire()
    {
        Debug.Log("M2");
    }

    public virtual void Ability1()
    {
        Debug.Log("lshift");
    }

    public virtual void Ability2()
    {
        Debug.Log("e");
    }

    public virtual void Ability3()
    {
        Debug.Log("q");
    }
    public virtual void CollisionEnter(Collider other)
    {
        if (other.tag != CollisionPlayer.SpawnCollision)
        {
            return;
        }
        else
        {
            SpawnArea spawnArea = other.GetComponentInParent<SpawnArea>();
            spawnArea.EnteredSpawnArea();
            Debug.Log("in spawn");
        }
    }
    public virtual void CollisionExit(Collider other)
    {
        if (other.tag != CollisionPlayer.SpawnCollision)
        {
            return;
        }
        else
        {
            SpawnArea spawnArea = other.GetComponentInParent<SpawnArea>();
            spawnArea.ExitedSpawnArea();
            Debug.Log("out spawn");
        }
    }
    public virtual void Ability1Duration() { }
    void OnTriggerEnter(Collider other)
    {
        CollisionEnter(other);
    }
    void OnTriggerExit(Collider other) 
    {    
        CollisionExit(other);
    }
}
