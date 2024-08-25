using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.ProBuilder;

public class ChainTogether : AbilityBase
{
    public float projectileSpeed = 10f;

    public ChainTogether()
    {
        cooldown = 1f;
        duration = 0f;
        timer = cooldown;
        durationTimer = 0;
        isInstanced = false;
        isActive = false;
    }
    public override void Use()
    {
        if (IsReady())
        {
            base.Use();
            LaunchProjectile();
        }
    }

    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
    }
    private void LaunchProjectile()
    {
        HeroBase player = PlayerController.Player;

        //dont need local sim for this object in scene
        player.GetComponent<SpawnObject>().SpawnObjectServerRpc(
        NetworkManager.Singleton.LocalClientId,
        player.transform.position,
        player.orientation.localRotation.normalized);

        player.GetComponent<SpawnObject>().ObjectConfigServerRpc(
            player.tempGunAngle.normalized,
            projectileSpeed,
            0);
    }

    public override void End()
    {
        base.End();
    }

    public override bool IsReady()
    {
        return base.IsReady();
    }
}
