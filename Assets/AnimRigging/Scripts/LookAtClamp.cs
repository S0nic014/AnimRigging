using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtClamp : MonoBehaviour
{
    [SerializeField] Rig lookAtRig;

    [SerializeField] Transform lookAtTarget;
    public float switchDelta = 0.02f;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isJumping"))
        {
            lookAtRig.weight = Mathf.MoveTowards(lookAtRig.weight, 0.0f, switchDelta);
            return;
        }

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
