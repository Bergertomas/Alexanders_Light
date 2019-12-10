using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private bool fadeToBlack = false;
    [SerializeField]
    private float fadeSpeed=0.1f;

    void Start()
    {
        // image.color = Color.black;
        image.color = new Color(0, 0, 0, 0);
        MasterController.Instance.RevertToPreviousCheckPoint += delegate () { fadeToBlack = false; };
        MasterController.Instance.GameOverEvent += delegate () { fadeToBlack = true; };
    }

    void Update()
    {
        if(fadeToBlack&& image.color.a < 1)
        {
            image.color = new Color(0, 0, 0, image.color.a+(fadeSpeed*Time.deltaTime));
        }
        else if (!fadeToBlack && image.color.a > 0)
        {
            image.color = new Color(0, 0, 0, image.color.a - (fadeSpeed * Time.deltaTime));
        }
    }

   /* public void FadeTo(bool black)
    {
        fadeToBlack = black;
    }*/
}
