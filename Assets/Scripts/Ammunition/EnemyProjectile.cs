using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyProjectile : ClientProjectile
{
    new void Start()
    {
        Debug.Log("EnemyProjectile Start");
        base.Start();
    }

    public override void HandleTrigger(Collider other)//client side uses trigger 
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<NetworkObject>() == null)
            {
                Debug.Log("No NetworkObject found on the object");
                return;
            }
            ulong objectid = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            NetcodeSolutions netcodeSolutions = other.gameObject.GetComponent<NetcodeSolutions>();
            netcodeSolutions.ClientProjectileOnHit(objectid, transform.position, ownerID, damage);
            Destroy(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public override void HandleCollision(Collision other)//server aka the HOST uses collision
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<NetworkObject>() == null)
            {
                Debug.Log("No NetworkObject found on the object");
                return;
            }
            ulong objectid = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            NetcodeSolutions netcodeSolutions = other.gameObject.GetComponent<NetcodeSolutions>();
            netcodeSolutions.ClientProjectileOnHit(objectid, transform.position, ownerID, damage);
            Destroy(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }
}