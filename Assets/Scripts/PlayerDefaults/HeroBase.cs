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
    [SerializeField] protected float ability1Cooldown = 7.0f;
    [SerializeField] protected float ability1Duration = 5.0f;
    protected float ability1Timer = 0.0f;
    protected float ability1DurationTimer = 0.0f;
    protected bool ability1Active = false;   

    [Header("Ability2")]
    [SerializeField] protected float ability2Cooldown = 0.0f;
    [SerializeField] protected float ability2Duration = 0.0f;
    protected float ability2Timer = 0.0f;
    protected float ability2DurationTimer = 0.0f;
    protected bool ability2Active = false;

    [Header("Ability3")]
    [SerializeField] protected float ability3Cooldown = 0.0f;
    [SerializeField] protected float ability3Duration = 0.0f;
    protected float ability3Timer = 0.0f;
    protected float ability3DurationTimer = 0.0f;
    protected bool ability3Active = false;

    void Update()
    {
        ability1Timer += Time.deltaTime;
        ability2Timer += Time.deltaTime;
        ability3Timer += Time.deltaTime;

        if (ability1Active)
        {
            ability1DurationTimer += Time.deltaTime;
            if(ability1DurationTimer >= ability1Duration)
            {
                ability1Active = false;
                ability1DurationTimer = 0.0f;
                ability1Timer = 0.0f;
            }
           
        }
        if (ability2Active)
        {
            ability2DurationTimer += Time.deltaTime;
            if (ability2DurationTimer >= ability2Duration)
            {
                ability2Active = false;
                ability2DurationTimer = 0.0f;
                ability2Timer = 0.0f;
            }

        }
        if (ability3Active)
        {
            ability3DurationTimer += Time.deltaTime;
            if (ability3DurationTimer >= ability3Duration)
            {
                ability3Active = false;
                ability3DurationTimer = 0.0f;
                ability3Timer = 0.0f;
            }

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
        
        if (!ability1Active && ability1Timer >= ability1Cooldown)
        {
            Debug.Log("LSHIFT: Ability 1");
            ability1Active = true;
        }
    }

    public virtual void Ability2()
    {
        if (!ability2Active && ability2Timer >= ability2Cooldown)
        {
            Debug.Log("E: Ability 2");
            ability2Active = true;
        }
    }

    public virtual void Ability3()
    {
        if (!ability3Active && ability3Timer >= ability3Cooldown)
        {
            Debug.Log("Q: Ability 3");
            ability3Active = true;
        }
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
    
    void OnTriggerEnter(Collider other)
    {
        CollisionEnter(other);
    }
    void OnTriggerExit(Collider other) 
    {    
        CollisionExit(other);
    }
}
