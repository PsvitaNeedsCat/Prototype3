using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    // README - setup
    /*
     * Drag this script onto button for lights switch.
     * Increase size of lights and drag a reference to each light into it.
     * Each light object needs a Light component.
     * If UseMaterial is ticked, the script needs 2 materials.
     * A 'fog' object must be referenced. This object needs a particle system and a collider.
     * The fog object should be given the layer titled 'fog'.
     */

    // Public/Serialized
    /// <summary>
    /// Fog object will be set in the level and have a collider that the follower can't get through AND a fog particle system attached.
    /// </summary>
    [SerializeField] GameObject fog;
    /// <summary>
    /// An array of light objects that will turn on/off.
    /// </summary>
    [SerializeField] GameObject[] lights;
    /// <summary>
    /// True if changing material of the lights
    /// </summary>
    [SerializeField] bool UseMaterial = false;
    [SerializeField] Material offMaterial;
    [SerializeField] Material onMaterial;

    // Private
    bool isLightActive = false;
    GameObject player;
    const float triggerRadius = 3.0f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CheckLightStatus();
    }

    // When clicked
    private void OnMouseDown()
    {
        float distance = (player.transform.position - this.transform.position).magnitude;
        // If player is within trigger radius
        if (distance < triggerRadius)
        {
            // Uses a scale of 3
            float timeToWait = distance / 7.5F;
            player.GetComponent<PlayerScript>().DynamicLightEffect(timeToWait, this.gameObject);
        }
    }

    /// <summary>
    /// Toggles the lights on/off.
    /// </summary>
    public void Interaction()
    {
        isLightActive = !isLightActive;

        CheckLightStatus();
    }

    /// <summary>
    /// Checks if the lights are on or off and changes things accordingly.
    /// </summary>
    void CheckLightStatus()
    {
        if (isLightActive)
        {
            // Turn all the lights on
            for (uint i = 0; i < lights.Length; i++)
            {
                lights[i].GetComponent<Light>().enabled = true;
                if (UseMaterial)
                {
                    lights[i].GetComponent<MeshRenderer>().material = onMaterial;
                }
            }

            // Turn off the collider
            fog.GetComponent<Collider>().enabled = false;

            // Turn off the fog
            fog.GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            // Turn all the lights off
            for (uint i = 0; i < lights.Length; i++)
            {
                lights[i].GetComponent<Light>().enabled = false;
                if (UseMaterial)
                {
                    lights[i].GetComponent<MeshRenderer>().material = offMaterial;
                }
            }

            // Turn off the collider
            fog.GetComponent<Collider>().enabled = true;

            fog.GetComponent<ParticleSystem>().Play();
        }
    }
}
