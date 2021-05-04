using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Awareness : MonoBehaviour
{

    [SerializeField]
    Transform sideRaycastSource;
    [SerializeField]
    Transform rightHandAvoider;

    [SerializeField]
    Rig handsAvoidanceRig;

    private float radius = 0.7f;
    private float handOffset = 0.07f;
    private Vector3 rightHandRestPosition;
    private Quaternion rightHandRestRotation;
    public float raiseSpeed = 0.02f;
    public float lowerSpeed = 0.05f;



    // Start is called before the first frame update
    void Start()
    {
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
        RaycastHit rightHit;
        if (Physics.Raycast(sideRaycastSource.position, sideRaycastSource.right, out rightHit, radius))
        {
            rightHandAvoider.position = rightHit.point + handOffset * rightHit.normal;
            Quaternion perfedicularHandRotation = Quaternion.FromToRotation(rightHandAvoider.up, rightHit.normal) * rightHandAvoider.rotation;
            Vector3 perpedicularEuler = perfedicularHandRotation.eulerAngles;

            // Reset unuszed rotation, clamp Z
            if (rightHit.normal.x < 0f)
            {
                perpedicularEuler.x = rightHandRestRotation.eulerAngles.x;
                perpedicularEuler.y = rightHandRestRotation.eulerAngles.y;
            }
            else
            {
                perpedicularEuler.x = 0f;
                perpedicularEuler.y = 0f;
            }
            perpedicularEuler.z = Mathf.Clamp(perfedicularHandRotation.eulerAngles.z, rightHandRestRotation.eulerAngles.z, -70.0f);
            perfedicularHandRotation.eulerAngles = perpedicularEuler;
            rightHandAvoider.rotation = perfedicularHandRotation;

            // Interpolate rotation, rig weight
            handsAvoidanceRig.weight = Mathf.MoveTowards(handsAvoidanceRig.weight, 1.0f, raiseSpeed);
        }
        else
        {
            handsAvoidanceRig.weight = Mathf.MoveTowards(handsAvoidanceRig.weight, 0.0f, lowerSpeed);
        }
    }
}
