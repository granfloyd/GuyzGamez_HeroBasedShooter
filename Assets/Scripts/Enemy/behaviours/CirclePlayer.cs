using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CirclePlayer : BaseAIBehaviour
{
    public override void EnterBehaviour(EnemyBehaviour enemy)
    {
        enemy.angle = 0;
    }

    public override void UpdateBehaviour(EnemyBehaviour enemy)
    {
        float circleRadius = 8f;
        float circleSpeed = .3f;
        Vector3 playerPosition = enemy.targetPlayer.transform.position;

        // Calculate the new position
        float x = playerPosition.x + circleRadius * Mathf.Cos(enemy.angle);
        float z = playerPosition.z + circleRadius * Mathf.Sin(enemy.angle);

        Vector3 newPosition = new Vector3(x,playerPosition.y, z);

        // Move the enemy to the new position
        enemy.navAgent.SetDestination(newPosition);

        // Update the angle for the next frame
        enemy.angle += Time.deltaTime * circleSpeed;
    }
}
