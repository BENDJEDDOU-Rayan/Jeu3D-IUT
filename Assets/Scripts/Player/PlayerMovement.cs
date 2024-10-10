using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    private bool isSprinting = false;
    [Header("Ground checking")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private Vector2 movementInput;
    private PlayerControls controls;
    private Animator anim;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movementInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => Jump();
        controls.Player.Sprint.started += ctx => StartSprinting();
        controls.Player.Sprint.canceled += ctx => StopSprinting();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void StartSprinting()
    {
        isSprinting = true;
    }

    private void StopSprinting()
    {
        isSprinting = false;
    }


    private void Start()
    {
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        rb.freezeRotation = true;
    }

    private void Update()
    {

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        SpeedControl();
        if (grounded)
        {
            rb.drag = groundDrag;
            anim.SetBool("isJumping", false);
        }
        else
        {
            rb.drag = 0;
            anim.SetBool("isJumping", true);
        }
            
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float playerSpeed = horizontalVelocity.magnitude;

        anim.SetFloat("moveSpeed", playerSpeed);
        anim.SetBool("isSprinting", isSprinting);
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        Vector3 moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;
        moveDirection.y = 0f;
        if (moveDirection.magnitude > 0)
        {
            Vector3 normalizedDirection = moveDirection.normalized;
            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
            Vector3 forceToAdd = normalizedDirection * currentSpeed * 10f;
            if (grounded)
                rb.AddForce(forceToAdd, ForceMode.Force);
            else
                rb.AddForce(forceToAdd * airMultiplier, ForceMode.Force);
        }
    }


    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float currentMaxSpeed = isSprinting ? sprintSpeed : walkSpeed;
        if (flatVel.magnitude > currentMaxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentMaxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
