using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetcodeSolutions : NetworkBehaviour
{
    public Vector3 hitLocation;
    private enum NetworkObjectType
    {
        Player,
        Enemy,
        Objective,
        Projectile,
        Shield
    }

    [SerializeField] private NetworkObjectType type;

    [ServerRpc(RequireOwnership = false)]
    public void ServerInfoFromClientServerRpc(ulong objectid, Vector3 whereWasHit, ulong owner, int clientDamage)
    {
        if (IsObjectInScene(objectid))
        {
            Debug.Log("Object is in scene");
            switch (type)
            {
                case NetworkObjectType.Player:
                    Debug.Log("player hit");
                    //Do something for when player hit
                    break;
                case NetworkObjectType.Enemy:
                    CoolEffects.Instance.PlayCoolEffectServerRpc(CoolEffects.EffectIndex.bullethitground, whereWasHit);
                    HealthScript enemyhp = gameObject.GetComponentInChildren<HealthScript>();
                    enemyhp.CalculateDamage(clientDamage);
                    break;
                case NetworkObjectType.Objective:
                    CoolEffects.Instance.PlayCoolEffectServerRpc(CoolEffects.EffectIndex.bullethitground, whereWasHit);
                    HealthScript objectivehp = gameObject.GetComponent<HealthScript>();
                    objectivehp.CalculateDamage(clientDamage);
                    break;
                case NetworkObjectType.Projectile:
                    //why do i have this
                    break;
                case NetworkObjectType.Shield:
                    //Do something for when shield hit
                    break;
            }
        }
    }
    public void ClientProjectileOnHit(ulong objectid, Vector3 whereWasHit, ulong ownerId, int damageToSend)
    {
        if(!IsObjectInScene(objectid))
        {
            Debug.Log("Object cant be found");
            return;
        }
        ServerInfoFromClientServerRpc(objectid,whereWasHit,ownerId,damageToSend);
    }

    public bool IsObjectInScene(ulong objectid)
    {
        bool isObjectInScene = NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(objectid);
        return isObjectInScene;
    }

}
