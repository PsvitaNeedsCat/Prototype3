using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour
{
    // Definitions
    /// <summary>
    /// The possible states that the follower's animation controller can be in.
    /// </summary>
    public enum AnimationStates
    {
        idle,
        walking,
        sitting
    }

    // Variables
    /// <summary>
    /// How many "lights" the follower has picked up so far
    /// </summary>
    int lightScore = 0;

    /// <summary>
    /// Played when the follower sits.
    /// </summary>
    [SerializeField] ParticleSystem touchPulse;
    
    /// <summary>
    /// Stores the animation that the follower model is currently displaying.
    /// </summary>
    AnimationStates currentState = AnimationStates.idle;

    /// <summary>
    /// Stores whether or not the follower is sitting
    /// </summary>
    bool isSitting = true;

    /// <summary>
    /// The follower's target
    /// </summary>
    [SerializeField] GameObject followTarget;

    /// <summary>
    /// How close to follow <see cref="followTarget"/>
    /// </summary>
    [SerializeField] float followDistance = 1.0F;

    /// <summary>
    /// Determines the speed at which the follower moves.
    /// </summary>
    [SerializeField] float force = 5.0F;

    /// <summary>
    /// Stores whether or not the follower is stationary
    /// </summary>
    bool stationary = true;

    // Functions
    /// <summary>
    /// Returns <see cref="lightScore"/>
    /// </summary>
    public int GetLightScore()
    {
        return lightScore;
    }

    /// <summary>
    /// Turns the follower to look at <see cref="followTarget"/>
    /// </summary>
    void LookAtTarget()
    {
        Vector3 playerPos = followTarget.transform.position;
        playerPos.y = this.transform.position.y;

        this.transform.LookAt(playerPos);
    }

    /// <summary>
    /// Checks if the follower is within ||<see cref="followDistance"/>|| from ||<see cref="followTarget"/>||
    /// </summary>
    bool NearTarget()
    {
        return (followTarget.transform.position - this.transform.position).magnitude < followDistance;
    }

    /// <summary>
    /// Pushes the follower towards its target
    /// </summary>
    void MoveTowardTarget()
    {
        // Get directional vector
        Vector3 dirVec = followTarget.transform.position - this.transform.position;
        dirVec.y = 0.0f;
        // Move in the direction
        this.GetComponent<Rigidbody>().AddForce(dirVec.normalized * force);
    }

    /// <summary>
    /// Manages currentState and updates the follower's animator component.
    /// </summary>
    void AnimationUpdate()
    {
        if (this.GetComponent<Rigidbody>().velocity.magnitude > 0.1F && !isSitting)
        {
            currentState = AnimationStates.walking;
        }
        else if (isSitting)
        {
            currentState = AnimationStates.sitting;
        }
        else if (this.GetComponent<Rigidbody>().velocity.magnitude < 0.1F)
        {
            currentState = AnimationStates.idle;
        }

        switch (currentState)
        {
            case AnimationStates.idle:
                GetComponent<Animator>().SetBool("Idle", true);
                GetComponent<Animator>().SetBool("Walk", false);
                GetComponent<Animator>().SetBool("Sit", false);
                break;
            case AnimationStates.walking:
                GetComponent<Animator>().SetBool("Idle", false);
                GetComponent<Animator>().SetBool("Walk", true);
                GetComponent<Animator>().SetBool("Sit", false);
                break;
            case AnimationStates.sitting:
                GetComponent<Animator>().SetBool("Idle", false);
                GetComponent<Animator>().SetBool("Walk", false);
                GetComponent<Animator>().SetBool("Sit", true);
                break;
        }
    }

    // Calls when this enters a trigger collision.
    void OnTriggerEnter(Collider other)
    {
        // Picks up light
        if (other.tag == "Light")
        {
            Destroy(other.gameObject);

            lightScore += 1;
        }
    }

    // Calls every frame.
    private void FixedUpdate()
    {
        // If moving
        if (!stationary)
        {
            // Look at player
            LookAtTarget();

            // Follow player
            if (!NearTarget())
            {
                MoveTowardTarget();
            }
        }

        AnimationUpdate();
    }

    // Calls when mouse clicks on this.
    private void OnMouseDown()
    {
        // If moving
        if (!stationary)
        {
            GameObject[] colliders = GameObject.FindGameObjectsWithTag("LeaveArea");

            foreach (GameObject obj in colliders)
            {
                if (this.GetComponent<Collider>().bounds.Intersects(obj.GetComponent<Collider>().bounds))
                {
                    // Spawn particles
                    touchPulse.Play();
                    isSitting = !isSitting;
                    stationary = !stationary;
                    break;
                }
            }
        }
        // If not moving
        else
        {
            // Spawn particles
            touchPulse.Play();
            isSitting = !isSitting;
            stationary = !stationary;
        }
    }
}
