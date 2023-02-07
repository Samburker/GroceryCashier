using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebug : MonoBehaviour
{
    private PlayerInput _input;
    private Rect screenRect;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<PlayerInput>();
        screenRect = new Rect(Vector2.zero, new Vector2(800, 600));
    }

    void OnGUI()
    {
        if (!Application.isEditor && !Application.isPlaying)  // or check the app debug flag
            return;
        string debugText = "currentControlScheme: " + _input.currentControlScheme;

        GUI.Label(screenRect, debugText);
    }
}
