using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkFire : MonoBehaviour
{
    [SerializeField]
   private ParticleSystem ps;
    [SerializeField]
    private GameObject scaler;
    [SerializeField]
    private AudioSource audioSource;
    private float minHearingDistance = 12f;
    private float loudHearingDistance = 4f;
    void Start()
    {
        if (scaler.GetComponent<MeshRenderer>())
        {
            scaler.GetComponent<MeshRenderer>().enabled = false;
        }

        UnityEngine.ParticleSystem.ShapeModule editableShape = ps.shape;
        editableShape.scale = scaler.transform.localScale;
    }

    private void Update()
    {
        float distanceFromAlex = Vector3.Distance(AlexanderController.Instance.transform.position, this.transform.position);
        if (distanceFromAlex < minHearingDistance)
        {
            if (ps.isPlaying)
            {
                audioSource.volume =
                    Mathf.Lerp(audioSource.volume,
                    (((distanceFromAlex - minHearingDistance) * 1) / (loudHearingDistance - minHearingDistance)),Time.deltaTime);


                MusicPlayer.Instance.MusicAudioSource.volume =// Utilities.MapRange(distanceFromAlex, loudHearingDistance, 0, 1);
                     Mathf.Lerp(MusicPlayer.Instance.MusicAudioSource.volume,
                Mathf.Abs(1 - ((distanceFromAlex - minHearingDistance) * 1) / (loudHearingDistance - minHearingDistance)) *
                 MusicPlayer.Instance.Tracks[MusicPlayer.Instance.CurrentTrackIndex].desiredVolume,Time.deltaTime);
            }
            else
            {
                audioSource.volume =
    Mathf.Lerp(audioSource.volume,
   0, Time.deltaTime);


                MusicPlayer.Instance.MusicAudioSource.volume =// Utilities.MapRange(distanceFromAlex, loudHearingDistance, 0, 1);
                     Mathf.Lerp(MusicPlayer.Instance.MusicAudioSource.volume,
                 MusicPlayer.Instance.Tracks[MusicPlayer.Instance.CurrentTrackIndex].desiredVolume, Time.deltaTime);
            }

        }
        else
        {
            audioSource.volume = 0;
        }
    }

}
