using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Public
    /// <summary>
    /// The possible states that the player's animation controller can be in.
    /// </summary>
    public enum AnimationStates
    {
        idle,
        walking,
        jumping
    }
    /// <summary>  
    /// Determines how much taller (than the y level of the player's feet) a climbable object can be.
    /// </summary>  
    [SerializeField] float climbMaxDistance = 1.5F;
    [SerializeField] float climbForce = 1.0F;
    /// <summary>
    /// Played when the player interacts with something.
    /// </summary>
    [SerializeField] ParticleSystem touchPulse;
    [SerializeField] float speed;
    public bool isBoating = false;

    // Private
    AnimationStates currentState = AnimationStates.idle;
    bool isClimbing = false;
    /// <summary>
    /// Stores whether or not the left mouse button was down last frame
    /// </summary>
    bool LMBLastFrame = false;
    /// <summary>
    /// Stores whether or not the the player started the current press & hold on a switch
    /// </summary>
    bool ClickedOnSwitch = false;
    /// <summary>
    /// Stores whether or not the player has dragged off the switch (to start moving towards it)
    /// </summary>
    bool HasMousedOffSwitch = false;
    GameObject follower;

    // Functions
    void MoveTowardsCursor()
    {
        // Get the point where the mouse clicked
        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);

        // If it hit something
        if (Physics.Raycast(point, out RaycastHit hit, 1000))
        {
            bool moveToPoint = false;
            bool mouseOnSwitch = hit.collider.gameObject.tag == "Switch";

            // Determines whether or not the player just clicked on the switch,
            // Or if they have done that in a previous frame and just dragged their cursor off the switch 
            if (mouseOnSwitch && !LMBLastFrame) { ClickedOnSwitch = true; }
            else if (ClickedOnSwitch && !mouseOnSwitch) { HasMousedOffSwitch = true; }

            // if the player did not start the click on the switch, they may move
            // otherwise, if they have since dragged their cursor off the switch, they may move
            if (!ClickedOnSwitch) { moveToPoint = true; }
            else if (HasMousedOffSwitch) { moveToPoint = true; }

            if (moveToPoint)
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
    }

    /// <summary>
    /// Plays <see cref="touchPulse"/>, and then after _timeToWait seconds, calls _caller.Interaciton()
    /// </summary>
    /// <param name="_timeToWait">
    /// The amount of time to wait before calling Interaction() on _caller
    /// </param>
    /// <param name="_caller">
    /// The object that called this.
    /// </param>
    public void DynamicLightEffect(float _timeToWait, GameObject _caller)
    {
        touchPulse.Play();
        _caller.GetComponent<MonoBehaviour>().Invoke("Interaction", _timeToWait);
    }

    /// <summary>
    /// Manages currentState and updates the player's animator component.
    /// </summary>
    void AnimationUpdate()
    {
        Animator thisAnimator = GetComponent<Animator>();
        if (this.GetComponent<Rigidbody>().velocity.magnitude > 0.2F && !isClimbing)
        {
            currentState = AnimationStates.walking;
        }
        else if (isClimbing)
        {
            currentState = AnimationStates.jumping;
        }
        else if (this.GetComponent<Rigidbody>().velocity.magnitude <= 0.2F)
        {
            currentState = AnimationStates.idle;
        }
        switch (currentState)
        {
            case AnimationStates.idle:
                thisAnimator.SetBool("Idle", true);
                thisAnimator.SetBool("Walking", false);
                thisAnimator.SetBool("Jumping", false);
                break;
            case AnimationStates.walking:
                thisAnimator.SetBool("Idle", false);
                thisAnimator.SetBool("Walking", true);
                thisAnimator.SetBool("Jumping", false);
                break;
            case AnimationStates.jumping:
                thisAnimator.SetBool("Idle", false);
                thisAnimator.SetBool("Walking", false);
                thisAnimator.SetBool("Jumping", true);
                break;
        }
    }

    void Awake()
    {
        follower = GameObject.FindGameObjectWithTag("Follower");
    }

    // Calls every frame.
    void FixedUpdate()
    {
        AnimationUpdate();
        if (!isBoating) // Only give control of player if not boating
        {
            // If clicking
            bool LMBdown = Input.GetMouseButton(0);

            if (LMBdown)
            {
                MoveTowardsCursor();
            }
            else
            {
                // Resets ClickedOnSwitch and HasMousedOffSwitch
                ClickedOnSwitch = false;
                HasMousedOffSwitch = false;
            }
            LMBLastFrame = LMBdown;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "EnvironmentClimbable")
        {
            Collider theirCollider = collision.gameObject.GetComponent<Collider>();
            Collider thisCollider = this.GetComponent<Collider>();
            // check that the player is not standing on top of the object
            if (thisCollider.bounds.min.y + 0.01 < theirCollider.bounds.max.y)
            {
                // check that the player is within the distance they need to be from the top of the object
                if (thisCollider.bounds.min.y + climbMaxDistance >= theirCollider.bounds.max.y)
                {
                    // apply a force upwards
                    float force = 9.81F + 9.81F * climbForce;
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0F, force, 0.0F));
                    isClimbing = true;
                }
            }
            else
            {
                isClimbing = false;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "EnvironmentClimbable")
        {
            isClimbing = false;
        }
    }

}
