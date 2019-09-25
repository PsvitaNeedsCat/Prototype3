﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftSwitchScript : MonoBehaviour
{
    // Public variables
    public Material mat1; // Unlocked
    public Material mat2; // Locked
    public GameObject lift;

    // Particles
    public ParticleSystem touch;

    // Private variables
    private const float triggerRadius = 3.0f;
    private GameObject player;
    private AudioSource touchFx;

    private void Awake()
    {
        // Set player
        player = GameObject.Find("Player");

        touchFx = this.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
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

    private void OnMouseDown()
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
    
    public void Interaction()
    {
        touch.Play();
        touchFx.Play();

        // Start lift
        lift.GetComponent<LiftScript>().active = !lift.GetComponent<LiftScript>().active;
    }
}