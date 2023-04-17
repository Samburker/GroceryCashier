using System;
using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public float fadeTime = 5f;
    private Coroutine fadeCoroutine;

    internal bool IsCompleted { get; private set; }

    public void Fade(float target, Action callback = null)
    {
        IsCompleted = false;
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCoroutine(target, callback));
    }

    private IEnumerator FadeCoroutine(float target, Action callback = null)
    {
        fadeCanvas.gameObject.SetActive(true);
        float current = fadeCanvas.alpha;
        bool increase = target > current;

        // If fadetime is zero or negative, skipping fade
        if (fadeTime >= 0f)
        {
            while (!(increase && current > target) && !(!increase && current < target))
            {
                if (increase) // If fading to black (opacity increases)
                    current += (1 / fadeTime) * Time.deltaTime;
                else // If fading from black (opacity degreases)
                    current -= (1 / fadeTime) * Time.deltaTime;

                // Setting calculated opacity
                fadeCanvas.alpha = current;

                // Waiting for next frame
                yield return new WaitForEndOfFrame();
            }
        }

        // Target value acquired, setting final target value
        fadeCanvas.alpha = target;

        // Disabling canvas if target is fully transparent
        fadeCanvas.gameObject.SetActive(target < 0.001f);

        IsCompleted = true;

        // Calling callback
        callback?.Invoke();
    }

}
