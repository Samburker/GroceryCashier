using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
[RequireComponent(typeof(NavMeshAgent), typeof(CharacterController))]
public class GroceryCustomer : MonoBehaviour
{
    public Transform goal;
    public bool isDone = false;
    public int itemsWanted = 0;
    public int itemCount = 0;
    public bool paymentDone = false;

    internal ShoppingList shoppingList;
    private NavMeshAgent _agent;
    private CharacterController _cc;
    private SceneDescriptor _sceneDescriptor;
    private Vector3 _currentTargetPosition;

    public CustomerState state;
    private CustomerState _previousState = CustomerState.NONE;
    private bool _pathComplete;

    public enum CustomerState
    {
        NONE, COLLECTING, QUEUE, PAYMENT, EXIT,
        DESPAWN
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _cc = GetComponent<CharacterController>();
        _sceneDescriptor = GameManager.Singleton.sceneDescriptor;
        _agent.avoidancePriority = UnityEngine.Random.Range(30, 70);
        isDone = false;
    }

    private void OnEnable()
    {
        CustomerManager.Singleton.OnCustomerSpawn(this);
        StartCoroutine(CustomerAi());
    }

    private void OnDisable()
    {
        CustomerManager.Singleton.OnCustomerDespawn(this);
    }

    private IEnumerator CustomerAi()
    {
        yield return new WaitUntil(() => shoppingList != null); // Wait until customer gets shopping list

        itemsWanted = UnityEngine.Random.Range(shoppingList.wantedItemsMin, shoppingList.wantedItemsMax);

        while (true)
        {
            yield return new WaitForEndOfFrame();
            _pathComplete = PathComplete();
            bool stateChanged = _previousState != state;
            _previousState = state;
            switch (state)
            {
                case CustomerState.NONE:
                case CustomerState.COLLECTING:
                    state = AiCollecting(stateChanged);
                    break;
                case CustomerState.QUEUE:
                    state = AiQueue(stateChanged);
                    break;
                case CustomerState.PAYMENT:
                    state = AiPayment(stateChanged);
                    break;
                case CustomerState.EXIT:
                    state = AiExit(stateChanged);
                    break;
                case CustomerState.DESPAWN:
                    state = AiDespawn(stateChanged);
                    break;
            }

            Walk();
        }

    }



    private CustomerState AiCollecting(bool first)
    {
        if (itemCount >= itemsWanted) // Going to next state
            return CustomerState.QUEUE;

        if (_pathComplete)
        {
            goal = _sceneDescriptor.randomItemPositions[UnityEngine.Random.Range(0, _sceneDescriptor.randomItemPositions.Length)];

            if (first)
                itemCount = 0;
            else
                itemCount++;
        }

        return CustomerState.COLLECTING;
    }

    private CustomerState AiQueue(bool first)
    {
        if (first)
            _sceneDescriptor.cashRegisters[0].Enqueue(this);

        int queueIndex = _sceneDescriptor.cashRegisters[0].GetQueue(this);
        if (_pathComplete && queueIndex < 0)
        {
            // Going to next state
            return CustomerState.PAYMENT;
        }

        goal = _sceneDescriptor.cashRegisters[0].GetQueueSpot(queueIndex + 1); // Spot 0 is for current customer
        return CustomerState.QUEUE;
    }

    private CustomerState AiPayment(bool first)
    {
        if (first)
        {
            goal = _sceneDescriptor.cashRegisters[0].GetQueueSpot(0);
            _sceneDescriptor.cashRegisters[0].PaymentStart(this);
        }

        if (_pathComplete && !first && paymentDone) // Going to next state
            return CustomerState.EXIT;

        return CustomerState.PAYMENT;
    }

    private CustomerState AiExit(bool first)
    {
        // Walking to spawn/despawn area
        if(first)
            goal = _sceneDescriptor.customerSpawnpoints[0];

        if (_pathComplete && !first) // Going to next state
            return CustomerState.DESPAWN;

        return CustomerState.EXIT;
    }

    private CustomerState AiDespawn(bool first)
    {
        // This is the final one, despawning
        if (first)
        {
            _agent.isStopped = true;
            StopAllCoroutines();
            Destroy(gameObject);
        }
        return CustomerState.DESPAWN;
    }

    private void Walk()
    {
        if (goal == null || _agent == null)
            return;

        if (_currentTargetPosition != goal.position)
        {
            _agent.destination = goal.position;
            if (_agent.pathStatus == NavMeshPathStatus.PathComplete)
                _agent.isStopped = false;
            else
                _agent.isStopped = true;
            _currentTargetPosition = goal.position;
        }
    }

    internal void SetPosition(Transform target)
    {
        if (_cc == null)
            _cc = GetComponent<CharacterController>();
        _cc.enabled = false;
        transform.position = target.position;
        transform.rotation = target.rotation;
        _cc.enabled = true;
    }

    internal void SetGoal(Transform target)
    {
        goal = target;
    }

    private bool PathComplete()
    {
        if (Vector3.Distance(_agent.destination, _agent.transform.position) <= _agent.stoppingDistance)
        {
            if (_agent.pathPending)
                return false;
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || _agent == null || _agent.path == null)
            return;

        Gizmos.color = _agent.isStopped ? Color.red : Color.blue;
        Vector3 previousPoint = _agent.path.corners[0];
        for (int i = 1; i < _agent.path.corners.Length; i++)
        {
            Gizmos.DrawLine(previousPoint, _agent.path.corners[i]);
            previousPoint = _agent.path.corners[i];
        }
    }


}
