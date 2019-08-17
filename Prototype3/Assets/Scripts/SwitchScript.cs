using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    // Public variables
    public GameObject door1;
    public GameObject door2;
    // Test
    public Material mat1;
    public Material mat2;

    // Private variables
    const float triggerRadius = 3.0f;
    private bool door1Unlocked = true;
    private GameObject player;

    private void Awake()
    {
        // Set player
        player = GameObject.Find("Player");

        CheckDoors();
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

    private void CheckDoors()
    {
        // Set door status
        if (door1Unlocked)
        {
            door1.GetComponent<Collider>().isTrigger = true;
            door2.GetComponent<Collider>().isTrigger = false;
        }
        else
        {
            door2.GetComponent<Collider>().isTrigger = true;
            door1.GetComponent<Collider>().isTrigger = false;
        }
    }

    // When clicked
    private void OnMouseDown()
    {
        // If player is within trigger radius
        if ((player.transform.position - this.transform.position).magnitude < triggerRadius)
        {
            door1Unlocked = !door1Unlocked;

            CheckDoors();
        }
    }
}
