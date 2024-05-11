using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroBase : PlayerMovement
{    
    [Header("   HeroBase")]
    [SerializeField] public PlayerController.HeroIndex heroId;
    [SerializeField] protected int health;
    [SerializeField] public float recovery;
    [SerializeField] public float recovery2;
    [SerializeField] public bool isFlying = false;
    public Vector3 crosshairPos;
    public Vector3 tempGunAngle;
    public Transform weaponPos;
    public GameObject heroPrimaryFirePrefab;
    public Transform primaryFireSpawnPos;
    [HideInInspector] public GameObject weaponInstance = null;
    [HideInInspector] public GameObject bulletInstance = null;    

    [Header("PrimaryFire")]
    [SerializeField] public float primaryFireTimer;    

    [Header("SecondaryFire")]
    [SerializeField] public float secondaryFireTimer;

    public Ability baseAbility1 { get; set; }
    public Ability baseAbility2{ get; set; }
    public Ability baseAbility3{ get; set; }   
   
    [SerializeField] public float ability3Charge;
    public float ability3MaxCharge;
    new protected void Update()
    {
        base.Update();        
        if (primaryFireTimer < recovery)
        {
            primaryFireTimer += Time.deltaTime;
        }
        if (secondaryFireTimer < recovery2)
        {
            secondaryFireTimer += Time.deltaTime;
        }
        if(PlayerController.Player != null && IsOwner)
        CalculateGunAngle();
    }
    private void CalculateGunAngle()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 bulletSpawnPos = PlayerController.Player.primaryFireSpawnPos.position;
        crosshairPos = Camera.main.ScreenToWorldPoint(screenCenter);
        Vector3 cameraPos = Camera.main.gameObject.transform.position;
        Vector3 cameraDirection = Camera.main.gameObject.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraDirection, out hit, 100.0f))
        {
            Vector3 temp = hit.point - bulletSpawnPos;
            tempGunAngle = temp.normalized;

            //Debug.DrawRay(cameraPos, cameraDirection * hit.distance, Color.red);
        }
        else
        {
            Vector3 endpointPosition = cameraPos + cameraDirection * 100.0f;
            Vector3 bulletEndPointDistance = endpointPosition - bulletSpawnPos;
            tempGunAngle = bulletEndPointDistance.normalized;

            //Debug.DrawRay(cameraPos, cameraDirection * 100.0f, Color.green);
        }
    }
    public virtual void PrimaryFire()
    { 
        if (!IsOwner) return; 
        Debug.Log("M1");
    }

    public virtual void SecondaryFire()
    {
        if (!IsOwner) return;
        Debug.Log("M2");
    }

    public virtual void Ability1()
    {
        if (!IsOwner) return;
        Debug.Log("lshift");
    }

    public virtual void Ability2()
    {
        if (!IsOwner) return;
        Debug.Log("e");
    }

    public virtual void Ability3()
    {
        if (!IsOwner) return;
        Debug.Log("q");
    }
    public virtual void CollisionEnter(Collider other)
    {
        if(IsOwner)
        {
            if (other.tag != CollisionPlayer.SpawnCollision)
            {
                return;
            }
            else
            {
                SpawnArea spawnArea = other.GetComponentInParent<SpawnArea>();
                spawnArea.EnteredSpawnArea();
            }
        }        
    }
    public virtual void CollisionExit(Collider other)
    {
        if (IsOwner)
        {
            if (other.tag != CollisionPlayer.SpawnCollision)
            {
                return;
            }
            else
            {
                SpawnArea spawnArea = other.GetComponentInParent<SpawnArea>();
                spawnArea.ExitedSpawnArea();
            }
        }
            
    }
    public virtual void Ability1Duration() { }
    void OnTriggerEnter(Collider other)
    {
        CollisionEnter(other);
    }
    void OnTriggerExit(Collider other) 
    {    
        CollisionExit(other);
    }
}
