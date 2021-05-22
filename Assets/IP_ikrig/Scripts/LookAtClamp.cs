using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtClamp : MonoBehaviour
{
    [SerializeField]
    private Rig lookAtRig;

    [SerializeField]
    private Transform lookAtTarget;
    public float switchDelta = 0.05f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toTarget = (lookAtTarget.position - transform.position).normalized;

        float dot = Vector3.Dot(toTarget, transform.forward);
        if (dot < 0)
        {
            lookAtRig.weight = Mathf.MoveTowards(lookAtRig.weight, 0.0f, switchDelta);
        }
        else
        {
            lookAtRig.weight = Mathf.MoveTowards(lookAtRig.weight, 1.0f, switchDelta);
        }

    }
}
