using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("   PlayerMovement")]
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
        rb.freezeRotation = true;
    }
    protected void Update()
    {
        //ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, Ground);
        // Draw the raycast in the scene view
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight), Color.red);
        MyInput();
        SpeedControl();
    }
    void FixedUpdate()
    {
        MovePlayer();
    }
    public void MyInput()
    {
        HeroBase player = PlayerController.Player;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(player == null)
        {
            return;            
        } 
        else if (!player.isFlying)
        {
            if (Input.GetKey(jumpKey) && isReadyToJump && isGrounded)
            {
                Jump();
                Invoke("ResetJump", jumpCD);
            }
        }
       
        if (player.isFlying)
        {
            player.rb.drag = 1;
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
            if(player.isGrounded)
            {
                player.rb.drag = groundDrag;
            }
            else
            {
                player.rb.drag = 1;
            }
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //on ground
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * groundDrag * rb.mass, ForceMode.Force);
        }
        else if (!isGrounded) //in air
        {
            if (PlayerController.Player != null)
            {
                if (PlayerController.Player.isFlying)
                {
                    Debug.Log("doing it right");
                    rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * rb.mass, ForceMode.Force);
                }
                else
                {
                    rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * rb.mass, ForceMode.Force);
                }
            }
        }
    }
    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, limitedVel.y, limitedVel.z);
        }
    }
    private void Jump()
    {
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
        isReadyToJump = true;
        isJumping = false;
    }
   
}
