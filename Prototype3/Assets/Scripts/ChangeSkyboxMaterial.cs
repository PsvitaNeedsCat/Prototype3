using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkyboxMaterial : MonoBehaviour
{
    public bool trigger = false;

    public Color colorStart = Color.blue;
    public Color colorEnd = Color.red;
    public float duration = 1.0F;

    public float step = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (trigger == true)
        {
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(colorStart, colorEnd, step));
            //step += Time.deltaTime / duration;
        }
    }
}
