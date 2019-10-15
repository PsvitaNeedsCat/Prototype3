using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSelf : MonoBehaviour
{
    [SerializeField] float timeTillDeletion;
    float timeSinceSpawn;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceSpawn = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn >= timeTillDeletion)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
