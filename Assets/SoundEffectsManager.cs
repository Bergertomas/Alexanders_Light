using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundEffects
{
    Drag = 0, Jump = 1, DoorRise = 2, DoorImpact = 3, WalkStep=4,JumpVoice=5, FireStart=6, FireCrackle=7,
}

[System.Serializable]
public class SoundEffect
{
    public SoundEffects SoundName;
    public AudioClip[] Clips;
    public float DesiredVolume;
    public float DesiredPitch;
}

public class SoundEffectsManager : MonoBehaviour
{
    [SerializeField]
    private SoundEffect[] soundEffects;
    [SerializeField]
    SoundEffectObject soundEffectPrefab;
    public static SoundEffectsManager Instance;
    [SerializeField]
    private AudioMixer soundEffectsMixer;

    private void Awake()
    {
        Debug.Log("I am a public whore (-;");
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Tried to create another SoundEffectsManager.");
            Destroy(this);
        }
    }

    public void CaveState()
    {
        soundEffectsMixer.SetFloat("ReverbParam", 0);
    }
    public void OutdoorsState()
    {
        soundEffectsMixer.SetFloat("ReverbParam", -10000f);
    }
    public SoundEffectObject PlaySoundEffectAt(SoundEffects soundEffect, Vector3 position, float randomisePitch=0,bool loop=false)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].SoundName == soundEffect)
            {
                SoundEffectObject soundObject = Instantiate(soundEffectPrefab, position, Quaternion.identity);
                //AudioSource audioSource = soundObject.AddComponent<AudioSource>();
                //audioSource.outputAudioMixerGroup=
                soundObject.SoundEffectAudioSource.clip = soundEffects[i].Clips[Random.Range(0, soundEffects[i].Clips.Length)];
                soundObject.SoundEffectAudioSource.volume = soundEffects[i].DesiredVolume;
                soundObject.SoundEffectAudioSource.pitch += Random.Range(-randomisePitch, randomisePitch);
                soundObject.SoundEffectAudioSource.loop=loop;
                soundObject.SoundEffectAudioSource.Play();
                return soundObject;
            }
        }
        {
            Debug.Log("Failed to find the soundEffect");
            return null;
        }
    }

    public AudioClip FindSoundEffect(SoundEffects soundEffect)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].SoundName == soundEffect)
            {
                return soundEffects[i].Clips[Random.Range(0, soundEffects[i].Clips.Length)];
            }
        }
        return null;
    }

    public SoundEffectObject StartFireLoop(Vector3 position)
    {
        PlaySoundEffectAt(SoundEffects.FireStart, position, 0, false);
       return PlaySoundEffectAt(SoundEffects.FireCrackle, position, 0, true);
    }
    public void StartFire(Vector3 position)
    {
        PlaySoundEffectAt(SoundEffects.FireStart, position, 0, false);
    }
    /*  private void Update()
      {
          if (Input.GetKeyDown(KeyCode.C))
          {
              CaveState();
          }
          if (Input.GetKeyDown(KeyCode.O))
          {
              OutdoorsState();
          }
      }*/
}
