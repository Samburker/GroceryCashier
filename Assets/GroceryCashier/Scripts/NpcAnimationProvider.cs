using Sunbox.Avatars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class NpcAnimationProvider : MonoBehaviour
{
    public AvatarCustomization avatar;

    private NavMeshAgent _agent;
    private Animator _anim;
    private Vector2 smoothDeltaPosition = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    private int _MoveXID;
    private int _MoveYID;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _anim = avatar.Animator;

        _MoveXID = Animator.StringToHash("MoveX");
        _MoveYID = Animator.StringToHash("MoveY");
    }

    private void Update()
    {
        Vector3 worldDeltaPosition = _agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && _agent.remainingDistance > _agent.radius;

        // Update animation parameters
        _anim.SetFloat(_MoveXID, velocity.x);
        _anim.SetFloat(_MoveYID, velocity.y);

        //GetComponent<LookAt>().lookAtTargetPosition = _agent.steeringTarget + transform.forward;

        // Because we dont have root motion
        if (!_anim.hasRootMotion)
            transform.position = _agent.nextPosition;
    }

    private void OnAnimatorMove()
    {
        // This works only with root motion
        transform.position = _agent.nextPosition;
    }
}
