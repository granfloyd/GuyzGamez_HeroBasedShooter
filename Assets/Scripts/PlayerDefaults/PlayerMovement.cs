using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("   PlayerMovement")]
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCD;
    [SerializeField] private float airMultiplier;
    [SerializeField] private bool isReadyToJump;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private bool isGrounded;

    public Transform orientation;
    public Rigidbody rb;

    public float horizontalInput;
    public float verticalInput;

    Vector3 moveDirection;

    public bool isCrouching = false;
    public bool isJumping = false;
    private void Start()
    {
        rb = gameObject.GetComponentInParent<Rigidbody>();
        rb.freezeRotation = true;
    }
    void Update()
    {
        if (isCrouching)
        {
            Debug.Log("Crouch started");
            rb.AddForce(Vector3.down * moveSpeed * 10f * Time.deltaTime, ForceMode.Impulse);
        }

        if (isJumping)
        {
            Debug.Log("Jump started");
            rb.AddForce(Vector3.up * moveSpeed * 10f * Time.deltaTime, ForceMode.Impulse);
        }

        //ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight + 0.5f, Ground);

        MyInput();
        SpeedControl();
    }
    void FixedUpdate()
    {
        MovePlayer();
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && isReadyToJump && isGrounded)
        {
            Jump();
            rb.drag = 0;
            isReadyToJump = false;
            
            Invoke(nameof(ResetJemp), jumpCD);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on ground
        if(isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!isGrounded) //in air
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);
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
    public void Jump()
    {
        Physics.gravity = new Vector3(0, -20, 0);
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce * 2, ForceMode.Impulse);
    }
   
    private void ResetJemp()
    {
        isReadyToJump = true;
        rb.drag = groundDrag;
    }
}
