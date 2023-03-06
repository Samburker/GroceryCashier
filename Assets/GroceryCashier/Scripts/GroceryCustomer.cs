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
    private NavMeshAgent _agent;
    private CharacterController _cc;
    private SceneDescriptor _sceneManager;
    private Vector3 _currentTargetPosition;

    public CustomerState state;
    private CustomerState _previousState = CustomerState.NONE;
    private bool _pathComplete;
    public enum CustomerState { NONE, COLLECTING, QUEUE, PAYMENT, EXIT }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _cc = GetComponent<CharacterController>();
        _agent.avoidancePriority = UnityEngine.Random.Range(30, 70);
        _sceneManager = GameManager.Singleton.sceneDescriptor;
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

            switch (state)
            {
                case CustomerState.NONE:
                case CustomerState.COLLECTING:
                    state = AiCollecting(_previousState != state);
                    break;
                case CustomerState.QUEUE:
                    state = AiQueue(_previousState != state);
                    break;
                case CustomerState.PAYMENT:
                    state = AiPayment(_previousState != state);
                    break;
                case CustomerState.EXIT:
                    state = AiExit(_previousState != state);
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

        if (_pathComplete)
        {
            goal = _sceneManager.randomItemPositions[UnityEngine.Random.Range(0, _sceneManager.randomItemPositions.Length)];

            if (first)
                itemCount = 0;
            else
                itemCount++;
        }

        return CustomerState.COLLECTING;
    }

    private CustomerState AiQueue(bool first)
    {
        if (_pathComplete) // Going to next state
            return CustomerState.PAYMENT;

        goal = _sceneManager.cashRegisters[0].GetQueueSpot(0);
        return CustomerState.QUEUE;
    }

    private CustomerState AiPayment(bool first)
    {
        if (_pathComplete) // Going to next state
            return CustomerState.EXIT;

        goal = _sceneManager.cashRegisters[0].GetQueueSpot(0);
        return CustomerState.PAYMENT;
    }

    private CustomerState AiExit(bool first)
    {
        // This is the final one, despawning
        if (_pathComplete)
        {
            goal = _sceneManager.customerManager.customerSpawnpoints[0];
            StopAllCoroutines();
            Destroy(gameObject);
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
