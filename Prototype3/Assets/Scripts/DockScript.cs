using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockScript : MonoBehaviour
{
    // Public
    /// <summary>
    /// Where the player respawns when getting off the boat.
    /// </summary>
    [SerializeField] Transform spawn;

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
                player.transform.position = this.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
                player.GetComponent<Collider>().enabled = true;
                player.GetComponent<Rigidbody>().isKinematic = false;

                // Snap follower
                follower.transform.parent = null;
                follower.transform.position = player.transform.position + new Vector3(-1.0f, 0.0f, 0.0f);
                follower.GetComponent<Collider>().enabled = true;
                follower.GetComponent<Rigidbody>().isKinematic = false;

                // Gain control of player
                Camera.main.GetComponent<CameraScript>().target = player;
                player.GetComponent<PlayerScript>().isBoating = false;
                follower.GetComponent<FollowerScript>().isBoating = false;
            }
        }
    }
}
