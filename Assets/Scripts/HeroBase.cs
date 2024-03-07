using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBase : PlayerMovement
{
    [SerializeField] protected float health;
    [SerializeField] protected float fireRate;

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
}
