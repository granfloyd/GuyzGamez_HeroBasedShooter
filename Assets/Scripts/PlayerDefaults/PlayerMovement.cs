using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("   PlayerMovement")]
    [SerializeField] private NetworkAnimator networkAnimator;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float flySpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCD;
    [SerializeField] private float airMultiplier;
    [SerializeField] private bool isReadyToJump;
    [SerializeField] public bool isJumping;
    [SerializeField] public bool isCrouching;    
    [SerializeField] public bool isMovingUp;
    [SerializeField] public bool isMovingDown;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;//half of the player's height
    RaycastHit hit;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private LayerMask Objective;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isOnObjective;

    public Transform orientation;
    
    public float horizontalInput;
    public float verticalInput;

    Vector3 moveDirection;

    [SerializeField] public CharacterController controller;
    [SerializeField] public float verticalVelocity;
    private float gravityValue = -9.81f;

    public AudioSource emoteMusic;
    public bool isEmoting;

    private void Start()
    {
        emoteMusic = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }
    protected void Update()
    {
        if (!IsOwner) return;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, Ground);

        isOnObjective = Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight ,Objective);

        Debug.DrawRay(transform.position, Vector3.down * (playerHeight), Color.red);

        MyInput();
        Animations();
    }
    
    void FixedUpdate()
    {
        if (!IsOwner) return;
        MovePlayer(PlayerController.Player.isAffectedByGravity);

        if (isOnObjective)
        {
            Objective objective = hit.collider.GetComponent<Objective>();
            objective.UpdateObjective(0.1f);
        }
    }

    public void MyInput()
    {
        HeroBase player = PlayerController.Player;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        networkAnimator.Animator.SetFloat("horz", horizontalInput);
        networkAnimator.Animator.SetFloat("vert", verticalInput);
        if (player.isAffectedByGravity)
        {
            if (Input.GetKey(jumpKey) && isReadyToJump && isGrounded)
            {
                Jump();
                Invoke("ResetJump", jumpCD);
            }
        }
        else
        {
            
        }
    }

    private void MovePlayer(bool isUsingGravity)
    {
        if (!IsOwner) return;

        if(isUsingGravity)
        {
            gravityValue = -9.81f;
        }
        else
        {
            gravityValue = 0f;
            verticalVelocity = 0f;
            if (PlayerController.Player.isFlying)
            {
                if (isMovingUp)
                {
                    verticalVelocity += moveSpeed;
                }
                if (isMovingDown)
                {
                    verticalVelocity -= moveSpeed;
                }
            }
        }

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(moveDirection.magnitude > Vector3.zero.magnitude)
        {
            networkAnimator.Animator.SetBool("isMoving", true);
        }
        else
        {
            networkAnimator.Animator.SetBool("isMoving", false);
        }
        if (isGrounded)
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(moveDirection * moveSpeed * airMultiplier * Time.deltaTime);

            if (verticalVelocity > gravityValue)
            {
                verticalVelocity += gravityValue * Time.deltaTime;
            }
        }
        controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    private void Jump()
    {
        networkAnimator.Animator.SetBool("isJumping", true);
        verticalVelocity = jumpForce;
        isReadyToJump = false;
    }

    private void ResetJump()
    {
        networkAnimator.Animator.SetBool("isJumping", false);
        isReadyToJump = true;
        isJumping = false;
    }

    void Animations()
    {
        if (isGrounded)
        {
            networkAnimator.Animator.SetBool("isGrounded", true);
            networkAnimator.Animator.SetBool("isFalling", false);
        }
        else
        {
            networkAnimator.Animator.SetBool("isGrounded", false);
            networkAnimator.Animator.SetBool("isFalling", true);
            networkAnimator.Animator.SetBool("isEmoting", false);
            isEmoting = false;
        }

        if (networkAnimator.Animator.GetBool("isEmoting") == false)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                networkAnimator.Animator.SetBool("isEmoting", true);
                Debug.Log("1 pressed");
                isEmoting = true;
                emoteMusic.Play();

            }
        }

        if (networkAnimator.Animator.GetBool("isEmoting") == true)
        {
            if (networkAnimator.Animator.GetBool("isMoving") == true)
            {
                Debug.Log("is moving stoping emoting");
                networkAnimator.Animator.SetBool("isEmoting", false);
                isEmoting = false;
                if (emoteMusic.isPlaying)
                {
                    emoteMusic.Stop();
                }
            }

        }
    }
}
