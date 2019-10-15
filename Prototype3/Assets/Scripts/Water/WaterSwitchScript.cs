using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSwitchScript : MonoBehaviour
{
    // Private
    GameObject water;
    bool triggered = false;
    GameObject player;
    float triggerDistance = 3.0f;
    int callCount = 0;

    void Awake()
    {
        water = GameObject.Find("WaterPlane_1");
        player = GameObject.FindGameObjectWithTag("Player");
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
