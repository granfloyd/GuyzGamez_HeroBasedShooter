using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class SpawnObject : NetworkBehaviour
{
    public enum ObjectType
    {
        Local,
        Netcode
    }

    public GameObject projectileLocalPrefab;
    public GameObject projectileNetcodePrefab;
    public LineRenderer lineRendererRM;//for rectangle man grapple

    public ClientProjectile localInstance;//chnage back to gamaobject todo
    public ClientProjectile netobjectInstance;//chnage back to gamaobject todo
    void Start()
    {
        if(lineRendererRM != null)
        lineRendererRM.enabled = false;
    }
    public void SpawnObjectLocal(ulong clientid, double roundedping, Vector3 position, Quaternion rotation)
    {
        if (NetworkManager.Singleton.IsServer) return;

        localInstance = null;
        HeroBase player = PlayerController.Player;
        GameObject localGameObject = Instantiate(projectileLocalPrefab, position, rotation);
        localInstance = localGameObject.GetComponent<ClientProjectile>();
        localInstance.ownerID = clientid;
    }

    [ServerRpc]
    public void SpawnObjectServerRpc(ulong clientid, Vector3 position, Quaternion rotation)
    {
        netobjectInstance = null;
        HeroBase player = PlayerController.Player;
        GameObject netcodeGameObject = Instantiate(projectileNetcodePrefab, position, rotation);
        netobjectInstance = netcodeGameObject.GetComponent<ClientProjectile>();

        NetworkObject networkobject = netobjectInstance.GetComponent<NetworkObject>();

        if (NetworkManager.Singleton.IsServer)
        {
            networkobject.SpawnWithOwnership(clientid);

            if (networkobject.GetComponent<ChainProjectile>() != null) return;

            if (!IsOwnedByServer) //hide CLONED OBJECT to the client
                networkobject.NetworkHide(clientid);
        }
        netobjectInstance.ownerID = clientid;
    }
    public void ObjectConfig(Vector3 velocity, float speed, int damage)
    {
        if (localInstance != null)
        {
            localInstance.SetMovement(velocity, speed);
            localInstance.SetDamage(damage);
        }

    }

    [ServerRpc]
    public void ObjectConfigServerRpc(Vector3 velocity, float speed, int damage)
    {
        if (netobjectInstance != null)
        {
            netobjectInstance.SetMovement(velocity, speed);
            netobjectInstance.SetDamage(damage);
        }
    }

    [ServerRpc]//this needs looking at
    public void SetGameObjectServerRpc(bool setActive)
    {
        if(setActive)
        {
            if (lineRendererRM != null)
                lineRendererRM.enabled = true;
        }
        else
        {
            if (lineRendererRM != null)
                lineRendererRM.enabled = false;
        }

    }
}
