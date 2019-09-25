using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour
{
    // Public Variables
    public int lightScore = 0;
    public ParticleSystem touch;


    // Stuff for animation
    public enum animationStates
    {
        idle,
        walking,
        sitting
    }
    public animationStates currentState = animationStates.idle;
    public bool isSitting = false;

    // Private variables
    GameObject player;
    const float radiusToPlayer = 1.0f;
    const float speed = 5.0f;
    bool stationary = false;

    // Awake function
    private void Awake()
    {
        // Set player
        player = GameObject.Find("Player");
    }

    // Fixed update function
    private void FixedUpdate()
    {

        // If moving
        if (!stationary)
        {
            // Look at player
            LookAtPlayer();

            // Follow player
            if (!NearPlayer())
            {
                MoveTowardPlayer();
            }
        }
    }

    private void Update()
    {
        AnimationUpdate();
    }

    // When clicked
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
                    touch.Play();
                    isSitting = !isSitting;
                    stationary = !stationary;
                    return;
                }
            }
        }
        // If not moving
        else
        {
            // Spawn particles
            touch.Play();
            isSitting = !isSitting;
            stationary = !stationary;
        }
    }

    // Look at player
    private void LookAtPlayer()
    {
        Vector3 playerPos = player.transform.position;
        playerPos.y = this.transform.position.y;

        this.transform.LookAt(playerPos);
    }

    // Checks if follower is close to the player
    private bool NearPlayer()
    {
        if ((player.transform.position - this.transform.position).magnitude < radiusToPlayer)
        {
            return true;
        }

        return false;
    }

    // Takes a step towards the player
    private void MoveTowardPlayer()
    {
        // Get directional vector
        Vector3 dirVec = player.transform.position - this.transform.position;
        dirVec.y = 0.0f;

        // Move forward
        this.GetComponent<Rigidbody>().AddForce(dirVec.normalized * speed);
    }

    // Collides with trigger object
    private void OnTriggerEnter(Collider other)
    {
        // Picks up light
        if (other.tag == "Light")
        {
            Destroy(other.gameObject);

            lightScore += 1;
        }
    }

    public void AnimationUpdate()
    {
        if (this.GetComponent<Rigidbody>().velocity.magnitude > 0.1F && !isSitting)
        {
            currentState = animationStates.walking;
        }
        else if (isSitting)
        {
            currentState = animationStates.sitting;
        }
        else if (this.GetComponent<Rigidbody>().velocity.magnitude < 0.1F)
        {
            currentState = animationStates.idle;
        }

        switch (currentState)
        {
            case animationStates.idle:
                GetComponent<Animator>().SetBool("Idle", true);
                GetComponent<Animator>().SetBool("Walk", false);
                GetComponent<Animator>().SetBool("Sit", false);
                break;
            case animationStates.walking:
                GetComponent<Animator>().SetBool("Idle", false);
                GetComponent<Animator>().SetBool("Walk", true);
                GetComponent<Animator>().SetBool("Sit", false);
                break;
            case animationStates.sitting:
                GetComponent<Animator>().SetBool("Idle", false);
                GetComponent<Animator>().SetBool("Walk", false);
                GetComponent<Animator>().SetBool("Sit", true);
                break;
        }
    }
}
