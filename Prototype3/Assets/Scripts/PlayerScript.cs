using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Layer mask
    [SerializeField] LayerMask ignoreCollision;

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
    [SerializeField] float selfLiftForce = 3.0F;
    [SerializeField] float glideForce = 0.7F;
    /// <summary>
    /// Played when the player interacts with something.
    /// </summary>
    [SerializeField] ParticleSystem touchPulse;
    [SerializeField] GameObject jumpParticleSystem;
    [SerializeField] float speed = 5.0F;
    public bool isBoating = false;

    // Private
    AnimationStates currentState = AnimationStates.idle;
    public bool isClimbing = false;
    public bool isGliding = false;
    /// <summary>
    /// Stores whether or not the left mouse button was down last frame
    /// </summary>
    bool LMBLastFrame = false;
    bool hitPlayerThisFrame = false;
    bool hitPlayerLastFrame = false;
    /// <summary>
    /// Stores whether or not the the player started the current press & hold on a switch
    /// </summary>
    bool ClickedOnSwitch = false;
    /// <summary>
    /// Stores whether or not the player has dragged off the switch (to start moving towards it)
    /// </summary>
    bool HasMousedOffSwitch = false;

    public bool isTouchingGround = true;
    float selfLiftTime = 0.0F;
    float maxSelfLiftTime = 0.5f;
    GameObject follower;
    GameObject raycastPlane;
    Rigidbody rb;

    bool bushAudioPlaying = false;

    // Functions

    float Distance(GameObject _object1, GameObject _object2)
    {
        return (_object1.transform.position - _object2.transform.position).magnitude;
    }

    public void PlayStepAudio()
    {
        AudioSource audio = GetComponents<AudioSource>()[0];
        audio.pitch = Random.Range(0.85F, 1.15F);
        audio.Play();
    }

    public void PlayBushAudio()
    {
        AudioSource audio = GetComponents<AudioSource>()[1];
        audio.Play();
        bushAudioPlaying = true;
    }

    public void StopBushAudio()
    {
        AudioSource audio = GetComponents<AudioSource>()[1];
        GracefulStopBushAudio();
    }

    public void GracefulStopBushAudio()
    {
        AudioSource audio = GetComponents<AudioSource>()[1];
        if (audio.time >= audio.clip.length - audio.clip.length * 0.35F)
        {
            audio.Stop();
            bushAudioPlaying = false;
        }
        else
        {
            Invoke("GracefulStopBushAudio", 0.02F);
        }
    }

    void MoveTowardsCursor()
    {
        // Get the point where the mouse clicked
        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);

        // If it hit something
        if (Physics.Raycast(point, out RaycastHit hit, 1000, ignoreCollision))
        {
            bool moveToPoint = false;
            bool mouseOnSwitch = hit.collider.gameObject.tag == "Switch";
            hitPlayerThisFrame = hit.collider.gameObject.tag == "Player";

            // Determines whether or not the player just clicked on the switch,
            // Or if they have done that in a previous frame and just dragged their cursor off the switch 
            if (mouseOnSwitch && !LMBLastFrame) { ClickedOnSwitch = true; }
            else if (ClickedOnSwitch && !mouseOnSwitch) { HasMousedOffSwitch = true; }

            // if the player did not start the click on the switch, they may move
            // otherwise, if they have since dragged their cursor off the switch, they may move
            if (!ClickedOnSwitch) { moveToPoint = true; }
            else if (HasMousedOffSwitch) { moveToPoint = true; }

            if (hitPlayerThisFrame) { moveToPoint = false; }
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
                if (isTouchingGround)
                {
                    currentState = AnimationStates.walking;
                }
            }

            if (!hitPlayerLastFrame && LMBLastFrame)
            {
                hitPlayerThisFrame = false;
            }


            if (hitPlayerThisFrame && selfLiftTime <= maxSelfLiftTime && !isGliding)
            {
                // apply a force upwards
                float force = (9.81F + 9.81F * selfLiftForce) * rb.mass;
                this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0F, force, 0.0F));
                isClimbing = true;
                currentState = AnimationStates.jumping;
                if (isTouchingGround && !hitPlayerLastFrame)
                {
                    JumpParticles();
                }
            }
            else if (hitPlayerThisFrame && selfLiftTime > maxSelfLiftTime)
            {
                isGliding = true;
                isClimbing = false;
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

    public void JumpParticles()
    {
        var jumpPS = GameObject.Instantiate(jumpParticleSystem);
        jumpPS.transform.position = this.transform.position;
        jumpPS.GetComponent<ParticleSystem>().Play();
        jumpPS.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Manages currentState and updates the player's animator component.
    /// </summary>
    void AnimationUpdate()
    {
        Animator thisAnimator = GetComponent<Animator>();
        if (this.GetComponent<Rigidbody>().velocity.magnitude > 0.2F && isTouchingGround)
        {
            currentState = AnimationStates.walking;
        }
        else if (this.GetComponent<Rigidbody>().velocity.magnitude <= 0.2F && isTouchingGround)
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

    GameObject GetClosestBush()
    {
        GameObject[] bushes = GameObject.FindGameObjectsWithTag("Bush");
        if (bushes.Length > 0)
        {
            GameObject currentClosest = null;
            float currentShortestDistance = float.MaxValue;
            for (int i = 0; i < bushes.Length; i++)
            {
                if (i == 0)
                {
                    currentClosest = bushes[i];
                    currentShortestDistance = Distance(currentClosest, this.gameObject);
                    continue;
                }
                float distance = Distance(bushes[i], this.gameObject);
                if (distance < currentShortestDistance)
                {
                    currentClosest = bushes[i];
                    currentShortestDistance = distance;
                }
            }
            return currentClosest;
        }
        return null;
    }

    void Awake()
    {
        follower = GameObject.FindGameObjectWithTag("Follower");
        raycastPlane = GameObject.Find("RaycastPlane");
        rb = GetComponent<Rigidbody>();
        //jump = GameObject.Find("Jump - PS").GetComponent<ParticleSystem>();
    }

    // Calls every frame.
    void FixedUpdate()
    {
        hitPlayerThisFrame = false;
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
        AnimationUpdate();
        if (!isTouchingGround)
        {
            if (!isClimbing)
            {
                isGliding = true;
                if (rb.velocity.y < 0)
                {
                    float force = (9.81F * glideForce) * rb.mass;
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0F, force, 0.0F));
                    currentState = AnimationStates.jumping;
                }
            }
            selfLiftTime += Time.deltaTime;
        }
        Animator thisAnimator = GetComponent<Animator>();
        if (currentState == AnimationStates.walking)
        {
            float animationSpeed = Mathf.Clamp(Mathf.Abs(rb.velocity.x * 2.0F) + Mathf.Abs(rb.velocity.z * 2.0F), 0.01F, 2.0F);
            thisAnimator.speed = animationSpeed * 0.5F;
        }
        else
        {
            thisAnimator.speed = 1;
        }

        float velMagnitude = rb.velocity.magnitude;
        GameObject closestBush = GetClosestBush();
        if (closestBush)
        {
            if (velMagnitude > 0.1F && !bushAudioPlaying && Distance(closestBush, this.gameObject) < 0.5F)
            {
                PlayBushAudio();
            }
            if ((velMagnitude < 0.1F || Distance(closestBush, this.gameObject) >= 0.5F) && bushAudioPlaying)
            {
                StopBushAudio();
            }
        }


        // all logic over, reset isTouchingGround and isClimbing.
        if (rb.velocity.y != 0) { isTouchingGround = false; }
        else { isTouchingGround = true; }

        if (hitPlayerLastFrame && !hitPlayerThisFrame)
        {
            selfLiftTime = maxSelfLiftTime;
            isClimbing = false;
            isGliding = true;
        }

        hitPlayerLastFrame = hitPlayerThisFrame;
    }

    private void Update()
    {
        AudioSource audio = GetComponents<AudioSource>()[1];
        if (audio.time >= audio.clip.length - audio.clip.length * 0.3F)
        {
            audio.pitch = Random.Range(0.85F, 1.15F);
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
            isTouchingGround = true;
            isGliding = false;
            selfLiftTime = 0.0F;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Brazier")
        {
            float pushForce = other.gameObject.GetComponent<BrazierScript>().GetPushForce();
            // apply a force upwards
            float force = 9.81F + 9.81F * pushForce;
            this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0F, force, 0.0F));
            isClimbing = true;
            currentState = AnimationStates.jumping;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Brazier")
        {
            isClimbing = false;
        }
    }
}
