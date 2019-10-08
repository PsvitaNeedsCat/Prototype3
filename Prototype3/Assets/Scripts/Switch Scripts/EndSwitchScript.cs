using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class EndSwitchScript : MonoBehaviour
{
    // Public variables
    public Material mat1; // Unlocked
    public Material mat2; // Locked

    // Particles
    public ParticleSystem touch;

    // Private variables
    const float triggerRadius = 4.0f;
    GameObject follower;
    int totalLight;

    void Awake()
    {
        // Set player
        follower = GameObject.FindGameObjectWithTag("Follower");

        // Find amount of light in level
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        totalLight = lights.Length;
    }

    void FixedUpdate()
    {
        // Check if follower is close enough and has collected all light
        if (((follower.transform.position - this.transform.position).magnitude < triggerRadius) && (follower.GetComponent<FollowerScript>().GetLightScore() >= totalLight))
        {
            this.GetComponent<MeshRenderer>().material = mat2;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = mat1;
        }
    }

    void OnMouseDown()
    {
        // Check if follower is close enough and has collected all light
        if (((follower.transform.position - this.transform.position).magnitude < triggerRadius) && (follower.GetComponent<FollowerScript>().GetLightScore() >= totalLight))
        {
            //touch.Play();

            SceneManager.LoadScene("End Level");
        }
    }
}
