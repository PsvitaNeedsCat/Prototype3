using UnityEngine;

public class CameraDepthTextureMode : MonoBehaviour 
{
    [SerializeField] DepthTextureMode depthTextureMode;

    private void OnValidate()
    {
        SetCameraDepthTextureMode();
    }

    private void Awake()
    {
        SetCameraDepthTextureMode();
    }

    private void SetCameraDepthTextureMode()
    {
        Camera.main.depthTextureMode = depthTextureMode;
    }
}

// Credit: Free online tutorial by Erik Roystan Ross.
// https://roystan.net/articles/toon-water.html