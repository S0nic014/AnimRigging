using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandAttraction : MonoBehaviour
{

    [SerializeField] Transform sideRaycastSource;
    [SerializeField] Transform rightHandAvoider;

    [SerializeField] Rig handsAttractionRig;
    [SerializeField] LayerMask attractedLayer = default;
    CharacterMovement movement;
    Animator animator;

    private float radius = 0.7f;
    private float handOffset = 0.07f;
    private Vector3 rightHandRestPosition;
    private Quaternion rightHandRestRotation;
    public float raiseSpeed = 0.05f;
    public float lowerSpeed = 0.05f;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
        rightHandRestPosition = rightHandAvoider.position;
        rightHandRestRotation = rightHandAvoider.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        HandleRightHand();
    }

    void HandleRightHand()
    {
        bool isJumping = animator.GetBool("isJumping");
        bool isRunning = movement.CurrentSpeed() > movement.walkSpeed;
        bool canAvoid = !isJumping && !isRunning;
        RaycastHit rightHit;
        if (Physics.Raycast(sideRaycastSource.position, sideRaycastSource.right, out rightHit, radius, attractedLayer.value) && canAvoid)
        {

            rightHandAvoider.position = rightHit.point + handOffset * rightHit.normal;
            Quaternion perfedicularHandRotation = Quaternion.FromToRotation(rightHandAvoider.up, rightHit.normal.normalized) * rightHandAvoider.rotation;
            Vector3 perpedicularEuler = perfedicularHandRotation.eulerAngles;

            // Clamp Z
            perpedicularEuler.z = Mathf.Clamp(perfedicularHandRotation.eulerAngles.z, rightHandRestRotation.eulerAngles.z, -70.0f);
            perfedicularHandRotation.eulerAngles = perpedicularEuler;
            rightHandAvoider.rotation = perfedicularHandRotation;

            // Interpolate rotation, rig weight
            EnableRig();
        }
        else
        {
            DisableRig();
        }
    }

    void EnableRig()
    {
        handsAttractionRig.weight = Mathf.MoveTowards(handsAttractionRig.weight, 1.0f, raiseSpeed);
    }
    void DisableRig()
    {
        handsAttractionRig.weight = Mathf.MoveTowards(handsAttractionRig.weight, 0.0f, lowerSpeed);
    }
}
