using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attention : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Transform lookTargetLocator;
    public float attentionRadius = 3.0f;
    private Collider[] overlappedColliders;
    private Vector3 restOffset;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attentionRadius);
    }

    void Start()
    {
        restOffset = lookTargetLocator.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Interactable");
        overlappedColliders = Physics.OverlapSphere(transform.position, attentionRadius, layerMask);
        Transform attentionTarget = getClosestTarget();

        Vector3 newTargetPosition;
        if (attentionTarget is null)
        {
            newTargetPosition = transform.position + transform.TransformDirection(restOffset);
        }
        else
        {
            newTargetPosition = attentionTarget.position;
        }
        lookTargetLocator.position = Vector3.MoveTowards(lookTargetLocator.position, newTargetPosition, 0.1f);
    }

    Transform getClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Collider hitCollider in overlappedColliders)
        {
            Vector3 directionToTarget = hitCollider.transform.position - currentPosition;
            float distanceToTargetSqr = directionToTarget.sqrMagnitude;
            if (distanceToTargetSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceToTargetSqr;
                closestTarget = hitCollider.transform;
            }
        }
        return closestTarget;
    }
}
