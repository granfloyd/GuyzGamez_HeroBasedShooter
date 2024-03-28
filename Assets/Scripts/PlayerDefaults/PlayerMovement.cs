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
    [SerializeField] private LayerMask Ground;
    [SerializeField] private bool isGrounded;

    public Transform orientation;
    public Rigidbody rb;

    public float horizontalInput;
    public float verticalInput;

    Vector3 moveDirection;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected void Update()
    {
        if (!IsOwner) return;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, Ground);

        Debug.DrawRay(transform.position, Vector3.down * (playerHeight), Color.red);
        MyInput();
        SpeedControl();

        if(isGrounded)
        {
            networkAnimator.Animator.SetBool("isGrounded", true);
            networkAnimator.Animator.SetBool("isFalling", false);
        }
        else
        {
            networkAnimator.Animator.SetBool("isGrounded", false);
            networkAnimator.Animator.SetBool("isFalling", true);
        }
    }
    void FixedUpdate()
    {
        if (!IsOwner) return;
        MovePlayer();
    }
    public void MyInput()
    {
        

        HeroBase player = PlayerController.Player;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        networkAnimator.Animator.SetFloat("horz", horizontalInput);
        networkAnimator.Animator.SetFloat("vert", verticalInput);
        if (!player.isFlying)
        {
            if (Input.GetKey(jumpKey) && isReadyToJump && isGrounded)
            {
                Jump();
                Invoke("ResetJump", jumpCD);
            }
        }
        
        if (player.isFlying)
        {
            rb.drag = 1;
            if (isMovingUp)
            {
                MakePlayerMoveUp();
            }
            else if (isMovingDown)
            {
                MakePlayerMoveDown();
            }
        }
        else
        {
            if (isGrounded)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = 1;
            }
        }
    }

    private void MovePlayer()
    {
        if (!IsOwner) return;
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(moveDirection.magnitude > Vector3.zero.magnitude)
        {
            networkAnimator.Animator.SetBool("isMoving", true);
        }
        else
        {
            networkAnimator.Animator.SetBool("isMoving", false);
        }

        //on ground
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * groundDrag * rb.mass, ForceMode.Force);
        }
        //in air
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * rb.mass, ForceMode.Force);
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
        networkAnimator.Animator.SetBool("isJumping", true);
        isJumping = true;
        rb.drag = 1;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce * rb.mass, ForceMode.Impulse);
        isReadyToJump = false;
    }
    private void MakePlayerMoveUp()
    {
        rb.AddForce(Vector3.up * flySpeed, ForceMode.Force);
    }
    private void MakePlayerMoveDown()
    {
        rb.AddForce(Vector3.down * flySpeed, ForceMode.Force);
    }
    private void ResetJump()
    {
        networkAnimator.Animator.SetBool("isJumping", false);
        isReadyToJump = true;
        isJumping = false;
    }

}
