using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour
{
    // Public Variables
    public static int light = 0;
    public ParticleSystem touch;

    // Private variables
    GameObject player;
    const float radiusToPlayer = 1.0f;
    const float speed = 5.0f;
    bool stationary = false;

    // Awake function
    private void Awake()
    {
        // Set player
        player = GameObject.Find("Player");
    }

    // Fixed update function
    private void FixedUpdate()
    {
        // Look at player
        LookAtPlayer();

        // If moving
        if (!stationary)
        {
            // Follow player
            if (!NearPlayer())
            {
                MoveTowardPlayer();
            }
        }
    }

    // When clicked
    private void OnMouseDown()
    {
        // If moving
        if (!stationary)
        {
            GameObject[] colliders = GameObject.FindGameObjectsWithTag("LeaveArea");

            foreach (GameObject obj in colliders)
            {
                if (this.GetComponent<Collider>().bounds.Intersects(obj.GetComponent<Collider>().bounds))
                {
                    // Spawn particles
                    touch.Play();

                    stationary = !stationary;
                    return;
                }
            }
        }
        // If not moving
        else
        {
            // Spawn particles
            touch.Play();

            stationary = !stationary;
        }
    }

    // Look at player
    private void LookAtPlayer()
    {
        Vector3 playerPos = player.transform.position;
        playerPos.y = this.transform.position.y;

        this.transform.LookAt(playerPos);
    }

    // Checks if follower is close to the player
    private bool NearPlayer()
    {
        if ((player.transform.position - this.transform.position).magnitude < radiusToPlayer)
        {
            return true;
        }

        return false;
    }

    // Takes a step towards the player
    private void MoveTowardPlayer()
    {
        // Get directional vector
        Vector3 dirVec = player.transform.position - this.transform.position;
        dirVec.y = 0.0f;

        // Move forward
        this.GetComponent<Rigidbody>().AddForce(dirVec.normalized * speed);
    }

    // Collides with trigger object
    private void OnTriggerEnter(Collider other)
    {
        // Picks up light
        if (other.tag == "Light")
        {
            Destroy(other.gameObject);

            light += 1;
        }
    }
}
