using Unity.Netcode;

public class ServerProjectile : NetworkBehaviour
{
    protected void Start()
    {
        NetworkObject networkObject = GetComponent<NetworkObject>();
        networkObject.AutoObjectParentSync = false;
        if(IsServer)
        {
            if(!networkObject.IsSpawned)
            networkObject.Spawn();
        }
    }

}
