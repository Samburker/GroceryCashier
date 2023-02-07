using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Singleton { get; private set; }

    [Header("Settings")]
    public bool onPause = false;
    public bool onMenu = false;

    [Header("Input values")]
    public Vector2 look;
    public Vector2 move;
    public bool jump;
    public bool sprint;
    public Action<bool> pauseMenu;

    internal bool analogMovement;
    private PlayerInput _playerInput;

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
            onPause = true;
            OnPause();
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
        look = AcceptInputs() ? value.Get<Vector2>() : Vector2.zero;
    }
    private void OnMove(InputValue value)
    {
        move = AcceptInputs() ? value.Get<Vector2>() : Vector2.zero;
    }

    private void OnFire(InputValue value)
    {
        UpdateMouseLock();
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

    public void OnPause()
    {
        pauseMenu?.Invoke(onPause);
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
