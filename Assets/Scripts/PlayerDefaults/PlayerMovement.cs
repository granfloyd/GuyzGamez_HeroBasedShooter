using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("   PlayerMovement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float flySpeed;
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
        //rb = gameObject.GetComponentInParent<Rigidbody>();
        //rb.freezeRotation = true;
    }
    protected void Update()
    {
        
        //ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, Ground);
        // Draw the raycast in the scene view
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight), Color.red);
        MyInput();
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
           if (isMovingUp)
           {
               MakePlayerMoveUp();
           }
           else if (isMovingDown)
           {
               MakePlayerMoveDown();
           }
       }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on ground
        if (isGrounded)
        {
            transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
        else if(!isGrounded) //in air
        {
            transform.Translate(moveDirection.normalized * moveSpeed * airMultiplier * Time.deltaTime, Space.World);
        }   

    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isReadyToJump = false;
    }
    private void MakePlayerMoveUp()
    {
        transform.Translate(Vector3.up * flySpeed * Time.deltaTime, Space.World);
        //transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
    private void MakePlayerMoveDown()
    {
        transform.Translate(Vector3.down * flySpeed * Time.deltaTime, Space.World);
    }
    private void ResetJump()
    {
        isReadyToJump = true;
    }
   
}
