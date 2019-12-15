using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectObject : MonoBehaviour
{
    public AudioSource SoundEffectAudioSource;

    // Update is called once per frame
    void Update()
    {
        if (!SoundEffectAudioSource.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
