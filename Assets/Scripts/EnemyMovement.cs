using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        MoveToRandomWaypoint();
    }

    void MoveToRandomWaypoint()
    {
        if (waypoints.Length == 0) return;

        int randomIndex = Random.Range(0, waypoints.Length);

        navMeshAgent.SetDestination(waypoints[randomIndex].position);
    }

    void Update()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            MoveToRandomWaypoint();
        }
    }
}
