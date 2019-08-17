using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour
{
    // Private variables
    GameObject player;
    const float radiusToPlayer = 2.0f;
    const float speed = 5.0f;
    bool stationary = true;

    // Awake function
    private void Awake()
    {
        // Set player
        player = GameObject.Find("Player");
    }

    // Fixed update function
    private void FixedUpdate()
    {
        // If clicked on


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
        stationary = !stationary;
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
        dirVec.y = this.transform.position.y;

        // Move forward
        this.GetComponent<Rigidbody>().AddForce(dirVec.normalized * speed);
    }
}
