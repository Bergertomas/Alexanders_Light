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
    private MusicTrack[] tracks;
    [SerializeField]
    private AudioSource audioSource;
    private bool isFadingOut = false;
    private float fadeOutSpeed = 0.2f;

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
        for (int i = 0; i < tracks.Length; i++)
        {
            if (tracks[i].TrackName == track)
            {
                isFadingOut = false;
                audioSource.volume = tracks[i].desiredVolume;
                audioSource.clip = tracks[i].clip;
                //audioSource.Stop();
                audioSource.Play();
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
        for (int i = 0; i < tracks.Length; i++)
        {
            if (tracks[i].TrackName ==(MusicTracks) track)
            {
                isFadingOut = false;
                audioSource.volume = tracks[i].desiredVolume;
                audioSource.clip = tracks[i].clip;
                //audioSource.Stop();
                audioSource.Play();
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
            audioSource.volume -= fadeOutSpeed * Time.deltaTime;
        }
    }
}
