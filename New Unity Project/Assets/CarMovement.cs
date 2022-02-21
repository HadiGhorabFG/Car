using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float maxSteerAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 50f;
    public float brakeSpeed = 50f;
    public WheelCollider wheelOne, wheelTwo, wheelThree, wheelFour;
    public Transform wheelOneT, wheelTwoT, wheelThreeT, wheelFourT;
    
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;
 
    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        Brakes();
        UpdateWheelPoses();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput;
        wheelOne.steerAngle = steeringAngle;
        wheelTwo.steerAngle = steeringAngle;
    }

    private void Accelerate()
    {
        wheelOne.motorTorque = motorForce * verticalInput;
        wheelTwo.motorTorque = motorForce * verticalInput;
    }

    private void Brakes()
    {
        if (Input.GetKey(KeyCode.Space) && wheelThree.brakeTorque < brakeForce)
        {
            float currentBrakeForce = 0;
            
            wheelOne.brakeTorque += currentBrakeForce + Time.deltaTime * brakeSpeed;
            wheelTwo.brakeTorque += currentBrakeForce + Time.deltaTime * brakeSpeed;
            wheelThree.brakeTorque += currentBrakeForce + Time.deltaTime * brakeSpeed;
            wheelFour.brakeTorque += currentBrakeForce + Time.deltaTime * brakeSpeed;
        }
        
        if(!Input.GetKey(KeyCode.Space))
        {
            wheelOne.brakeTorque = 0;
            wheelTwo.brakeTorque = 0;
            wheelThree.brakeTorque = 0;
            wheelFour.brakeTorque = 0;
        }
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(wheelOne, wheelOneT);
        UpdateWheelPose(wheelTwo, wheelTwoT);
        UpdateWheelPose(wheelThree, wheelThreeT);
        UpdateWheelPose(wheelFour, wheelFourT);
    }
    
    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        
        collider.GetWorldPose(out pos, out rotation);

        transform.position = pos;
        transform.rotation = rotation;
    }

    private void OnDrawGizmos()
    {
        Handles.Label(new Vector3(transform.position.x, transform.position.y, transform.position.z), 
            "Motor: " + wheelOne.motorTorque.ToString());
        
        Handles.Label(new Vector3(transform.position.x, transform.position.y - 2, transform.position.z), 
            "Brakes: " + wheelThree.brakeTorque.ToString());
    }
}
