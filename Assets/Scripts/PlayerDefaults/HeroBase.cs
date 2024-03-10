using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBase : PlayerMovement
{
    [Header("   HeroBase")]
    [SerializeField] protected int health;
    public bool isFlying = false;
    [SerializeField] public float recovery = 0.5f;
    [SerializeField] public float recovery2 = 1.0f;
    public GameObject heroWeaponPrefab;
    public Transform weaponPos;
    public GameObject heroPrimaryFirePrefab;
    public Transform primaryFireSpawnPos;
    [HideInInspector] public GameObject weaponInstance = null;
    [HideInInspector] public GameObject bulletInstance = null;

    [Header("PrimaryFire")]
    [SerializeField] public float primaryFireTimer = 0.0f;    

    [Header("SecondaryFire")]
    [SerializeField] public float secondaryFireTimer = 0.0f;

    [Header("Ability1")]
    
    [SerializeField] public float ability1Cooldown = 7.0f;
    [SerializeField] public float ability1Duration = 5.0f;
    public float ability1Timer = 0.0f;

    [Header("Ability2")]
    [SerializeField] protected GameObject ability2Prefab;
    [HideInInspector] public GameObject ability2Instance = null;    
    [SerializeField] public float ability2Cooldown = 0.0f;
    [SerializeField] public float ability2Duration = 0.0f;
    public float ability2Timer = 0.0f;

    [Header("Ability3")]
    [SerializeField] public float ability3Cooldown = 0.0f;
    [SerializeField] public float ability3Duration = 0.0f;
    public float ability3Timer = 0.0f;
    void Start()
    {
        if (heroWeaponPrefab != null)
        {
            primaryFireSpawnPos = heroWeaponPrefab.transform.GetChild(0);
        }
    }
    void Update()
    {
        if(ability1Timer < ability1Cooldown)
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
        if (secondaryFireTimer < recovery2)
        {
            secondaryFireTimer += Time.deltaTime;
        }
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
