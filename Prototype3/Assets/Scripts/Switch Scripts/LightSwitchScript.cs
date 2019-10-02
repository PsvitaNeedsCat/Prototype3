using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchScript : MonoBehaviour
{
    // Public variables
    public Material mat1; // Locked
    public Material mat2; // Unlocked
    public GameObject leaveZone;

    // Particles
    public ParticleSystem touch;

    // Private variables
    const float triggerRadius = 3.0f;
    private GameObject player;
    private GameObject follower;

    private void Awake()
    {
        // Set player
        player = GameObject.Find("Player");
        follower = GameObject.Find("Follower");
    }

    private void FixedUpdate()
    {
        // Check if player is close enough
        if ((player.transform.position - this.transform.position).magnitude < triggerRadius)
        {
            this.GetComponent<MeshRenderer>().material = mat2;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = mat1;
        }
    }

    // When clicked
    private void OnMouseDown()
    {
        // If not in the light
        if (!follower.GetComponent<Collider>().bounds.Intersects(leaveZone.GetComponent<Collider>().bounds))
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
    }


    public void Interaction()
    {
        touch.Play();

        GetComponent<AudioSource>().Play();

        // Toggle collider
        Collider leaveZoneCollider = leaveZone.GetComponent<Collider>();
        leaveZoneCollider.enabled = !leaveZoneCollider.enabled;

        // Toggle light
        Light leaveZoneLight = leaveZone.transform.GetChild(0).gameObject.GetComponent<Light>();
        leaveZoneLight.enabled = !leaveZoneLight.enabled;
    }

}
