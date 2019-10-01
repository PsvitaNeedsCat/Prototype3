using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    // Calls on awake.
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
