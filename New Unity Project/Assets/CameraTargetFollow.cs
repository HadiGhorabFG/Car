using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lookSpeed = 10;
    [SerializeField] private float followSpeed = 10;

    [SerializeField] private Vector3 offset;

    private void FixedUpdate()
    {
        LookAtTarget();
        FollowTarget();
    }

    private void LookAtTarget()
    {
        Vector3 lookDirection = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * lookSpeed);
    }

    private void FollowTarget()
    {
        Vector3 pos = target.position + target.right * offset.x + target.forward * offset.z + target.up * offset.y;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * followSpeed);
    }
}
