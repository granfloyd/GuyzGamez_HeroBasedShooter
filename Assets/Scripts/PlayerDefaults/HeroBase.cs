using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBase : PlayerMovement
{
    [Header("   HeroBase")]    
    [SerializeField] protected float health;
    [SerializeField] protected float fireRate;
    public bool isFlying = false;

    [Header("Ability1")]
    [SerializeField] public float ability1Cooldown = 7.0f;
    [SerializeField] public float ability1Duration = 5.0f;
    public float ability1Timer = 0.0f;

    [Header("Ability2")]
    [SerializeField] protected float ability2Cooldown = 0.0f;
    [SerializeField] protected float ability2Duration = 0.0f;
    protected float ability2Timer = 0.0f;

    [Header("Ability3")]
    [SerializeField] protected float ability3Cooldown = 0.0f;
    [SerializeField] protected float ability3Duration = 0.0f;
    protected float ability3Timer = 0.0f;

    void Update()
    {
        ability1Timer += Time.deltaTime;
        ability2Timer += Time.deltaTime;
        ability3Timer += Time.deltaTime;
        //Debug.Log("Ability1: " + ability1Timer);
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
