using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.ProBuilder;

public class ChainProjectile : ClientProjectile
{
    private Vector3 initialVelocity;
    private float gravity = -9.81f;
    private float flightTime;

    new void Start()
    {
        ParabolicTrajectory();
        if (NetworkManager.Singleton.IsServer)
        {
            base.Start();
        }        
    }
    private void ParabolicTrajectory()
    {
        HeroBase player = PlayerController.Player;
        Vector3 direction = velocity;
        float distance = 20f;
        float angle = 25f * Mathf.Deg2Rad;

        initialVelocity = new Vector3(direction.x * Mathf.Cos(angle), Mathf.Sin(angle), direction.z * Mathf.Cos(angle)) * speed;
        flightTime = distance / (speed * Mathf.Cos(angle));

    }
    private void UpdateProjectile()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x += initialVelocity.x * Time.deltaTime;
        currentPosition.z += initialVelocity.z * Time.deltaTime;
        currentPosition.y += initialVelocity.y * Time.deltaTime + 0.1f * gravity * Mathf.Pow(Time.deltaTime, 2);

        transform.position = currentPosition;
    }
    private void FixedUpdate()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            
            UpdateProjectile();
        }
    }
    
    public override void HandleTrigger(Collider other)//client side uses trigger 
    {
        if (other.gameObject.tag == "Enemy1")
        {
            Debug.Log("Hit Enemy client trigger");
            if (other.gameObject.GetComponent<NetworkObject>() == null)
            {
                Debug.Log("No NetworkObject found on the object");
                return;
            }
            ulong objectid = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            NetcodeSolutions netcodeSolutions = other.gameObject.GetComponent<NetcodeSolutions>();
            netcodeSolutions.ClientProjectileOnHit(objectid, transform.position, ownerID, damage);
            //Destroy(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public override void HandleCollision(Collision other)//server 
    {
        if (ownerID != 0) return;

        if (other.gameObject.tag == "Enemy1")
        {
            if (other.gameObject.GetComponent<NetworkObject>() == null)
            {
                Debug.Log("No NetworkObject found on the object");
                return;
            }
            ulong objectid = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            NetcodeSolutions netcodeSolutions = other.gameObject.GetComponent<NetcodeSolutions>();
            netcodeSolutions.ClientProjectileOnHit(objectid, transform.position, ownerID, damage);


            HeroUI.Instance.UpdateUltSlider(damage);

            if (!isSecondaryFire)
            {
                PlayerController.Player.GetComponent<Villain>().AddToRage(damage);
            }
            //Destroy(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }
}
