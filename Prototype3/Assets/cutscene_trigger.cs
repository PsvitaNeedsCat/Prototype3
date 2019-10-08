using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class cutscene_trigger : MonoBehaviour
{

    public PlayableDirector timeline;
    public GameObject Timeline_O;

    // Use this for initialization
    void Start()
    {
        timeline = GetComponent<PlayableDirector>();
    }


    void OnTriggerExit(Collider c)
    {
        /*
        if (c.gameObject.tag == "Player")
        {
            timeline.Stop();
        }
        */
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            Timeline_O.SetActive(true);
            timeline.Play();
        }
    }
}