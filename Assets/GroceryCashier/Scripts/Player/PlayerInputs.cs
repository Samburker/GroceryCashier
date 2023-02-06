using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Singleton { get; private set; }
    public bool menuOpen = false;
    private Vector2 look;
    private Vector2 move;
    public Action<bool> pauseMenu;

    #region Unity callbacks
    private void Awake()
    {
        Singleton = this;
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            menuOpen = false;
            OnPause();
        }
    }
    #endregion

    private void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }
    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public Vector2 GetLookInput()
    {
        if (!AcceptInputs())
            return Vector2.zero;
        return look;
    }
    public Vector2 GetMoveInput()
    {
        if (!AcceptInputs())
            return Vector2.zero;
        return move;
    }

    public void OnPause()
    {
        menuOpen = !menuOpen;
        pauseMenu?.Invoke(menuOpen);
        Cursor.lockState = menuOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public bool AcceptInputs()
    {
        return Cursor.lockState == CursorLockMode.Locked && !menuOpen;
    }

}
