using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionPoint : MonoBehaviour
{
    [SerializeField] Transform target;

    public Transform GetTarget()
    {
        return target;
    }
}
