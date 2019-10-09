using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAtMidpoint : MonoBehaviour
{
    /// <summary>  
    /// This object will stay at the halfway point between Point A and Point B.
    /// Point A is the Player (if it hasn't been overriden) and Point B is <see cref="target"/>.
    /// </summary>  
    [SerializeField] GameObject target;

    /// <summary>  
    /// Reference to the player. Can be overriden by another GameObject in the editor.
    /// </summary>  
    [SerializeField] GameObject playerOverridable;

    private void Awake()
    {
        if (!playerOverridable) { playerOverridable = GameObject.Find("Player"); }
    }
    void FixedUpdate()
    {
        Vector3 targetPosition = target.transform.position;
        Vector3 playerPosition = playerOverridable.transform.position;
        Vector3 difference = targetPosition - playerPosition;
        Vector3 midPoint = playerPosition + difference * 0.5F;
        this.transform.position = midPoint;
    }
}
