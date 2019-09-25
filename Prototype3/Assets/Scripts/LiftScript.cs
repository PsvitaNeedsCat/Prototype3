using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    // Public var
    public Transform pointA;
    public Transform pointB;
    public bool active = false;

    // Private var
    private Transform currentPoint;
    private float timer = 0.0f;
    private const float timerMax = 5.0f;
    private const float speed = 2.0f;
    private const float radiusCheck = 0.5f;

    private enum State
    {
        MOVING,
        WAITING,
        OFF
    }
    State curState = State.MOVING;

    private void Awake()
    {
        currentPoint = pointA;
    }

    private void FixedUpdate()
    {
        if (!active)
        {
            curState = State.OFF;
        }
        else if (curState == State.OFF)
        {
            curState = State.MOVING;
            SwapPoints();
        }
        if (curState == State.WAITING)
        {
            // Count timer
            timer += Time.fixedDeltaTime;

            // Check timer
            if (timer >= timerMax)
            {
                // Reset timer
                timer = 0.0f;

                // Change point
                SwapPoints();

                // Change state
                curState = State.MOVING;
            }
        }
        else if (curState == State.OFF)
        {
            // Check if at point
            if ((this.transform.position - currentPoint.position).magnitude < radiusCheck)
            {
                this.transform.position = currentPoint.transform.position;
            }
            else
            {
                // Move towards point
                Vector3 direction = currentPoint.position - this.transform.position;

                this.GetComponent<Rigidbody>().velocity = (direction.normalized * speed);
            }
        }
        // State.MOVING
        else
        {
            // Move towards point
            Vector3 direction = currentPoint.position - this.transform.position;

            this.GetComponent<Rigidbody>().velocity = (direction.normalized * speed);

            // Check if at point
            if ((this.transform.position - currentPoint.position).magnitude < radiusCheck)
            {
                // Wait
                curState = State.WAITING;
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
}
