using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : BaseAIBehaviour
{
    public override void UpdateBehaviour(EnemyBehaviour enemy)
    {
        if(enemy.targetPlayer == null) return;
        Vector3 playerPosition = enemy.targetPlayer.transform.position;
        if(enemy.navAgent.enabled) enemy.navAgent.SetDestination(playerPosition);
    }
}
