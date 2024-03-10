using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMain : HeroBase
{
    private float boostForce = 20f;
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
        Boost();
    }
    void Boost()
    {
        Debug.Log("Boosting");
        //Physics.gravity = new Vector3(0, -20, 0);
        //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //rb.AddForce(Vector3.up * boostForce, ForceMode.Impulse);
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
