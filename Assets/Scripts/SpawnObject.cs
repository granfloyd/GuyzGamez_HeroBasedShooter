using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class SpawnObject : NetworkBehaviour
{
    public GameObject projectileLocalPrefab;
    public GameObject projectileNetcodePrefab;
    public LineRenderer lineRendererRM;//for rectangle man grapple
    void Start()
    {
        if(lineRendererRM != null)
        lineRendererRM.enabled = false;
    }
    public void SpawnObjectLocal(ulong clientid, Vector3 position, Quaternion rotation)
    {
        HeroBase player = PlayerController.Player;
        if (NetworkManager.Singleton.IsServer) return; 

        GameObject gameobject = Instantiate(projectileLocalPrefab, position, rotation);

        if(gameobject.GetComponent<ClientProjectile>() != null)
        gameobject.GetComponent<ClientProjectile>().SetMovement(player.tempGunAngle.normalized, 20f);
    }

    [ServerRpc]
    public void SpawnObjectServerRpc(ulong clientid, Vector3 position, Quaternion rotation)
    {
        HeroBase player = PlayerController.Player;
        GameObject gameobject = Instantiate(projectileNetcodePrefab, position, rotation);

        NetworkObject networkobject = gameobject.GetComponent<NetworkObject>();
        if (NetworkManager.Singleton.IsServer)
        {
            networkobject.SpawnWithOwnership(clientid);

            if (!IsOwnedByServer) //hide CLONED OBJECT to the client
                networkobject.NetworkHide(clientid);
        }

        if (gameobject.GetComponent<ClientProjectile>() != null)
            gameobject.GetComponent<ClientProjectile>().SetMovement(player.tempGunAngle.normalized, 20f);
    }

    [ServerRpc]
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
