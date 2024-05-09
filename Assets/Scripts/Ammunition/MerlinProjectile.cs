using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MerlinProjectile : GenericProjectile
{
    public bool isSecondaryFire;
    public ulong ownerID;
    void Start()
    {
        SetLifeSpan(2);
        rb = GetComponent<Rigidbody>();
        ServerDelete(false);
        Debug.Log("og owner "+ownerID);
    }

    public override void HandleCollision(Collision other)
    {
        if(IsServer)
        {
            if (other.gameObject.tag != "Player")
            {
                Debug.Log("Collided with " + other.gameObject.name);
                rb.velocity = Vector3.zero;
            }

            if (other.gameObject.tag == "Enemy1")
            {
                HeroUI.Instance.UpdateUltSlider(10);
                HealthScript enemyhp = other.gameObject.GetComponentInChildren<HealthScript>();
                enemyhp.CalculateDamage(damage);

                if (!isSecondaryFire)
                {
                    if (IsServer)
                    {
                        if (ownerID == 0)
                        {
                            PlayerController.Player.GetComponent<Merlin>().AddToRage(damage);
                            Debug.Log("sent rage to server");
                        }
                        else
                        {
                            ClientSendRageClientRpc(ownerID,damage);
                            Debug.Log("sent rage to client");
                        }
                    }
                }
                ServerDelete(true);
            }
        }
        
    }

    [ClientRpc]
    private void ClientSendRageClientRpc(ulong clientid,int damageToSend)
    {
        if(NetworkManager.Singleton.LocalClientId == clientid)
        {
            PlayerController.Player.GetComponent<Merlin>().AddToRage(damageToSend);
            Debug.Log("adding rage to this client"+ clientid);
        }
        

    }
    public override void HandleTrigger(Collider other)
    {

    }
}
