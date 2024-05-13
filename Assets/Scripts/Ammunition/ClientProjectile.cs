using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ClientProjectile : MonoBehaviour
{
    public Rigidbody rb;
    public bool isSecondaryFire;
    public ulong ownerID;
    public int damage;
    public float lifespan = 5;
    public float speed;     
    public Vector3 velocity;
    

    protected void Start()
    {  
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifespan);
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void SetLifeSpan(float life)
    {
        lifespan = life;
    }

    public void SetMovement(Vector3 dir,float spd)
    {
        velocity = dir;
        speed = spd;
        rb.velocity = dir.normalized * spd;
    }
    public virtual void HandleCollision(Collision other) { }
    public virtual void HandleTrigger(Collider other)
    {
        //if (other.gameObject.tag != "Enemy1")
        //{
        //    rb.velocity = Vector3.zero;
        //}
    }
    private void OnCollisionEnter(Collision other) { HandleCollision(other); }
    private void OnTriggerEnter(Collider other) { HandleTrigger(other); }
}
