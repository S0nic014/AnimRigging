using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    PlayerInput input;
    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;
    CharacterController controller;
    public bool gravityEnabled = true;
    public float turnLerpSpeed = 0.05f;
    public Camera thirdPersonCamera;

    void Awake()
    {
        input = new PlayerInput();
        input.CharacterControls.Movement.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
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
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, turnLerpSpeed);
        }
    }

    void HandleMovement()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        // Start running if movement and run pressed and not already running
        if ((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        // Stop running if not movement or run pressed and currently running
        if ((!movementPressed || !runPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
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

    void OnEnable()
    {
        input.CharacterControls.Enable();
    }

    void OnDisable()
    {
        input.CharacterControls.Disable();
    }
}
