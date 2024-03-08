using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMain : HeroBase
{
    public override void PrimaryFire()
    {
        Debug.Log("M1: DamageMain");
    }

    public override void SecondaryFire()
    {
        Debug.Log("M2: DamageMain");
    }

    public override void Ability1()
    {
        Debug.Log("LSHIFT: Ability 1: DamageMain");
    }

    public override void Ability2()
    {
        Debug.Log("E: Ability 2: DamageMain");
    }

    public override void Ability3()
    {
        Debug.Log("Q: Ability 3: DamageMain");
    }
}
