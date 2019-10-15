using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    [SerializeField] Transform[] points;
    Vector3[] positions;

    uint currentPoint = 0;
    /// <summary>
    /// How close to the point the water must be to stop
    /// </summary>
    float checkDistance = 0.05f;
    float speed = 3.0f;
    GameObject boat;

    void Awake()
    {
        // Set positions size
        positions = new Vector3[points.Length];

        // Convert transforms to positions
        positions[0] = this.transform.position;
        for (int i = 1; i < points.Length; i++)
        {
            positions[i] = points[i].position;
        }

        boat = GameObject.FindGameObjectWithTag("Boat");
    }

    void FixedUpdate()
    {
        // If points are not close enough
        if (Mathf.Abs(positions[currentPoint].y - transform.position.y) > checkDistance)
        {
            // Move upwards towards point
            Vector3 waterPos = transform.position;
            waterPos.x = positions[currentPoint].x; waterPos.z = positions[currentPoint].z;
            Vector3 direction = (positions[currentPoint] - waterPos).normalized;
            transform.position += direction * speed * Time.fixedDeltaTime;
            boat.transform.position += direction * speed * Time.fixedDeltaTime;
        }
    }

    public void RaiseWater()
    {
        if (currentPoint < positions.Length - 1)
        {
            currentPoint += 1;
        }
    }

    public void LowerWater()
    {
        if (currentPoint > 0)
        {
            currentPoint -= 1;
        }
    }
}
