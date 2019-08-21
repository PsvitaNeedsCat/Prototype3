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
    private const float triggerRadius = 4.0f;
    private GameObject follower;
    private int totalLight;

    private void Awake()
    {
        // Set player
        follower = GameObject.Find("Follower");

        // Find amount of light in level
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        totalLight = lights.Length;
    }

    private void FixedUpdate()
    {
        // Check if follower is close enough and has collected all light
        if (((follower.transform.position - this.transform.position).magnitude < triggerRadius) && (follower.GetComponent<FollowerScript>().lightScore >= totalLight))
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
        // Check if follower is close enough and has collected all light
        if (((follower.transform.position - this.transform.position).magnitude < triggerRadius) && (follower.GetComponent<FollowerScript>().lightScore >= totalLight))
        {
            touch.Play();

            SceneManager.LoadScene("Menu");
        }
    }
}
