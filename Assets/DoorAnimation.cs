using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DoorAnimation : MonoBehaviour
{
    SoundEffectObject doorSoundEffect;
    public UnityEvent WeAreInTheCaveEvent;
    public void DoorRiseSound()
    {
        doorSoundEffect = SoundEffectsManager.Instance.PlaySoundEffectAt(SoundEffects.DoorRise, this.transform.position);
    }
    public void DoorImpactSound()
    {
        doorSoundEffect.SoundEffectAudioSource.clip = SoundEffectsManager.Instance.FindSoundEffect(SoundEffects.DoorImpact);
        doorSoundEffect.SoundEffectAudioSource.Play();
    }
    public void WeAreInTheCave()
    {
        WeAreInTheCaveEvent.Invoke();
    }
}
