using UnityEngine;

public class NormalsReplacementShader : MonoBehaviour
{
    /// <summary>
    /// Shader normals
    /// </summary>
    [SerializeField] Shader normalsShader;

    private RenderTexture renderTexture;
    private new Camera camera;

    private void Start()
    {
        // Create a render texture matching the main camera's current dimensions.
        renderTexture = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 24);
        // Surface the render texture as a global variable, available to all shaders.
        Shader.SetGlobalTexture("_CameraNormalsTexture", renderTexture);

        // Setup a copy of the camera to render the scene using the normals shader.
        GameObject copy = new GameObject("Normals camera");
        camera = copy.AddComponent<Camera>();
        camera.CopyFrom(Camera.main);
        camera.transform.SetParent(transform);
        camera.targetTexture = renderTexture;
        camera.SetReplacementShader(normalsShader, "RenderType");
        camera.depth = Camera.main.depth - 1;
    }
}

// Credit: Free online tutorial by Erik Roystan Ross.
// https://roystan.net/articles/toon-water.html
