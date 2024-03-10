using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DamageMain : HeroBase
{
    public float boostForce = 40f;
   
    private void Start()
    {
        Debug.Log("dmgmain");

        PlayerController.Player.orientation = gameObject.transform.parent;
        PlayerController.Player.rb = gameObject.GetComponentInParent<Rigidbody>();
    }
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
        Boost();
        Debug.Log("LSHIFT: Ability 1: DamageMain");
        Invoke("Ability1Duration", ability1Duration);
    }
    public override void Ability1Duration()
    {
        Debug.Log("calling this");
        if(PlayerController.Player.isFlying)
        {
            PlayerController.Player.rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            PlayerController.Player.isFlying = false;
            PlayerController.Player.rb.useGravity = true;
        }        
    }
    void Boost()
    {
        PlayerController.Player.rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        PlayerController.Player.isFlying = true;
        PlayerController.Player.rb.AddForce(Vector3.up * boostForce, ForceMode.Impulse);
        PlayerController.Player.rb.useGravity = false;
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
