using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendMaterial : MonoBehaviour
{
    // Blends between two materials

    public Material material1;
    public Material material2;

    public Color startColour;
    public Color endColour;
    public float exposure = 0.4f;


    float tParam = 0f;
    float valToBeLerped = 0f;
    public float BlendSpeed = 0.1f;
    Renderer rend;

    public bool trigger = false;
    public float step = 0f;
    
    void Start()
    {
        material1.SetFloat("_Exposure", exposure);
        //RenderSettings.skybox = StartSkybox;
        //rend = GetComponent<Renderer>();

        // At start, use the first material
        //rend.material = material1;
    }

    void Update()
    {
        if (trigger == true)
        {

            // Lerp thingy
            //var tParam : float = 0f;
            //var valToBeLerped : float = 0f;
            //var speed : float = 0.3f;
            /*
            if (tParam < 1)
            {
                tParam += Time.deltaTime * BlendSpeed; //This will increment tParam based on Time.deltaTime multiplied by a speed multiplier
                valToBeLerped = Mathf.Lerp(0, 1, tParam);
            }

            // float lerp = Mathf.PingPong(Time.time, duration) / duration;
            rend.material.Lerp(material1, material2, valToBeLerped);
            */

            //RenderSettings.skybox.Lerp(material1, material2, step);
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(startColour, endColour, step));
        }
    }
}
