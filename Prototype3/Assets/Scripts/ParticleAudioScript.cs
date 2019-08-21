using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAudioScript : MonoBehaviour
{
    private AudioSource chime;

    private void Awake()
    {
        chime = this.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (this.GetComponent<ParticleSystem>().isPlaying && !chime.isPlaying)
        {
            chime.Play();
        }
    }
}
