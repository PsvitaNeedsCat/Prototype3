using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Public Variables
    public float climbMaxDistance;
    public float climbSpeed;
    public ParticleSystem touch;

    // Stuff for particle effect manipulation
    private float timeToWait = 0.0F;
    private float timePassed = 0.0F;
    private GameObject caller = null;

    // Private Variables
    private Vector3 playerVelocity;
    private readonly float speed = 5.0f;
    private float leftRightAxis = 0.0f;
    private float upDownAxis = 0.0f;
    private bool atTarget = false;
    private Vector3 targetPosition;

    private void Awake()
    {
        playerVelocity = new Vector3(0, 0, 0);
        targetPosition = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        if (caller != null)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= timeToWait)
            {
                caller.GetComponent<MonoBehaviour>().Invoke("Interaction", 0);
                timePassed = 0.0F;
                timeToWait = 0.0F;
                caller = null;
            }
        }
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
        //RaycastHit hit;

        // If it hit something
        if (Physics.Raycast(point, out RaycastHit hit, 1000))
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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "EnvironmentClimbable")
        {
            Collider theirCollider = collision.gameObject.GetComponent<Collider>();
            Collider thisCollider = this.GetComponent<Collider>();
            // check that the player is not standing on top of the object
            if (thisCollider.bounds.min.y < theirCollider.bounds.max.y)
            {
                // check that the player is within the distance they need to be from the top of the object
                if (thisCollider.bounds.min.y + climbMaxDistance >= theirCollider.bounds.max.y)
                {
                    // apply a force upwards
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0F, 9.81F * climbSpeed, 0.0F));
                }
            }
        }
    }

    public void PlayLightCircle()
    {
        touch.Play();
    }

    public void DynamicLightEffect(float _timeToWait, GameObject _caller)
    {
        PlayLightCircle();
        timeToWait = _timeToWait;
        caller = _caller;
    }
}
