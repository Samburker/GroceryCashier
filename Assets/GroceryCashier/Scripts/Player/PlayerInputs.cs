using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-10)]
public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Singleton { get; private set; }

    [Header("Status")]
    public bool onPause = false;
    public bool onMenu = false;

    [Header("Input values")]
    public Vector2 look;
    public Vector2 rotate;
    public Vector2 move;
    public bool jump;
    public bool sprint;
    public Action<bool> OnPauseStatusChanges;

    internal bool analogMovement;
    private PlayerInput _playerInput;

    internal Action<bool> pickupButton;
    internal Action<bool> rotateButton;
    internal bool rotateMode;

    #region Unity callbacks
    private void Awake()
    {
        Singleton = this;
    }
    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // Forcing game to pause when losing focus
            onPause = true;
            OnPauseInternal();
        }
    }
    #endregion

    public bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
			return false;
#endif
        }
    }
    private void OnLook(InputValue value)
    {
        look = AcceptInputs() && !rotateMode ? value.Get<Vector2>() : Vector2.zero;
        rotate = AcceptInputs() && rotateMode ? value.Get<Vector2>() : Vector2.zero;
    }
    private void OnMove(InputValue value)
    {
        move = AcceptInputs() ? value.Get<Vector2>() : Vector2.zero;
    }

    private void OnPickup(InputValue value)
    {
        UpdateMouseLock();
        pickupButton?.Invoke(value.isPressed);

    }
    private void OnRotate(InputValue value)
    {
        UpdateMouseLock();
        rotateButton?.Invoke(value.isPressed);
    }

    //public Vector2 GetLookInput()
    //{
    //    if (!AcceptInputs())
    //        return Vector2.zero;
    //    return look;
    //}
    //public Vector2 GetMoveInput()
    //{
    //    if (!AcceptInputs())
    //        return Vector2.zero;
    //    return move;
    //}

    private void OnPause(InputValue value)
    {
        onPause = !onPause; // Flip pause status when pause button is pressed
        OnPauseInternal();
    }

    private void OnPauseInternal()
    {
        if (onMenu) // On menu game cant be paused
            onPause = false;

        OnPauseStatusChanges?.Invoke(onPause);
        UpdateMouseLock();
    }

    public bool AcceptInputs()
    {
        return Cursor.lockState == CursorLockMode.Locked && !onPause && !onMenu;
    }
    private void UpdateMouseLock()
    {
        Cursor.lockState = onPause || onMenu ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
