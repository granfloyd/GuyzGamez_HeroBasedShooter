using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ClientProjectile : MonoBehaviour
{
    public ulong ownerID;
    public bool isSecondaryFire;
    public float speed;
    public float lifespan = 10.0f;
    public int damage;
    public Rigidbody rb;    
    
    private void Start()
    {  
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy1")
        {
            if(other.gameObject.GetComponent<NetworkObject>() == null)
            {
                Debug.Log("No NetworkObject found on the object");
                return;
            }
            ulong objectid = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            NetcodeSolutions netcodeSolutions = other.gameObject.GetComponent<NetcodeSolutions>();
            netcodeSolutions.ClientProjectileOnHit(objectid, transform.position,ownerID,damage);
            Destroy(gameObject);
        }
    }
}
