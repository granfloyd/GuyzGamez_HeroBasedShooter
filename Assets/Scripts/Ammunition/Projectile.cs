using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 50f;
    public float lifespan = 5f;
    public Rigidbody rb;
    
    [SerializeField] private bool isHomingType;
    void Start()
    {
        //Vector3 tempAngle = Aerial.tempGunAngle;
        rb = GetComponent<Rigidbody>();
        if(isHomingType)
        {
            
        }
        else
        {

        }
        
        Destroy(gameObject, lifespan);
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
