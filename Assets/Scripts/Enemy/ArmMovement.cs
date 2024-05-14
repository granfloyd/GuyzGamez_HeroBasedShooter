using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArmMovement : MonoBehaviour
{
    public State currentState;
    public Vector3 targetPosition;
    public Transform coreRotY;
    public Transform mainArmRotZ;
    private float coreMaxRotY = 180f;
    public float speed = 1f;
    public Transform eyePosition;
    public float raycastDistance = 10f;
    public LayerMask playerLayer;
    private bool isRotatingBack = false;
    public enum State
    {
        Idle,
        Patrol,
        Found,
    }
    void Start()
    {
        currentState = State.Patrol;
    }
    void Patrol()
    {
        if (Mathf.Abs(coreRotY.localEulerAngles.y - coreMaxRotY) < 1f)
        {
            // 50/50 chance to rotate back or continue rotating
            if (Random.value > 0.5f)
            {
                isRotatingBack = !isRotatingBack;
            }
            coreMaxRotY = isRotatingBack ? 0f : 360f;
        }

        // Rotate on Y
        coreRotY.localRotation = Quaternion.RotateTowards(coreRotY.localRotation, Quaternion.Euler(0, coreMaxRotY, 0), speed * Time.deltaTime);

        // Shoot a raycast from the eye position to check for player
        RaycastHit hit;
        if (Physics.Raycast(eyePosition.position, transform.forward, out hit, raycastDistance, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                currentState = State.Found;
            }
        }
    }
    void FollowPlayer()
    {
        Vector3 playerPosition = PlayerPosition();
        Vector3 directionToPlayer = playerPosition - coreRotY.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        coreRotY.rotation = Quaternion.RotateTowards(coreRotY.rotation, targetRotation, speed * Time.deltaTime);
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
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Found:
                FollowPlayer();
                break;
            default:
                break;
        }
    }
}
