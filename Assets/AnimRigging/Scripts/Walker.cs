using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walker : MonoBehaviour
{
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        updateDestination();

    }
    void Update()
    {
        if (HasReachedDestination())
        {
            updateDestination();
        }
    }

    bool HasReachedDestination()
    {
        return transform.position == agent.destination;
    }

    void updateDestination()
    {
        agent.SetDestination(new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-30f, 30f)));
    }

}
