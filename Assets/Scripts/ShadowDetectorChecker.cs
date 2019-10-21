using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShadowDetectorChecker : MonoBehaviour
{
    /// <summary>
    /// This script gets info about the brightness from ShadowDetector and sends it to the player (prints info such as hidden/seen, and light levels).
    /// We may want to ditch this script since it only controls various obsolete UI elements from the shadow demo. 
    /// The responsibility of getting the light levels from ShadowDetector should be given to GameManager(?)
    /// </summary>
    /// 
    public Text sensorBr;
    public Transform eyeEnabled;
    public Transform eyeDisabled;
    private ShadowDetector sd;
    private Image img;

    void Start()
    {
        sd = transform.GetComponent<ShadowDetector>();
        img = eyeEnabled.GetComponent<Image>();
    }

    void Update()
    {
        if (sd.hidden)
        {
            eyeEnabled.gameObject.SetActive(false);
            eyeDisabled.gameObject.SetActive(true);
        }
        else
        {
            eyeEnabled.gameObject.SetActive(true);
            eyeDisabled.gameObject.SetActive(false);
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, sd.bright / sd.maxShadowBright);
        sensorBr.text = "Sensor Bright: " + System.Math.Round(sd.bright, 2);
    }

}
