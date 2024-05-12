using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MerlinProjectile : GenericProjectile
{
    public bool isSecondaryFire;
    public ulong ownerID;
    float movespeed = 0.5f;
    void Start()
    {
        SetLifeSpan(10);
        rb = GetComponent<Rigidbody>();
        ServerDelete(false,0);
        //Debug.Log("og owner "+ownerID);
    }
    private void Update()
    {
        movespeed += Time.deltaTime;
        if (IsServer)
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, movespeed))
            {
                Debug.Log("hit");
            }
            Debug.DrawRay(transform.position, transform.forward * movespeed , Color.red);
        }
    }
    public override void HandleCollision(Collision other)
    {
        if (IsServer)
        {
            if (other.gameObject.tag != "Enemy1")
            {
                Debug.Log("not enemy ");
                //Debug.Log("Collided with " + other.gameObject.name);
                rb.velocity = Vector3.zero;
                ServerDelete(true, 1);
            }
            else if (other.gameObject.tag == "Enemy1")
            {
                Debug.Log("enemy hit ");

                HealthScript enemyhp = other.gameObject.GetComponentInChildren<HealthScript>();
                enemyhp.CalculateDamage(damage);

                if (ownerID == 0)
                {
                    HeroUI.Instance.UpdateUltSlider(damage);
                }
                else
                {
                    ClientSendUltChargeClientRpc(ownerID, damage);
                }

                if (!isSecondaryFire)
                {
                    if (ownerID == 0)
                    {
                        PlayerController.Player.GetComponent<Merlin>().AddToRage(damage);
                    }
                    else
                    {
                        ClientSendRageClientRpc(ownerID, damage);
                    }
                }
                ServerDelete(true, 2);
            }
        }

    }

    [ClientRpc]
    private void ClientSendRageClientRpc(ulong clientid, int damageToSend)
    {
        if(NetworkManager.Singleton.LocalClientId == clientid)
        {
            PlayerController.Player.GetComponent<Merlin>().AddToRage(damageToSend);
            Debug.Log("adding rage to this client"+ clientid);
        } 
    }

    [ClientRpc]
    private void ClientSendUltChargeClientRpc(ulong clientid, float amountToSend)
    {
        if(NetworkManager.Singleton.LocalClientId == clientid)
        {
            HeroUI.Instance.UpdateUltSlider(amountToSend);
        }
    }
    public override void HandleTrigger(Collider other)
    {

    }
}
