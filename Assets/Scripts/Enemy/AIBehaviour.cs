using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    public float viewDistance = 5f;
    public State currentState;

    public LayerMask PlayerLayer;
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public enum State
    {
        Idle,
        Patrol,
        Search,
        Found,
        Chase,
        Attack,
        Dead
    }
    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            SetState(State.Patrol);
        }
        else
        {
            SetState(State.Idle);
        }
    }
    public Vector3 PlayerPosition()
    {
        if (PlayerController.Player != null)
        {
            return targetPosition = PlayerController.Player.transform.position;
        }
        else
        {
            return targetPosition = Vector3.zero;
        }            
    }
    void Update()
    {
        //is player within enemy los distance 
        if (Vector3.Distance(transform.position, PlayerPosition()) <= viewDistance)
        {
            Vector3 directionToPlayer = (PlayerPosition() - transform.position).normalized;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, viewDistance, PlayerLayer))
            {
                SetState(State.Patrol);
                //Debug.Log("Player is not in sight");
                Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);
            }
            else//enemy has los on player
            {
                SetState(State.Chase);
                //Debug.Log("Player is in sight");
                Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.green);
            }
        }
        else if (currentState == State.Patrol)
        {
            MoveToTargetPosition(patrolPoints[currentPatrolIndex].position);
        }
    }
    void MoveToTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
        float speed = 5f;
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pos) < 0.1f && currentState == State.Patrol)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
    void SetState(State state)
    {
        currentState = state;

        switch (state)
        {
            case State.Idle:
                break;
            case State.Patrol:
                MoveToTargetPosition(patrolPoints[currentPatrolIndex].position);
                break;
            case State.Chase:
                MoveToTargetPosition(PlayerPosition());
                break;
            case State.Found:
                SetState(State.Chase);
                break;
            case State.Attack:
                break;
            case State.Dead:
                break;
        }
    }

}
