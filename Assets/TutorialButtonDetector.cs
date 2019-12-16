using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonDetector : MonoBehaviour
{
    [SerializeField]
    private string buttonName;
    [SerializeField]
    private GameObject buttonGraphics;
    [SerializeField]
    private bool DisappearOncePlayerClickedInMyCollider = true;
    void Start()
    {
        buttonGraphics.SetActive(false);
    }

    /*void Update()
    {
        if (Input.GetButton(buttonName))
        {
            this.gameObject.SetActive(false);
        }
    }*/
    public void Disappear()
    {
        //this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            buttonGraphics.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (DisappearOncePlayerClickedInMyCollider)
        {
            if (other.tag == "Player" && Input.GetButtonDown(buttonName))
            {
                Disappear();
            }
        }

    }
}
