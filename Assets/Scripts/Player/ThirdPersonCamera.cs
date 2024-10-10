using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float rotationSpeed = 5f; 

    private float mouseX;
    private float mouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        mouseY = Mathf.Clamp(mouseY, -35f, 60f);

        Quaternion cameraRotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.position = player.position + cameraRotation * offset;

        transform.LookAt(player.position + Vector3.up * 1.5f);

        player.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
