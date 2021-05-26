using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    public enum MoveMode { walk, run };
    CharacterController controller;
    public Camera thirdPersonCamera;
    private Picker itemPicker;

    // Animator
    Animator animator;
    int speedHash;
    int isJumpingHash;
    PlayerInput input;
    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;
    bool walkPressed;
    public float walkSpeed = 0.5f;
    public float runSpeed = 1.0f;
    private float currentSpeed = 0.0f;

    // Locomotion params
    public MoveMode defaultMoveMode = MoveMode.walk;
    public bool rotateDuringJump = false;
    public bool gravityEnabled = true;
    public float turnLerpTime = 0.05f;
    public float accelerationTime = 0.02f;

    void Awake()
    {
        input = new PlayerInput();
        input.CharacterControls.Movement.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Walk.performed += ctx => walkPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Jump.performed += ctx => HandleJump();
        input.CharacterControls.Interact.performed += ctx => Interact();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        speedHash = Animator.StringToHash("speed");
        isJumpingHash = Animator.StringToHash("isJumping");
        itemPicker = GetComponent<Picker>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleGravity();
        HandleRotation();
        HandleMovement();
    }

    void HandleRotation()
    {
        // Commited jump
        if (animator.GetBool(isJumpingHash) && !rotateDuringJump)
        {
            return;
        }

        // Process rotation
        if (currentMovement == Vector2.zero)
        {
            return;
        }
        Vector3 newForward = thirdPersonCamera.transform.forward;
        Vector3 newRight = thirdPersonCamera.transform.right;

        newForward.y = 0f;
        newRight.y = 0f;
        newForward.Normalize();
        newRight.Normalize();
        Vector3 moveDirection = newForward * currentMovement.y + newRight * currentMovement.x;
        if (moveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, turnLerpTime);
        }
    }

    void HandleMovement()
    {
        float maxSpeed = 0.0f;
        if (defaultMoveMode == MoveMode.walk)
        {
            maxSpeed = walkSpeed;
            if (runPressed)
            {
                maxSpeed = runSpeed;
            }
        }
        else
        {
            maxSpeed = runSpeed;
            if (walkPressed)
            {
                maxSpeed = walkSpeed;
            }
        }
        float oldSpeed = animator.GetFloat(speedHash);
        float newSpeed = Mathf.Clamp(Mathf.Abs(currentMovement.x) + Mathf.Abs(currentMovement.y), 0.0f, maxSpeed);
        currentSpeed = Mathf.Lerp(oldSpeed, newSpeed, accelerationTime);
        animator.SetFloat(speedHash, currentSpeed);
    }

    void HandleGravity()
    {
        if (!gravityEnabled)
        {
            return;
        }
        Vector3 moveVector = Vector3.zero;
        if (!controller.isGrounded)
        {
            moveVector += Physics.gravity;
        }
        controller.SimpleMove(moveVector);
    }

    void HandleJump()
    {
        animator.SetBool(isJumpingHash, true);
        gravityEnabled = false;
    }

    private void Land()
    {
        gravityEnabled = true;
        animator.SetBool(isJumpingHash, false);
    }

    private void Interact()
    {
        itemPicker.PickupItem(animator);
    }

    // Controls
    void OnEnable()
    {
        input.CharacterControls.Enable();
    }

    void OnDisable()
    {
        input.CharacterControls.Disable();
    }

    public float CurrentSpeed()
    {
        return currentSpeed;
    }
}
