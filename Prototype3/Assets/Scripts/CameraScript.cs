using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Variables
    /// <summary>
    /// Reference for the camera's target
    /// </summary>
    [SerializeField] GameObject target;

    /// <summary>
    /// How far above the target the camera needs to stay.
    /// </summary>
    [SerializeField] float Yoffset;

    /// <summary>
    /// How far behind the target the camera needs to stay.
    /// </summary>
    [SerializeField] float Zoffset;

    // Functions
    /// <summary>
    /// Sets the camera's position, relative to the target
    /// </summary>
    void SetPosBasedOnTarget()
    {
        // The player's position
        Vector3 playerPos = target.transform.position;

        // Where the camera should be
        Vector3 playerPosWithOffset = playerPos;
        playerPosWithOffset.y += Yoffset;
        playerPosWithOffset.z += Zoffset;

        // Move the camera to where it should be
        this.transform.position = playerPosWithOffset;
    }

    private void FixedUpdate()
    {
        SetPosBasedOnTarget();
    }

}
