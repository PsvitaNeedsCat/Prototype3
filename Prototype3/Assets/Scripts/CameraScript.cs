using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public float Yoffset;
    public float Zoffset;

    private void FixedUpdate()
    {
        // The player's position
        Vector3 playerPos = player.transform.position;

        // Where the camera should be
        Vector3 playerPosWithOffset = playerPos;
        playerPosWithOffset.y += Yoffset;
        playerPosWithOffset.z += Zoffset;

        // Move the camera to where it should be
        this.transform.position = playerPosWithOffset;
    }
}
