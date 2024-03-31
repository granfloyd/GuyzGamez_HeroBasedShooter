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
    public void SetSLD(float Speed,float Lifespan,int Damage)
    {
        speed = Speed;
        lifespan = Lifespan;
        damage = Damage;
    }
    public virtual void HandleCollision(Collision other) { }
    public virtual void HandleTrigger(Collider other) { }
    private void OnCollisionEnter(Collision other) { HandleCollision(other); }    
    private void OnTriggerEnter(Collider other) { HandleTrigger(other); }
    
}
