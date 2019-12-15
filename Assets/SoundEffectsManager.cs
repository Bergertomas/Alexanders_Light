using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffects
{
    Drag = 0, Jump = 1
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

    public void PlaySoundEffectAt(SoundEffects soundEffect, Vector3 position)
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
                soundObject.SoundEffectAudioSource.pitch += Random.Range(-0.2f, 0.2f);
                soundObject.SoundEffectAudioSource.Play();
                return;
            }
        }
        {
            Debug.Log("Failed to find the soundEffect");
        }
    }
}
