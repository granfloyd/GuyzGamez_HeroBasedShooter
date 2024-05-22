using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootProjectileAtTarget : BaseAIBehaviour
{
    
    public override void EnterBehaviour(EnemyBehaviour enemy)
    {
        //AirDrone airDrone = enemy as AirDrone;
        //if (airDrone == null) return;
        //
        //Debug.Log("calling dis");
        //GameObject projectileInstance = Instantiate(airDrone.Projectile1Prefab, airDrone.Projectile1SpawnPoint[0].position, Quaternion.identity);
        //ServerProjectile serverProjectileComponent = projectileInstance.GetComponent<ServerProjectile>();
        //serverProjectileComponent.NetworkObject.Spawn();
        //EnemyProjectile enemyProjectile = projectileInstance.GetComponent<EnemyProjectile>();
        //enemyProjectile.ownerID = airDrone.NetworkObject.NetworkObjectId;
        //if(airDrone.flip == 0)
        //{
        //    airDrone.flip = 1;
        //}
        //else
        //{
        //    airDrone.flip = 0;
        //}
        //enemyProjectile.SetLifeSpan(5f);
        //enemyProjectile.SetMovement(enemyProjectile.transform.forward, 10f);

    }

    public override void UpdateBehaviour(EnemyBehaviour enemy)
    {

    }
}
