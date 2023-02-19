using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GroceryCustomer : MonoBehaviour
{
    public Transform goal;

    private NavMeshAgent agent;
    private Vector3 _currentTargetPosition;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (goal == null)
            return;

        if (_currentTargetPosition != goal.position)
        {
            agent.destination = goal.position;
            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
                agent.isStopped = false;
            else
                agent.isStopped = true;
            _currentTargetPosition = goal.position;
        }
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || agent == null || agent.path == null)
            return;

        Gizmos.color = agent.isStopped ? Color.red : Color.blue;
        Vector3 previousPoint = agent.path.corners[0];
        for (int i = 1; i < agent.path.corners.Length; i++)
        {           
            Gizmos.DrawLine(previousPoint, agent.path.corners[i]);
            previousPoint = agent.path.corners[i];
        }
    }


}
