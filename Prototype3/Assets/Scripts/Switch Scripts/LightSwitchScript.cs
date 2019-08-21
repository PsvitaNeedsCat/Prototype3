using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchScript : MonoBehaviour
{
    // Public variables
    public Material mat1; // Unlocked
    public Material mat2; // Locked
    public GameObject leaveZone;

    // Particles
    public ParticleSystem touch;

    // Private variables
    const float triggerRadius = 3.0f;
    private GameObject player;
    private GameObject follower;
    private AudioSource touchFx;

    private void Awake()
    {
        // Set player
        player = GameObject.Find("Player");
        follower = GameObject.Find("Follower");

        touchFx = this.GetComponent<AudioSource>();
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
            // If player is within trigger radius
            if ((player.transform.position - this.transform.position).magnitude < triggerRadius)
            {
                touch.Play();
                touchFx.Play();

                // Toggle collider
                leaveZone.GetComponent<Collider>().enabled =
                    !leaveZone.GetComponent<Collider>().enabled;

                // Toggle light
                leaveZone.transform.GetChild(0).gameObject.GetComponent<Light>().enabled =
                    !leaveZone.transform.GetChild(0).gameObject.GetComponent<Light>().enabled;
            }
        }
    }
}
