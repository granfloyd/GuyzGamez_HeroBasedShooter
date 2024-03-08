using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBase : PlayerMovement
{
    [Header("   HeroBase")]    
    [SerializeField] protected float health;
    [SerializeField] protected float fireRate;
    //[Header("HitBoxes")]
    //[SerializeField] protected Collider BodyCollider;
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
        Debug.Log("LSHIFT: Ability 1");
    }

    public virtual void Ability2()
    {
        Debug.Log("E: Ability 2");
    }

    public virtual void Ability3()
    {
        Debug.Log("Q: Ability 3");
    }
    public virtual void CollisionEnter(Collider other) { }
    public virtual void CollisionExit(Collider other) { }
    
    void OnTriggerEnter(Collider other)
    {
        CollisionEnter(other);
    }
    void OnTriggerExit(Collider other) 
    {    
        CollisionExit(other);
    }
}
