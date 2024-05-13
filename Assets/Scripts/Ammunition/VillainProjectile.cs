using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VillainProjectile : ClientProjectile
{
    new void Start()
    {
        base.Start();
    }
    public override void HandleTrigger(Collider other)//client side uses trigger 
    {
        if (other.gameObject.tag == "Enemy1")
        {
            if(other.gameObject.GetComponent<NetworkObject>() == null)
            {
                Debug.Log("No NetworkObject found on the object");
                return;
            }
            ulong objectid = other.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            NetcodeSolutions netcodeSolutions = other.gameObject.GetComponent<NetcodeSolutions>();
            netcodeSolutions.ClientProjectileOnHit(objectid, transform.position,ownerID,damage);
            Destroy(gameObject);

            ClientSendUltChargeClientRpc(ownerID, damage);
            ClientSendRageClientRpc(ownerID, damage);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void HandleCollision(Collision other)//server aka the HOST uses collision
    {
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
            Destroy(gameObject);

            HeroUI.Instance.UpdateUltSlider(damage);

            if (!isSecondaryFire)
            {
                PlayerController.Player.GetComponent<Villain>().AddToRage(damage);
            }            
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [ClientRpc]
    private void ClientSendRageClientRpc(ulong clientid, int damageToSend)
    {
        if (NetworkManager.Singleton.LocalClientId == clientid)
        {
            PlayerController.Player.GetComponent<Villain>().AddToRage(damageToSend);
            Debug.Log("adding rage to this client" + clientid);
        }
    }

    [ClientRpc]
    private void ClientSendUltChargeClientRpc(ulong clientid, float amountToSend)
    {
        if (NetworkManager.Singleton.LocalClientId == clientid)
        {
            HeroUI.Instance.UpdateUltSlider(amountToSend);
        }
    }
}
