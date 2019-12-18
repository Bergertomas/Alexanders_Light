using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    [SerializeField]
    public Image Image;
    private bool fadeToBlack = false;
    [SerializeField]
    private float fadeSpeed=0.1f;
    public bool IBelongToEndLight = false;
    void Start()
    {
        // image.color = Color.black;
        Image.color = new Color(0, 0, 0, 0);
        MasterController.Instance.RevertToPreviousCheckPoint += delegate () { fadeToBlack = false; };
        MasterController.Instance.GameOverEvent += delegate () { fadeToBlack = true; };
    }

    void Update()
    {
        if(fadeToBlack&& Image.color.a < 1)
        {
            Image.color = new Color(0, 0, 0, Image.color.a+(fadeSpeed*Time.deltaTime));
        }
        else if (!fadeToBlack && Image.color.a > 0&& !IBelongToEndLight)
        {
            Image.color = new Color(0, 0, 0, Image.color.a - (fadeSpeed * Time.deltaTime));
        }
    }

    public void FadeTo(bool black)
    {
        fadeToBlack = black;
    }
}
