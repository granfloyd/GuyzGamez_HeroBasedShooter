using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DamageMain : HeroBase
{
    private float boostForce = 40f;
   
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
        Debug.Log(PlayerController.Player.ToString());
        Debug.Log("LSHIFT: Ability 1: DamageMain");
    }
    void Boost()
    {
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

    private void Update()
    {
        //if(ability1Active)
        //{
        //    isFlying = true;
        //}
        //else
        //{
        //    isFlying = false;   
        //}
        //if(isFlying)
        //{
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        PlayerController.Player.transform.position += Vector3.up * 4 * Time.deltaTime;
        //    }
        //    if (Input.GetKey(KeyCode.LeftControl))
        //    {
        //        PlayerController.Player.transform.position += Vector3.down * 4 * Time.deltaTime;
        //    }
        //}
        
    }
}
