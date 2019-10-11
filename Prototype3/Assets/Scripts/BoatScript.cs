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
    public bool isStatic = true;

    // Private
    const float speed = 5.0f;
    float speedModifier = 1.0f;
    /// <summary>
    /// How close the player must be to the boat to be able to enter it.
    /// </summary>
    const float playerRadius = 2.0f;
    const float lookRadius = 10.0f;
    GameObject player;
    GameObject follower;
    Vector3 prevVelocity = new Vector3(0.0f, 0.0f, 0.0f);

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        follower = GameObject.FindGameObjectWithTag("Follower");
    }

    // Called every frame
    void FixedUpdate()
    {
        vecForward = this.transform.forward;
        prevVelocity = this.GetComponent<Rigidbody>().velocity;

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
                SeekMouse();
            }
            else
            {
                // Reset angular velocity
                this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
    }

    // When clicked
    private void OnMouseDown()
    {
        // If player is close enough
        if ((player.transform.position - this.transform.position).magnitude < playerRadius)
        {
            // Set boating for player and follower
            player.GetComponent<PlayerScript>().isBoating = true;
            // Set camera focus to boat
            Camera.main.GetComponent<CameraScript>().target = this.gameObject;
            // Change from static to control
            isStatic = false;
            // Change player collision box
            player.GetComponent<Collider>().enabled = false;
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.transform.parent = this.transform;
            player.transform.localPosition = Vector3.zero + -(Vector3.forward).normalized;

            if (!follower.GetComponent<FollowerScript>().isSitting)
            {
                // Set follower too
                follower.GetComponent<FollowerScript>().isBoating = true;
                follower.GetComponent<Collider>().enabled = false;
                follower.GetComponent<Rigidbody>().isKinematic = true;
                follower.transform.parent = this.transform;
                follower.transform.localPosition = Vector3.zero + (Vector3.forward).normalized;
                follower.GetComponent<FollowerScript>().currentState = FollowerScript.AnimationStates.idle;
            }
        }
    }

    void SeekMouse()
    {
        // Get direction
        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(point, out RaycastHit hit, 1000))
        {
            // Seek mouse
            // Where we want to go
            Vector3 targetPoint = new Vector3(hit.point.x, this.transform.position.y, hit.point.z) - this.transform.position;
            Vector3 desired_velocity = targetPoint.normalized;

            // Rotate
            Quaternion targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime);

            speedModifier = (Vector3.Dot(transform.forward.normalized, desired_velocity.normalized));
            speedModifier *= -speedModifier;

            this.GetComponent<Rigidbody>().AddForce(transform.forward * speed * speedModifier);
        }
    }
}
