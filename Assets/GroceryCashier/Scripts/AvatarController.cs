using Sunbox.Avatars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AvatarController : MonoBehaviour
{
    public AvatarCustomization avatar;
    public bool randomizeAvatar;

    private CharacterController _cc;
    private Animator _animator;
    private int _animID_MoveX;
    private int _animID_MoveY;
    private Vector3 _previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (randomizeAvatar)
        {
            avatar.RandomizeBodyParameters();
            avatar.RandomizeClothing();
        }

        _cc = GetComponent<CharacterController>();
        _animator = avatar.Animator;

        _animID_MoveX = Animator.StringToHash("MoveX");
        _animID_MoveY = Animator.StringToHash("MoveY");
        _previousPosition = _cc.transform.position;
    }

    private void Update()
    {
        Vector3 pos = _cc.transform.position;
        Vector3 velocity = (pos - _previousPosition) * (1f/Time.deltaTime);
        _animator.SetFloat(_animID_MoveX, velocity.y);
        _animator.SetFloat(_animID_MoveY, velocity.x);
        _previousPosition = pos;
    }
}
