using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public void Collect()
    {
        MusicPlayer.Instance.PlayCollectSound();
        gameObject.SetActive(false);
    }
}
