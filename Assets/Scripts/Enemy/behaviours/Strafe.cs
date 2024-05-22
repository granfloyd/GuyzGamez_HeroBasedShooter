using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Strafe : BaseAIBehaviour
{
    private bool moveRight = true;
    private float strafeDistance = 1.0f;
    public override void UpdateBehaviour(EnemyBehaviour enemy)
    {
        if (!enemy.navAgent.enabled || enemy.targetPlayer == null) return;

        Vector3 strafeDirection = moveRight ? enemy.transform.right : -enemy.transform.right;
        Vector3 desiredPosition = enemy.transform.position + strafeDirection * strafeDistance;

        NavMeshHit hit;
        bool isDestinationBlocked = enemy.navAgent.Raycast(desiredPosition, out hit);

        if (!isDestinationBlocked)
        {
            enemy.navAgent.SetDestination(desiredPosition);
        }
        else
        {
            moveRight = !moveRight;
        }
    }
}
