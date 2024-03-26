using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class HeroBase : PlayerMovement
{    
    [Header("   HeroBase")]
    [SerializeField] public HeroIndex heroId;
    [SerializeField] protected int health;
    [SerializeField] public float recovery;
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

    [Header("Ability1")]    
    [SerializeField] public float ability1Cooldown;
    [SerializeField] public float ability1Duration;
    public float ability1Timer;
    
    [Header("Ability2")] 
    [SerializeField] public float ability2Cooldown;
    [SerializeField] public float ability2Duration;
    public float ability2Timer;
    
    [Header("Ability3")]
    [SerializeField] public float ability3Charge;//called in heros damage scripts
    [SerializeField] public float ability3MaxCharge;
    [SerializeField] public float ability3Cooldown;
    [SerializeField] public float ability3Duration;
    public float ability3Timer;

    new protected void Update()
    {
        base.Update();       

        if (ability1Timer < ability1Cooldown)
        {
            ability1Timer += Time.deltaTime;
        }
        if(ability2Timer < ability2Cooldown)
        {
            ability2Timer += Time.deltaTime;
        }
        else if(ability3Timer < ability3Cooldown)
        {
            ability3Timer += Time.deltaTime;
        }

        if (primaryFireTimer < recovery)
        {
            primaryFireTimer += Time.deltaTime;
        }
        if (secondaryFireTimer < recovery)
        {
            secondaryFireTimer += Time.deltaTime;
        }
        if(Player != null && IsOwner)
        CalculateGunAngle();
    }
    private void CalculateGunAngle()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 bulletSpawnPos = PlayerController.Player.primaryFireSpawnPos.position;
        crosshairPos = Camera.main.ScreenToWorldPoint(screenCenter);
        Vector3 cameraPos = Camera.main.gameObject.transform.position;
        Vector3 cameraDirection = Camera.main.gameObject.transform.forward;
        int layerMask = 2;

        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraDirection, out hit, 100.0f, layerMask))
        {
            Vector3 temp = hit.point - bulletSpawnPos;
            tempGunAngle = temp.normalized;
        }
        else
        {
            Vector3 endpointPosition = cameraPos + cameraDirection * 100.0f;
            Vector3 bulletEndPointDistance = endpointPosition - bulletSpawnPos;
            tempGunAngle = bulletEndPointDistance.normalized;
        }

        if (tempGunAngle != Vector3.zero)
            Debug.DrawRay(bulletSpawnPos, tempGunAngle * 5, Color.yellow);
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
