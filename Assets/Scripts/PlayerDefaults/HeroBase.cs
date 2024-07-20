using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class HeroBase : PlayerMovement
{    
    [Header("   HeroBase")]
    [SerializeField] public PlayerController.HeroIndex heroId;
    [SerializeField] protected int maxHealth;
    [SerializeField] public float primaryRecovery;
    [SerializeField] public float secondaryRecovery;
    [SerializeField] public bool isFlying = false;
    [SerializeField] public bool isAffectedByGravity = true;

    public Vector3 crosshairPos;
    public Vector3 tempGunAngle;
    public Transform weaponPos;
    public GameObject heroPrimaryFirePrefab;
    public GameObject localGO;
    public Transform primaryFireSpawnPos;
    [HideInInspector] public GameObject weaponInstance = null;
    [HideInInspector] public GameObject bulletInstance = null;    

    [Header("PrimaryFire")]
    [SerializeField] public float primaryFireTimer;    

    [Header("SecondaryFire")]
    [SerializeField] public float secondaryFireTimer;

    public AbilityBase BaseAbility1 { get; set; }
    public AbilityBase BaseAbility2{ get; set; }
    public AbilityBase BaseAbility3{ get; set; }   
   
    [SerializeField] public float ability3Charge;
    public float ability3MaxCharge;
    public bool canGainUltCharge = true;
    new protected void Update()
    {
        base.Update();        
        if (primaryFireTimer < primaryRecovery)
        {
            primaryFireTimer += Time.deltaTime;
        }
        if (secondaryFireTimer < secondaryRecovery)
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
        }
        else
        {
            Vector3 endpointPosition = cameraPos + cameraDirection * 100.0f;
            Vector3 bulletEndPointDistance = endpointPosition - bulletSpawnPos;
            tempGunAngle = bulletEndPointDistance.normalized;
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

    public virtual void Ability1End()
    {
        if (!IsOwner) return;
        Debug.Log("lshift end");
        HeroBase player = PlayerController.Player;
        player.BaseAbility1.End();
    }

    public virtual void Ability2End()
    {
        if (!IsOwner) return;
        Debug.Log("e end");
        HeroBase player = PlayerController.Player;
        player.BaseAbility2.End();
    }

    public virtual void Ability3End()
    {
        if (!IsOwner) return;
        Debug.Log("q end");
        HeroBase player = PlayerController.Player;
        player.BaseAbility3.End();
    }
    public virtual void CollisionEnter(Collider other)
    {
        if(IsOwner)
        {
            if (other.tag == CollisionPlayer.SpawnCollision)
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
            if (other.tag == CollisionPlayer.SpawnCollision)
            {
                SpawnArea spawnArea = other.GetComponentInParent<SpawnArea>();
                spawnArea.ExitedSpawnArea();
            }
        }
            
    }

    void OnTriggerEnter(Collider other)
    {
        CollisionEnter(other);
    }
    void OnTriggerExit(Collider other) 
    {    
        CollisionExit(other);
    }
}
