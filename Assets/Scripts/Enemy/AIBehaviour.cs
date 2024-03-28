using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    private float stateChangeCooldown;
    private float stateChangeTimer;
    public bool isRanged;
    private bool isReadyToChangeState;
    private Vector3 targetPosition;

    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }
    void Start()
    {
        isReadyToChangeState = false;
        stateChangeTimer = 0;
        stateChangeCooldown = 5;
    }

    void Update()
    {
        if (stateChangeTimer < stateChangeCooldown)
        {
            stateChangeTimer += Time.deltaTime;
        }
        else
        {
            isReadyToChangeState = true;
        }

        FindTarget();
    }
    void FindTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            // Draw a green ray for visualization
            Debug.DrawRay(transform.position, transform.forward * 100, Color.green);
            if (hit.transform.tag == "Player")
            {
                if(!isRanged)
                {
                    SetState(State.Chase);
                }
                else
                {
                    SetState(State.Attack);
                }
                
            }
        }
        else
        {
            // Draw a red ray for visualization
            Debug.DrawRay(transform.position, transform.forward * 100, Color.red);
            SetState(State.Patrol);
        }
    }
    void ImComingForYou()
    {

    }
    void SetState(State state)
    {
        //if (!isReadyToChangeState) { return;}

        switch (state)
        {
            case State.Idle:
                break;
            case State.Patrol:
                break;
            case State.Chase:
                ImComingForYou();
                break;
            case State.Attack:
                break;
            case State.Dead:
                break;
        }
    }

}
