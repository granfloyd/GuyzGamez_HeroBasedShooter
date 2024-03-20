using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float Speed;
    public float Lifespan;
    public int Damage;
    public Rigidbody rb;
    void SetSpeedLifespanDamage(float speed,float lifespan,int damage)
    {
        Speed = speed;
        Lifespan = lifespan;
        Damage = damage;
    }
    void Start()
    {
        SetSpeedLifespanDamage(50,2,10);       
        Destroy(gameObject, Lifespan);
    }

    void OnCollisionEnter(Collision other)
    {
        HandleCollision(other);                
    }

    public virtual void HandleCollision(Collision other)
    {
        if (other.gameObject.tag != "Player")
        {
            rb.velocity = Vector3.zero;
        }

        if(other.gameObject.tag == "Enemy1")
        {
            PlayerController.Player.UpdateUltSlider(Damage);
            HealthScript enemyhp = other.gameObject.GetComponentInChildren<HealthScript>();
            enemyhp.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        HandleTrigger(other);
    }
    public virtual void HandleTrigger(Collider other)
    {
        if (other.gameObject.tag != "Enemy1")
        {
            return;
        }
        else
        {
            
        }
    }
}
