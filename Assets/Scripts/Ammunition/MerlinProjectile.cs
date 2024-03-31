using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MerlinProjectile : GenericProjectile
{
    void Start()
    {
        SetSLD(100, 2, 10);
        rb = GetComponent<Rigidbody>();
        ServerDelete(false);
        
    }

    public override void HandleCollision(Collision other)
    {
        if (other.gameObject.tag != "Player")
        {
            rb.velocity = Vector3.zero;
        }

        if(other.gameObject.tag == "Enemy1")
        {
            HeroUI.Instance.UpdateUltSlider(10);
            HealthScript enemyhp = other.gameObject.GetComponentInChildren<HealthScript>();
            enemyhp.CalculateDamage(damage);
            ServerDelete(true);
        }
    }
    public override void HandleTrigger(Collider other)
    {

    }
}
