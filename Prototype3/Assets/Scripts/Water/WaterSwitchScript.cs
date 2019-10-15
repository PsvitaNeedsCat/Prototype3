using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSwitchScript : MonoBehaviour
{
    // Public/Serialized
    [SerializeField] Material farTex;
    [SerializeField] Material nearTex;

    // Private
    GameObject water;
    bool triggered = false;
    GameObject player;
    float triggerDistance = 5.0f;
    int callCount = 0;
    bool playerNear = false;

    void Awake()
    {
        water = GameObject.Find("WaterPlane_1");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // If player was not near but now is near
        if ((player.transform.position - this.transform.position).magnitude < triggerDistance)
        {
            if (!playerNear)
            {
                playerNear = true;
                this.GetComponent<MeshRenderer>().material = nearTex;
            }
        }
        // If player was near but now is far
        else
        {
            if (playerNear)
            {
                playerNear = false;
                this.GetComponent<MeshRenderer>().material = farTex;
            }
        }
    }

    void OnMouseDown()
    {
        if (!triggered && (player.transform.position - this.transform.position).magnitude < triggerDistance)
        {
            // Raise water
            triggered = true;

            water.GetComponent<WaterScript>().RaiseWater();
        }
    }
}
