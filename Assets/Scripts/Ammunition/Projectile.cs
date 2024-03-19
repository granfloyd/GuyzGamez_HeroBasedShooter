using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public float Lifespan;
    public int Damage;
    public Rigidbody rb;
    public bool isAbility2;
    public float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float radius;
    [SerializeField] private float pullSpeed;//2 for soft || >3 cant escape
    void SetSpeedLifespanDamage(float speed,float lifespan,int damage)
    {
        Speed = speed;
        Lifespan = lifespan;
        Damage = damage;
    }
    void Start()
    {
        //SetSpeedLifespanDamage(5,20,10);       
        Destroy(gameObject, Lifespan);
    }

    void PullThemIn()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
        int playerLayer = LayerMask.NameToLayer("Player");
        int ignoreLayer = LayerMask.NameToLayer("IgnoreLayer");
        int layerMask = ~((1 << playerLayer) | (1 << ignoreLayer)); // will ignore the players layer and other ignore layers

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = transform.position - hitCollider.transform.position;
                rb.AddForce(direction.normalized * pullSpeed * rb.mass, ForceMode.Force);
                Debug.DrawLine(transform.position, hitCollider.transform.position, Color.red);
            }
        }
    }
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, Ground);
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
        if (isGrounded)
        {
            Debug.Log("Grounded)");
            PullThemIn();
        }
    }
    void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
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
            if(!isAbility2)
            {
                //PlayerController.Player.UpdateUltSlider(Damage);
                HealthScript enemyhp = other.gameObject.GetComponentInChildren<HealthScript>();
                enemyhp.TakeDamage(Damage);
                Destroy(gameObject);
            }
            
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
