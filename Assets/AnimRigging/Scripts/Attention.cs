using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attention : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private LayerMask attentionLayer = default;
    [SerializeField] private Transform lookTargetLocator;
    [SerializeField] private Transform attentionSource;
    public float attentionRadius = 3.0f;
    private Collider[] overlappedColliders;
    private Vector3 restOffset;

    void OnDrawGizmos()
    {
        if (!attentionSource)
        {
            attentionSource = transform;
        }
        Gizmos.DrawWireSphere(attentionSource.position, attentionRadius);
    }

    void Start()
    {
        restOffset = lookTargetLocator.localPosition;
        if (!attentionSource)
        {
            attentionSource = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        overlappedColliders = Physics.OverlapSphere(attentionSource.position, attentionRadius, attentionLayer);
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
        Vector3 currentPosition = attentionSource.position;
        foreach (Collider hitCollider in overlappedColliders)
        {
            if (hitCollider.transform == this.transform)
            {
                continue;
            }
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
