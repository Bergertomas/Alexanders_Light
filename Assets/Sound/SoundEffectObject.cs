using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectObject : MonoBehaviour
{
    public AudioSource SoundEffectAudioSource;
    void Update()
    {
        if (!SoundEffectAudioSource.isPlaying&& !SoundEffectAudioSource.loop)
        {
            Destroy(this.gameObject);
        }
    }
}
