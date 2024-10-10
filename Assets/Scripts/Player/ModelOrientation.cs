using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelOrientation : MonoBehaviour
{
    public Transform orientation;

    void Update()
    {
        Vector3 sourceRotation = orientation.eulerAngles;
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.y = sourceRotation.y;
        transform.eulerAngles = currentRotation;
    }
}
