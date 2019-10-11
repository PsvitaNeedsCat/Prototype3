using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierScript : MonoBehaviour
{
    /// <summary>  
    /// Determines how quickly the player is pushed up.
    /// </summary>  
    [SerializeField] float pushForce = 0.5F;

    public float GetPushForce()
    {
        return pushForce;
    }
}
