using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SecurityCamera : MonoBehaviour
{
    internal Camera _camera;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _camera.enabled = enabled;
    }

    internal void SetRenderTexture(RenderTexture texture)
    {
        _camera = GetComponent<Camera>();
        _camera.targetTexture = texture;
    }

    private void OnEnable()
    {
        _camera.enabled = true;
    }

    private void OnDisable()
    {
        _camera.enabled = false;
    }
}
