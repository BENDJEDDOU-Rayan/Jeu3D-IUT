using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedCheck : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 lastPosition;

    private void Start()
    {
        
        lastPosition = transform.position;
    }

    private void Update()
    {
        CalculateSpeed();
    }

    private void CalculateSpeed()
    {
        Vector3 displacement = transform.position - lastPosition;
        moveSpeed = displacement.magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }
}
