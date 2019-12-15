using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlexSounds : MonoBehaviour
{
    public void Step()
    {
        SoundEffectsManager.Instance.PlaySoundEffectAt(SoundEffects.WalkStep, this.transform.position, 0.15f);
    }
}
