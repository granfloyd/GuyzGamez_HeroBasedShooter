using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerProjectile : NetworkBehaviour
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

}
