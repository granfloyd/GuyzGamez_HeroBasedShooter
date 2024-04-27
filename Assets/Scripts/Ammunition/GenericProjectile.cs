using Unity.Netcode;
using UnityEngine;

public class GenericProjectile : NetworkBehaviour
{
    public float speed;
    public float lifespan;
    public int damage;
    public Rigidbody rb;
    public void ServerDelete(bool deletenow)
    {
        if(IsServer)
        {
            if(deletenow)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, lifespan);
            }
            
        }
    }
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
    public void SetLifeSpan(float life)
    {
        lifespan = life;
    }

    public virtual void HandleCollision(Collision other) { }
    public virtual void HandleTrigger(Collider other) { }
    private void OnCollisionEnter(Collision other) { HandleCollision(other); }    
    private void OnTriggerEnter(Collider other) { HandleTrigger(other); }
    
}
