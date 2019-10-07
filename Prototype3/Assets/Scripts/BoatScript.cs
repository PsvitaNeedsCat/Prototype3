using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour
{
    // Public

    /// <summary>
    /// Reference to the front collider - For the player to sit.
    /// </summary>
    public SphereCollider frontCollider;

    /// <summary>
    /// Reference to the middle collider - For the characters to walk into.
    /// </summary>
    public SphereCollider middleCollider;

    /// <summary>
    /// Reference to the back collider - For the follower to sit.
    /// </summary>
    public SphereCollider backCollider;

    /// <summary>
    /// Vector facing front of boat
    /// </summary>
    public Vector3 vecForward;

    /// <summary>
    /// States the boat can be in.
    /// </summary>
    public enum States
    {
        STATIC,
        PLAYER_CONTROL,
    }
    /// <summary>
    /// Current state the boat is in.
    /// </summary>
    public States curState = States.STATIC;

    // Private

    float speed = 5.0f;

    // Called every frame
    void FixedUpdate()
    {
        // Update vector
        vecForward = Vector3.forward;

        switch(curState)
        {
            // Static - Not moving
            case States.STATIC:
                {
                    // Freeze object
                    this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    break;
                }
            
            // Player is in control of boat
            case States.PLAYER_CONTROL:
                {
                    if (Input.GetMouseButton(0))
                    {
                        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

                        // Get the point where the mouse clicked
                        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);

                        // If it hit something
                        if (Physics.Raycast(point, out RaycastHit hit, 1000))
                        {
                            // Look at the point
                            this.transform.LookAt(new Vector3(hit.point.x, this.transform.position.y, hit.point.z));

                            // Move player
                            Vector3 force = (new Vector3(hit.point.x, this.transform.position.y, hit.point.z) - this.transform.position).normalized * speed;
                            if (Input.GetAxis("Sprint") == 1.0F)
                            {
                                force *= 2.0F;
                            }
                            this.GetComponent<Rigidbody>().AddForce(force);
                        }
                    }

                    break;
                }

            default:
                break;
        }
    }
}
