using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private bool chargeForward = false;
    [SerializeField] private MoveStates moveStates;
    [SerializeField] private float dashForce = 200;
    [SerializeField] private float forwardForce = 500;
    [SerializeField] private float maxForwardVelocity = 15;
    [SerializeField] private float forwardChargeForce = 1000;
    [SerializeField] private float maxForwardChargeVelocity = 20;
    
    [Header("Turning")]
    [SerializeField] private float turnForce = 90;
    [SerializeField] private float turningSpeed = 50;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 1500;
    
    private float horizontalInput;
    private float verticalInput;

    private Rigidbody rb;
    
    private enum MoveStates
    {
        running, sprinting 
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveStates = MoveStates.running;
    }

    private void Update()
    {
        GetInput();
        Jump();
        ChangeStates();
        
        if(Input.GetKey(KeyCode.LeftShift))
        {
            chargeForward = true;
        }
        else
        {
            chargeForward = false;
        }
    }

    private void FixedUpdate()
    {
        Turning();
        Accelerate();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void Accelerate()
    {
        if (moveStates == MoveStates.running)
        {
            ForceForward(1, forwardForce, maxForwardVelocity, ForceMode.Acceleration);
        }
        else if(moveStates == MoveStates.sprinting && !chargeForward)
        {
            ForceForward(1.5f, forwardForce, maxForwardVelocity, ForceMode.Acceleration);
        } 
        
        if(moveStates == MoveStates.sprinting && chargeForward)
        {
            Debug.Log("ok");
            ForceForward(1, forwardChargeForce, maxForwardChargeVelocity, ForceMode.Acceleration);
        }
    }

    private void Turning()
    {
        if (moveStates == MoveStates.running)
        {
            transform.rotation *= Quaternion.Euler(0f, turningSpeed * horizontalInput * Time.deltaTime, 0f);
        }
        else if(moveStates == MoveStates.sprinting)
        {
            rb.AddTorque(new Vector3(0f, turnForce * horizontalInput * Time.deltaTime, 0f), ForceMode.Impulse);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0)
        {
            Debug.Log("space");
            rb.AddForce(transform.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y > 0)
        {
            Debug.Log("space 2");
            float dashTotalForce = dashForce * transform.InverseTransformDirection(rb.velocity).z;

            rb.AddRelativeForce(new Vector3(0, 0, dashTotalForce * Time.deltaTime), ForceMode.Impulse);
        }
    }

    private void ChangeStates()
    {
        if (transform.InverseTransformDirection(rb.velocity).z < maxForwardVelocity * 0.33f) //3rd of the speed
        {
            moveStates = MoveStates.running;
        }
        else
        {
            moveStates = MoveStates.sprinting;
        }
    }

    private void ForceForward(float multiplier, float force, float maxSpeed, ForceMode forceMode)
    {
        if(transform.InverseTransformDirection(rb.velocity).z < maxSpeed)
            rb.AddRelativeForce(new Vector3(0, 0, verticalInput * force * multiplier * Time.deltaTime), forceMode);
    }
    
    private void OnDrawGizmos()
    {
        /*Handles.Label(new Vector3(transform.position.x, transform.position.y, transform.position.z), 
            "Forward Velocity: " + transform.InverseTransformDirection(rb.velocity).z);*/
    }
}
