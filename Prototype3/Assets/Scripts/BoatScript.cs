using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour
{
    // Public
    /// <summary>
    /// Vector facing front of boat
    /// </summary>
    public Vector3 vecForward;

    // Private
    const float speed = 5.0f;
    bool isStatic = true;
    /// <summary>
    /// How close the player must be to the boat to be able to enter it.
    /// </summary>
    const float playerRadius = 1.0f;
    const float lookRadius = 10.0f;
    GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Called every frame
    void FixedUpdate()
    {
        vecForward = this.transform.forward;

        // If static
        if (isStatic)
        {
            // Freeze all
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        // Player is in control
        else
        {
            // Unfreeze parts
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

            // If mouse down
            if (Input.GetMouseButton(0))
            {
                // Seek mouse
                //SeekMouse();
            }
        }
    }

    // When clicked
    private void OnMouseDown()
    {
        // If player is close enough
        if ((player.transform.position - this.transform.position).magnitude < playerRadius)
        {
            // Set current boat for player and follower
            player.GetComponent<PlayerScript>().curBoat = this.gameObject;
            // Set boating for player and follower
            player.GetComponent<PlayerScript>().isBoating = true;
            // Set camera focus to boat
            Camera.main.GetComponent<CameraScript>().target = this.gameObject;
            // Change from static to control
            isStatic = false;
        }
    }

    void SeekMouse()
    {
        // Get direction
        Vector3 direction = Input.mousePosition;
        direction.y = this.transform.position.y;
        direction = direction.normalized;

        // If angle between forward and mouse is less than the constant
        if (Vector3.Angle(transform.forward, direction) < lookRadius)
        {
            // Snap to look at mouse
            this.transform.LookAt(direction);

            // Add force
            this.GetComponent<Rigidbody>().AddForce(direction * speed);
        }
        // Angle between forward and mouse is more than the constant
        else
        {
            // Needs to rotate

            // Get the angle
            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            // If the angle is negative
            if (Mathf.Sign(angle) < 0)
            {
                // Rotate counter-clockwise
                this.transform.Rotate(new Vector3(0.0f, 5.0f, 0.0f));
            }
            // If the angle is positive
            else
            {
                // Rotate clockwise
                this.transform.Rotate(new Vector3(0.0f, 5.0f, 0.0f));
            }
        }
    }
}
