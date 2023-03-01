using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterController))]
public class GroceryCustomer : MonoBehaviour
{
    public Transform goal;
    public bool isDone = false;
    public int itemsWanted = 0;
    public int itemCount = 0;
    internal ShoppingList shoppingList;

    internal Action<GroceryCustomer> OnDespawn;
    internal CustomerManager customerManager;
    private NavMeshAgent agent;
    private CharacterController cc;
    private SceneDescriptor scene;
    private Vector3 _currentTargetPosition;

    public enum CustomerState { NONE, COLLECTING, QUEUE, PAYMENT, EXIT }
    public CustomerState state;
    private CustomerState _previousState = CustomerState.NONE;
    private bool pathComplete;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = UnityEngine.Random.Range(30, 70);
        cc = GetComponent<CharacterController>();
        scene = customerManager.sceneDescriptor;
        isDone = false;
        StartCoroutine(CustomerAi());
    }

    private IEnumerator CustomerAi()
    {
        itemsWanted = UnityEngine.Random.Range(shoppingList.wantedItemsMin, shoppingList.wantedItemsMax);

        while (true)
        {
            yield return new WaitForEndOfFrame();
            pathComplete = PathComplete();

            switch (state)
            {
                case CustomerState.NONE:
                case CustomerState.COLLECTING:
                    yield return AiCollecting(_previousState != state);
                    break;
                case CustomerState.QUEUE:
                    yield return AiQueue(_previousState != state);
                    break;
                case CustomerState.PAYMENT:
                    yield return AiPayment(_previousState != state);
                    break;
                case CustomerState.EXIT:
                    yield return AiExit(_previousState != state);
                    break;
            }
            _previousState = state;

            Walk();
        }

    }


    private CustomerState AiCollecting(bool first)
    {
        if (itemCount >= itemsWanted) // Going to next state
            return CustomerState.QUEUE;

        if (pathComplete)
            goal = scene.randomItemPositions[UnityEngine.Random.Range(0, scene.randomItemPositions.Length)];

        return CustomerState.COLLECTING;
    }

    private CustomerState AiQueue(bool first)
    {
        if (pathComplete) // Going to next state
            return CustomerState.PAYMENT;

        goal = scene.cashRegisters[0].GetQueueSpot(0);
        return CustomerState.QUEUE;
    }

    private CustomerState AiPayment(bool first)
    {
        if (pathComplete) // Going to next state
            return CustomerState.EXIT;

        goal = scene.cashRegisters[0].GetQueueSpot(0);
        return CustomerState.PAYMENT;
    }

    private CustomerState AiExit(bool first)
    {
        // This is the final one, despawning
        if (pathComplete)
        {
            goal = scene.CustomerManager.customerSpawnpoints[0];
            OnDespawn?.Invoke(this);
            StopAllCoroutines();
            return CustomerState.EXIT;
        }

        return CustomerState.EXIT;
    }

    private void Walk()
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

    internal void SetPosition(Transform target)
    {
        if (cc == null)
            cc = GetComponent<CharacterController>();
        cc.enabled = false;
        transform.position = target.position;
        transform.rotation = target.rotation;
        cc.enabled = true;
    }

    internal void SetGoal(Transform target)
    {
        goal = target;
    }

    private bool PathComplete()
    {
        if (Vector3.Distance(agent.destination, agent.transform.position) <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
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
