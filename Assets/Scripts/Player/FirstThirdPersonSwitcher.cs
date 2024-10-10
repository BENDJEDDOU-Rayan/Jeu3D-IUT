using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstThirdPersonSwitcher : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    private PlayerControls controls;

    private bool isFirstPerson = true;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.CameraMode.performed += ctx => ChangeViewMode();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);
    }

    void ChangeViewMode()
    {
        isFirstPerson = !isFirstPerson;

        firstPersonCamera.gameObject.SetActive(isFirstPerson);
        thirdPersonCamera.gameObject.SetActive(!isFirstPerson);
    }
}
