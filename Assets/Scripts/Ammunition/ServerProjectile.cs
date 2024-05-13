using Unity.Netcode;

public class ServerProjectile : NetworkBehaviour
{

    //public Rigidbody rb;
    //public bool isSecondaryFire;
    //public ulong ownerID;
    //public int damage;
    //public float lifespan = 5;
    //public float speed;
    //public Vector3 velocity;
    //
    //
    protected void Start()
    {
        NetworkObject networkObject = GetComponent<NetworkObject>();
        networkObject.AutoObjectParentSync = false;
        //rb = GetComponent<Rigidbody>();
        //Destroy(gameObject, lifespan);
    }
    //
    //public void SetDamage(int dmg)
    //{
    //    damage = dmg;
    //}
    //
    //public void SetLifeSpan(float life)
    //{
    //    lifespan = life;
    //}
    //
    //public void SetMovement(Vector3 dir, float spd)
    //{
    //    velocity = dir;
    //    speed = spd;
    //    rb.velocity = dir.normalized * spd;
    //}
    //public virtual void HandleCollision(Collision other) { }
    //public virtual void HandleTrigger(Collider other) { }
    //private void OnCollisionEnter(Collision other) { HandleCollision(other); }
    //private void OnTriggerEnter(Collider other) { HandleTrigger(other); }

    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag == "Enemy1")
    //    {
    //        if(other.gameObject.GetComponent<NetworkObject>() == null)
    //        {
    //            Debug.Log("No NetworkObject found on the object");
    //            return;
    //        }
    //        ulong objectid = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
    //        NetcodeSolutions netcodeSolutions = other.gameObject.GetComponent<NetcodeSolutions>();
    //        netcodeSolutions.ClientProjectileOnHit(objectid, transform.position,ownerID,damage);
    //        Destroy(gameObject);
    //    }
    //}

}
