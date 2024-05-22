using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MoveAwayFromTarget : BaseAIBehaviour
{
    public override void UpdateBehaviour(EnemyBehaviour enemy)
    {
        if(!enemy.navAgent.enabled) return;

        Vector3 desiredSpace;
        float distance = 1.0f;

        Vector3 targetPosition = enemy.targetPlayer.transform.position;
        Vector3 movebackDirection = (enemy.transform.position - targetPosition).normalized;

        //Check if away from player direction is valid
        NavMeshHit hit;
        desiredSpace = enemy.transform.position + movebackDirection * distance;

        bool isDestinationBlocked = enemy.navAgent.Raycast(desiredSpace, out hit);

        if (!isDestinationBlocked)
        {
            enemy.navAgent.SetDestination(desiredSpace);
            return;
        }
    }
}
