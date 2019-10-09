using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    // Public var
    /// <summary>
    /// First point that lift will travel to.
    /// </summary>
    [SerializeField] Transform pointA;

    /// <summary>
    /// Second point the lift will travel to.
    /// </summary>
    [SerializeField] Transform pointB;

    /// <summary>
    /// Is the lift currently active and moving.
    /// </summary>
    public bool active = false;

    // Private var
    /// <summary>
    /// Current point the lift is travelling to.
    /// </summary>
    Transform currentPoint;

    /// <summary>
    /// Times pause between the 2 points.
    /// </summary>
    float timer = 0.0f;

    /// <summary>
    /// Constant - Maximum length of timer.
    /// </summary>
    const float timerMax = 5.0f;

    /// <summary>
    /// Constant - Speed of the lift.
    /// </summary>
    const float speed = 2.0f;

    /// <summary>
    /// Constant - How close the lift needs to be to the point before it will pause.
    /// </summary>
    const float radiusCheck = 0.5f;

    /// <summary>
    /// States of the lift.
    /// </summary>
    enum State
    {
        MOVING,
        WAITING,
        OFF
    }
    /// <summary>
    /// Current state of the lift.
    /// </summary>
    State curState = State.MOVING;

    // Calls on awake.
    void Awake()
    {
        currentPoint = pointA;
    }

    // Calls every frame.
    void FixedUpdate()
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
            this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);

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
                this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
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

    /// <summary>
    /// Swaps between the 2 points.
    /// </summary>
    void SwapPoints()
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
