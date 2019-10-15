using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour
{
    // Public
    /// <summary>
    /// The possible states that the follower's animation controller can be in.
    /// </summary>
    public enum AnimationStates
    {
        idle,
        walking,
        sitting,
        jumping
    }

    /// <summary>  
    /// Determines how much taller (than the y level of the player's feet) a climbable object can be.
    /// </summary>  
    [SerializeField] float climbMaxDistance = 1.5F;
    [SerializeField] float climbForce = 1.0F;
    public bool isClimbing = false;

    /// <summary>
    /// Played when the follower sits.
    /// </summary>
    [SerializeField] ParticleSystem touchPulse;
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
    public bool isSitting = true;
    /// <summary>
    /// Stores the animation that the follower model is currently displaying.
    /// </summary>
    public AnimationStates currentState = AnimationStates.idle;

    // Private
    int score = 0;
    bool stationary = true;
    public bool isBoating = false;
    /// <summary>
    /// Reference to the current boat
    /// </summary>
    public GameObject curBoat = null;

    // Functions
    /// <summary>
    /// Returns <see cref="score"/>
    /// </summary>
    public int GetLightScore()
    {
        return score;
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
        Rigidbody rb = this.GetComponent<Rigidbody>();
        if (rb.velocity.magnitude > 0.1F && !isSitting && rb.velocity.y < 0.1F)
        {
            currentState = AnimationStates.walking;
        }
        else if (isSitting)
        {
            currentState = AnimationStates.sitting;
        }
        else if (rb.velocity.y >= 0.1F)
        {
            currentState = AnimationStates.jumping;
        }
        else if (rb.velocity.magnitude < 0.1F)
        {
            currentState = AnimationStates.idle;
        }
        Animator animator = GetComponent<Animator>();
        switch (currentState)
        {
            case AnimationStates.idle:
                animator.SetBool("Idle", true);
                animator.SetBool("Walk", false);
                animator.SetBool("Sit", false);
                animator.SetBool("Jump", false);
                break;
            case AnimationStates.walking:
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", true);
                animator.SetBool("Sit", false);
                animator.SetBool("Jump", false);
                break;
            case AnimationStates.sitting:
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("Sit", true);
                animator.SetBool("Jump", false);
                break;
            case AnimationStates.jumping:
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("Sit", false);
                animator.SetBool("Jump", true);
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

            score += 1;
        }
    }

    // Calls every frame.
    private void FixedUpdate()
    {
        AnimationUpdate();

        if (!isBoating)
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
        }
    }

    // Calls when mouse clicks on this.
    private void OnMouseDown()
    {
        if (!isBoating)
        {
            // If moving
            if (!stationary)
            {
                //GameObject[] colliders = GameObject.FindGameObjectsWithTag("LeaveArea");

                //foreach (GameObject obj in colliders)
                //{
                    //if (this.GetComponent<Collider>().bounds.Intersects(obj.GetComponent<Collider>().bounds))
                    //{
                        // Spawn particles
                        //touchPulse.Play();
                        isSitting = !isSitting;
                        stationary = !stationary;
                        //break;
                    //}
                //}
            }
            // If not moving
            else
            {
                // Spawn particles
                //touchPulse.Play();
                isSitting = !isSitting;
                stationary = !stationary;
            }
        }
    }
    void OnCollisionStay(Collision collision)
    {
        Collider theirCollider = collision.gameObject.GetComponent<Collider>();
        Collider thisCollider = this.GetComponent<Collider>();
        // check that the player is standing on top of the object
        if (thisCollider.bounds.min.y + 0.01 >= theirCollider.bounds.max.y)
        {
            if (isClimbing) { isClimbing = false; }
        }
        else if (collision.gameObject.tag == "EnvironmentClimbable")
        {
            // check that the player is not standing on top of the object
            if (thisCollider.bounds.min.y + 0.01 < theirCollider.bounds.max.y)
            {
                // check that the player is within the distance they need to be from the top of the object
                if (thisCollider.bounds.min.y + climbMaxDistance >= theirCollider.bounds.max.y)
                {
                    // apply a force upwards
                    float force = (9.81F + 9.81F * climbForce) * rb.mass;
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0F, force, 0.0F));
                    isClimbing = true;
                }
            }
        }
    }
}
