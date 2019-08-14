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
    private Vector3 targetPosition;

    private void Awake()
    {
        playerVelocity = new Vector3(0, 0, 0);
        targetPosition = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
    {
        DebugMovement();
    }

    

    // Sets the position target needs to go to
    private void SetTargetPosition()
    {
        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(point, out hit, 1000))
        {
            targetPosition = hit.point;
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
