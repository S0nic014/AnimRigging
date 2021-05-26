using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour
{
    [SerializeField] LayerMask itemMask;
    [SerializeField] Transform IKTarget;
    public float pickupRadius = 1.0f;
    private Transform targetItem;


    private void OnDrawGizmos()
    {
        Color debugColor = Color.grey;
        debugColor.a = 0.4f;
        Gizmos.color = debugColor;
        Gizmos.DrawSphere(transform.position, pickupRadius);
    }

    void Update()
    {
        if (targetItem != null)
        {
            IKTarget.position = targetItem.position;
        }
    }


    public void PickupItem(Animator animator)
    {
        Collider[] overllapped = Physics.OverlapSphere(transform.position, pickupRadius, itemMask);
        if (overllapped.Length > 0)
        {
            Collider item = (Collider)overllapped.GetValue(0);
            targetItem = item.transform;
            animator.SetTrigger("PickupTrigger");
        };

    }
}
