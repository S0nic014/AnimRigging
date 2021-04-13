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
    private float lerpSpeed = 2.0f;



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

            // Interpolate rotation, rig weight
            rightHandAvoider.rotation = perfedicularHandRotation;
            handsAvoidanceRig.weight = Mathf.Lerp(handsAvoidanceRig.weight, 1.0f, Time.deltaTime * lerpSpeed);
        }
        else
        {
            // TODO: Fix lerping
            if (handsAvoidanceRig.weight < 0.001f)
            {
                handsAvoidanceRig.weight = 0.0f;
            }
            else
            {
                handsAvoidanceRig.weight = Mathf.SmoothStep(handsAvoidanceRig.weight, 0.0f, 0.2f);
            }
        }
    }
}
