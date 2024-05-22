using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDrone : EnemyBehaviour
{
    [SerializeField] public Transform[] Projectile1SpawnPoint;//L-R | 0-1
    [SerializeField] public Transform[] Projectile2SpawnPoint;
    public int flip = 0;
    protected override void Start()
    {
        base.Start();
        maxDistanceFromPlayer = 15;
        minDistanceFromPlayer = 10;
        recovery = 1.5f;
    }

    protected override void DetermineBehaviour()
    {
        if (targetPlayer == null) return;

        if(recoveryTimer >= recovery)
        {
            MaybeMakeAttackClassThing();
            recoveryTimer = 0;

            return;
        }
        Vector3 targetPosition = targetPlayer.transform.position;
        Vector3 enemySelfPosition = transform.position;

        bool isEnemyFollowingTarget = Vector3.Distance(targetPosition, enemySelfPosition) > maxDistanceFromPlayer;
        bool isEnemyMovingBack = Vector3.Distance(targetPosition, enemySelfPosition) < minDistanceFromPlayer;

        if (isEnemyFollowingTarget)
            SwitchBehaviour(AIBehaviour.moveToTarget);
        else if (isEnemyMovingBack)
            SwitchBehaviour(AIBehaviour.moveAwayFromTarget);
        else
            SwitchBehaviour(AIBehaviour.strafe);

    } 

    void MaybeMakeAttackClassThing()
    {
        Debug.Log("calling dis22");
        Vector3 directionToPlayer = (targetPlayer.transform.position - transform.position).normalized;
        GameObject projectileInstance = Instantiate(Projectile1Prefab, Projectile1SpawnPoint[flip].position, transform.localRotation);
        ServerProjectile serverProjectileComponent = projectileInstance.GetComponent<ServerProjectile>();
        serverProjectileComponent.NetworkObject.Spawn();
        EnemyProjectile enemyProjectile = projectileInstance.GetComponent<EnemyProjectile>();
        enemyProjectile.ownerID = NetworkObject.NetworkObjectId;
        if (flip == 0)
        {
            flip = 1;
        }
        else
        {
            flip = 0;
        }
        enemyProjectile.SetLifeSpan(5f);
        enemyProjectile.SetMovement(directionToPlayer, 10f );
    }
}
