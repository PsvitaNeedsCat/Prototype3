using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftSwitchScript : MonoBehaviour
{
    // Public variables
    /// <summary>
    /// Material when the player is out of range of the switch.
    /// </summary>
    [SerializeField] Material mat1; // Unlocked

    /// <summary>
    /// Material when the player is in range of the switch.
    /// </summary>
    [SerializeField] Material mat2; // Locked

    /// <summary>
    /// Object reference to the lift.
    /// </summary>
    [SerializeField] GameObject lift;

    /// <summary>
    /// Reference to the particle system.
    /// </summary>
    [SerializeField] ParticleSystem touch;

    // Private variables
    /// <summary>
    /// Constant - radius required to be within to trigger the switch.
    /// </summary>
    const float triggerRadius = 3.0f;

    /// <summary>
    /// Reference to the player object.
    /// </summary>
    GameObject player;

    /// <summary>
    /// Reference to the sound effect triggered when touch is activated.
    /// </summary>
    AudioSource touchFx;

    // Called on awake.
    void Awake()
    {
        // Set player
        player = GameObject.FindGameObjectWithTag("Player");

        touchFx = this.GetComponent<AudioSource>();
    }

    // Called every frame.
    void FixedUpdate()
    {
        // Check if follower is close enough and has collected all light
        if (((player.transform.position - this.transform.position).magnitude < triggerRadius))
        {
            this.GetComponent<MeshRenderer>().material = mat2;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = mat1;
        }
    }

    // Called when mouse clicks on this.
    void OnMouseDown()
    {
        // Check if follower is close enough and has collected all lightfloat distance = (player.transform.position - this.transform.position).magnitude;
        // If player is within trigger radius
        float distance = (player.transform.position - this.transform.position).magnitude;
        if (distance < triggerRadius)
        {
            // Uses a scale of 3
            float timeToWait = distance / 7.5F;
            player.GetComponent<PlayerScript>().DynamicLightEffect(timeToWait, this.gameObject);
            
        }
    }
    
    /// <summary>
    /// Triggers lift interaction.
    /// </summary>
    public void Interaction()
    {
        touch.Play();
        touchFx.Play();

        // Start lift
        lift.GetComponent<LiftScript>().active = !lift.GetComponent<LiftScript>().active;
    }
}
