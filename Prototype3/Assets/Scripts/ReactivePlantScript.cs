using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactivePlantScript : MonoBehaviour
{
    /// <summary>
    /// Reference to the player object.
    /// </summary>
    GameObject player;

    /// <summary>
    /// Reference to fireflies particle effect.
    /// </summary>
    ParticleSystem fireflies;

    bool playerCloseLastUpdate = false;

    float timeSincePlayerClose = 0F;

    public enum ActivityStates
    {
        calm,
        excited
    }

    ActivityStates state = ActivityStates.calm;

    float DistanceFromPlayer()
    {
        var thisPos = this.transform.position;
        thisPos.y = 0F;
        var playerPos = player.transform.position;
        playerPos.y = 0F;
        return (thisPos - playerPos).magnitude;
    }

    private void Excite()
    {
        state = ActivityStates.excited;
        fireflies.Play();
        var main = fireflies.main;
        main.simulationSpeed = 10F;
    }

    private void Calm()
    {
        state = ActivityStates.calm;
        fireflies.Stop();
        timeSincePlayerClose = 0F;
    }

    void Awake()
    {
        player = GameObject.Find("Player");
        fireflies = transform.GetChild(0).GetComponent<ParticleSystem>();
        fireflies.Stop();
    }

    void Update()
    {
        // gets the distance between the player and self
        float distance = DistanceFromPlayer();
        bool playerClose = (distance < .5F);
        if (playerClose && state == ActivityStates.calm && !playerCloseLastUpdate)
        {
            Excite();
        }
        if (state == ActivityStates.excited)
        {
            timeSincePlayerClose += Time.deltaTime;
            
            if (timeSincePlayerClose > 1F)
            {
                var main = fireflies.main;
                //main.simulationSpeed = 10F / Mathf.Pow(timeSincePlayerClose, 3F);
                main.simulationSpeed = 10F / timeSincePlayerClose;
            }

            if (timeSincePlayerClose >= 3F)
            {
                Calm();
            }
        }

        playerCloseLastUpdate = playerClose;
    }
}
