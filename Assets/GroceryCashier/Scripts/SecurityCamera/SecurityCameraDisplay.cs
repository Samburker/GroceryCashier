using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurityCameraDisplay : MonoBehaviour
{
    public List<SecurityCamera> cameras;
    public RawImage targetImage;
    public int selectedCameraIndex;
    public RenderTexture renderTexture;
    public Texture cameraOfflineTexture;

    private void OnEnable()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] == null)
                continue;

            cameras[i].enabled = false;
            cameras[i].SetRenderTexture(renderTexture);
        }
        UpdateCamera(0);
    }

    public void PreviousCamera()
    {
        selectedCameraIndex = Mathf.RoundToInt(Mathf.Repeat(selectedCameraIndex - 1, cameras.Count));
        UpdateCamera(selectedCameraIndex);
    }

    public void NextCamera()
    {
        selectedCameraIndex = Mathf.RoundToInt(Mathf.Repeat(selectedCameraIndex + 1, cameras.Count));
        UpdateCamera(selectedCameraIndex);
    }
    private void UpdateCamera(int selectedCameraIndex)
    {
        bool cameraActive = false;
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] == null)
                continue;

            if (i == selectedCameraIndex && cameras[i].gameObject.activeSelf)
            {
                cameras[i].enabled = true;
                cameraActive = true;
            }
            else
            {
                cameras[i].enabled = false;
            }
        }

        // Settings active render texture to canvas if camera found
        targetImage.texture = cameraActive ? renderTexture : cameraOfflineTexture;
    }
}
