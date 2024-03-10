using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 50f;
    public float lifespan = 5f;
    public Rigidbody rb;

    [SerializeField] private bool isHomingType;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(isHomingType)
        {
            
        }
        else
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
            transform.position = worldCenter;
            rb.velocity = Camera.main.transform.forward * speed;
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
