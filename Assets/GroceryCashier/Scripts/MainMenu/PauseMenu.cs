using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    internal void Hide()
    {
        gameObject.SetActive(false);
    }
    internal void Show()
    {
        gameObject.SetActive(true);
    }
}
