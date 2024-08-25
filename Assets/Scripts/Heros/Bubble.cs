using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private float centerHeight;
    [SerializeField] private bool isGrounded;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, centerHeight);
        Debug.DrawRay(transform.position, Vector3.down * (centerHeight), Color.red);

        if(isGrounded)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }

    }
}
