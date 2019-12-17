using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlexandersLightObject : MonoBehaviour
{
    private SoundEffectObject trackedSoundEffectObject;

    public void PlaySoundEffect(int sound)
    {
        SoundEffectsManager.Instance.PlaySoundEffectAt((SoundEffects)sound, this.transform.position);
    }
    public void TrackAndPlaySoundEffect(int sound)
    {
        trackedSoundEffectObject = SoundEffectsManager.Instance.PlaySoundEffectAt((SoundEffects)sound, this.transform.position);
    }
    public void KillTrackedSoundEffect( )
    {
        if (trackedSoundEffectObject != null)
        {
            trackedSoundEffectObject.SoundEffectAudioSource.Stop();
            Destroy(trackedSoundEffectObject.gameObject);
        }

    }

}
