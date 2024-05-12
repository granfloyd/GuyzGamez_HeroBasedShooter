using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoolEffects : NetworkBehaviour
{
    public enum EffectIndex
    {
        bullethitground,
        bullethitenemy
    }
    public static CoolEffects Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [SerializeField] public ParticleSystem bulletHitGroundfx;
    [SerializeField] public ParticleSystem bulletHitEnemyfx;

    [ServerRpc]
    public void PlayCoolEffectServerRpc(EffectIndex ps, Vector3 where)
    {
        ParticleSystem fx;
        switch (ps)
        {
            case EffectIndex.bullethitground:
                fx = PlayCoolEffect(bulletHitGroundfx, where);
                PlayEffectAndDestroy(fx);//syncs / spawns this to clients 
                break;
            case EffectIndex.bullethitenemy:
                fx = PlayCoolEffect(bulletHitEnemyfx, where);
                PlayEffectAndDestroy(fx);
                break;
        }
    }

    private ParticleSystem PlayCoolEffect(ParticleSystem ps,Vector3 where)
    {
        ParticleSystem spawned = Instantiate(ps, where, Quaternion.identity);
        return spawned;
    }
    private void PlayEffectAndDestroy(ParticleSystem fx)
    {
        NetworkObject networkObject = fx.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(NetworkManager.LocalClientId);
        fx.Play();
        Destroy(fx.gameObject, fx.main.duration);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
