using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    // Public vars
    public GameObject pointA;
    public GameObject pointB;

    // Private vars
    private GameObject currentPoint;
    private float speed = 5.0f;
    private float maxTimer = 5.0f;
    private float timer = 0.0f;
    private GameObject player;
    private bool playerOn = false;

    private enum State
    {
        MOVING,
        WAITING
    }
    State currentState = State.MOVING;

    private void Awake()
    {
        // Set first point
        currentPoint = pointA;

        // Set player
        player = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        if (currentState == State.MOVING)
        {
            // Move towards point
            Vector3 direction = currentPoint.transform.position - this.transform.position;

            this.GetComponent<Rigidbody>().velocity = direction.normalized * speed;
            //this.GetComponent<Rigidbody>().AddForce(direction.normalized * speed);

            if (playerOn)
            {

            }

            // Check if colliding with point
            if (this.GetComponent<Collider>().bounds.Intersects(currentPoint.GetComponent<Collider>().bounds))
            {
                currentState = State.WAITING;
            }
        }
        else
        {
            // Add to timer
            timer += Time.fixedDeltaTime;

            // Check time
            if (timer >= maxTimer)
            {
                // Reset timer
                timer = 0.0f;

                // Change point
                SwapPoints();

                // Change states
                currentState = State.MOVING;
            }
        }
    }

    private void SwapPoints()
    {
        if (currentPoint == pointA)
        {
            currentPoint = pointB;
        }
        else
        {
            currentPoint = pointA;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // If player is on platform & player is higher than platform
        if (collision.collider.tag == "Player" && collision.collider.transform.position.y > this.transform.position.y)
        {
            playerOn = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // If the player left the platform
        if (collision.collider.tag == "Player")
        {
            playerOn = false;
        }
    }
}
