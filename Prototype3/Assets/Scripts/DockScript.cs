using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockScript : MonoBehaviour
{
    // Public/Serialized
    /// <summary>
    /// Where the player respawns when getting off the boat.
    /// </summary>
    [SerializeField] Transform playerSpawn;
    [SerializeField] Transform followerSpawn;
    [SerializeField] Transform leftBoatSpawn;
    [SerializeField] Transform rightBoatSpawn;
    [SerializeField] bool BoatSpawnsOnLeft = true;
    [SerializeField] bool IsFirstDock = false;

    // Private
    GameObject player;
    GameObject follower;
    GameObject boat;
    /// <summary>
    /// How close the boat must be to 
    /// </summary>
    float distanceCheck = 2.0f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        follower = GameObject.FindGameObjectWithTag("Follower");

        if (IsFirstDock)
        {
            // Spawn boat
            boat = GameObject.FindGameObjectWithTag("Boat");
            boat.transform.rotation = transform.rotation;
            boat.transform.position = (BoatSpawnsOnLeft) ? leftBoatSpawn.position : rightBoatSpawn.position;
        }
    }

    void OnMouseDown()
    {
        if (player.GetComponent<PlayerScript>().isBoating)
        {
            // If close enough
            if ((player.transform.position - this.transform.position).magnitude < distanceCheck)
            {
                // Lose control of boat
                boat = player.transform.parent.gameObject;
                boat.GetComponent<BoatScript>().isStatic = true;

                // Snap player
                player.transform.parent = null;
                player.transform.position = playerSpawn.position;
                player.GetComponent<Collider>().enabled = true;
                player.GetComponent<Rigidbody>().isKinematic = false;

                if (follower.GetComponent<FollowerScript>().isBoating)
                {
                    // Snap follower
                    follower.transform.parent = null;
                    follower.transform.position = followerSpawn.position;
                    follower.GetComponent<Collider>().enabled = true;
                    follower.GetComponent<Rigidbody>().isKinematic = false;
                }

                // Gain control of player
                Camera.main.GetComponent<CameraScript>().target = player;
                player.GetComponent<PlayerScript>().isBoating = false;
                follower.GetComponent<FollowerScript>().isBoating = false;

                // Respawn boat
                boat.transform.rotation = transform.rotation;
                boat.transform.position = (BoatSpawnsOnLeft) ? leftBoatSpawn.position : rightBoatSpawn.position;
            }
        }
    }
}
