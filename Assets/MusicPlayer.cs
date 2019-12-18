using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicTracks
{
    Main=0, ToTheLight=1
}

[System.Serializable]
public class MusicTrack 
{
    public MusicTracks TrackName;
    public AudioClip clip;
    public float desiredVolume;
}

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    public MusicTrack[] Tracks;
    public int CurrentTrackIndex = 0;
    [SerializeField]
    public AudioSource MusicAudioSource;
    private bool isFadingOut = false;
    private float fadeOutSpeed = 0.2f;
    [SerializeField]
    private SoundEffectObject collectSoundPrefab;
    public static MusicPlayer Instance;

    private void Awake()
    {
        Debug.Log("I am a public whore (-;");
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Tried to create another MusicPlayer.");
            Destroy(this);
        }
    }

    public void FadeOut()
    {
        isFadingOut = true;
    }

    public IEnumerator ChangeTrackAndPlayIn(MusicTracks track,float waitTime=0)
    {
       yield return new WaitForSeconds(waitTime);
       // bool trackFound = false;
        for (int i = 0; i < Tracks.Length; i++)
        {
            if (Tracks[i].TrackName == track)
            {
                isFadingOut = false;
                MusicAudioSource.volume = Tracks[i].desiredVolume;
                MusicAudioSource.clip = Tracks[i].clip;
                CurrentTrackIndex = i;
                //audioSource.Stop();
                MusicAudioSource.Play();
                //Invoke("PlayNow", waitTime);
                yield break;
            }
        }
        //if (!trackFound)
        {
            Debug.Log("Failed to find the track");
        }
    }

    public void ChangeTrackAndPlayNow(int track)
    {
        for (int i = 0; i < Tracks.Length; i++)
        {
            if (Tracks[i].TrackName ==(MusicTracks) track)
            {
                isFadingOut = false;
                MusicAudioSource.volume = Tracks[i].desiredVolume;
                MusicAudioSource.clip = Tracks[i].clip;
                CurrentTrackIndex = i;
                //audioSource.Stop();
                MusicAudioSource.Play();
                //Invoke("PlayNow", waitTime);
                 return;
            }
        }
        //if (!trackFound)
        {
            Debug.Log("Failed to find the track");
        }
    }

    /*private void PlayNow()
    {
        audioSource.Play();
    }*/

    void Update()
    {
        if (isFadingOut)
        {
            MusicAudioSource.volume -= fadeOutSpeed * Time.deltaTime;
        }
        else if(MusicAudioSource.volume< Tracks[CurrentTrackIndex].desiredVolume)
        {
            MusicAudioSource.volume +=   Time.deltaTime/7f;
        }
    }

    public void PlayCollectSound()
    {
      //  audioSource.Pause();
        Instantiate(collectSoundPrefab, Vector3.zero, Quaternion.identity);
   //     Invoke("ResumeMusic", 2f);

    }
   /* private void ResumeMusic()
    {
        audioSource.Play();
    }*/
}
