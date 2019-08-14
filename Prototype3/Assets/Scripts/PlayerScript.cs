using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Private Variables
    private Vector3 playerVelocity;
    private float speed = 5.0f;
    private float leftRightAxis = 0.0f;
    private float upDownAxis = 0.0f;
    private bool atTarget = false;
    private Vector3 targetPosition;

    private void Awake()
    {
        playerVelocity = new Vector3(0, 0, 0);
        targetPosition = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
    {
        // If clicking
        if (Input.GetMouseButton(0))
        {
            SetTargetPosition();
            // If at target, do not move
            if (!atTarget)
            {
                MoveTowardsPoint();
            }
        }
    }

    // Moves player towards the point
    private void MoveTowardsPoint()
    {
        // Look at point
        this.transform.LookAt(new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z));

        // Change velocity
        playerVelocity = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z) - this.transform.position;

        // Move player
        this.GetComponent<Rigidbody>().AddForce(playerVelocity.normalized * speed);
    }

    // Sets the position target needs to go to
    private void SetTargetPosition()
    {
        // Get the point where the mouse clicked
        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If it hit something
        if (Physics.Raycast(point, out hit, 1000))
        {
            // If hit player
            if (hit.transform == this.transform)
            {
                atTarget = true;
            }
            else
            {
                targetPosition = hit.point;
                atTarget = false;
            }
        }
    }

    private void DebugMovement()
    {
        leftRightAxis = Input.GetAxisRaw("Horizontal");
        upDownAxis = Input.GetAxisRaw("Vertical");

        playerVelocity = new Vector3(leftRightAxis, 0, upDownAxis);

        this.GetComponent<Rigidbody>().AddForce(playerVelocity.normalized * speed);
    }
}
