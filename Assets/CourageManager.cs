using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourageManager : MonoBehaviour
{
    [SerializeField]
    ShadowDetector sd;
    [SerializeField]
    Slider courageGauge;
    [SerializeField]
    AlexanderController pControl;
    [SerializeField]
    LightManager lm;
    private float increasePerSecond = 7f;
    [SerializeField]
    float maxHealth = 100f;
    [SerializeField]
    float minHealth = 0f;
    [SerializeField]
    float currentHealth = 50f;
    [SerializeField]
    float waitUntilBallArrives = 0.5f;
    [SerializeField]
    float secondsToWaitBeforeDarkness = 2f;
    private float timer = 0f;
    private float timerMax = 0f;
    private bool hasWaited = false;

    public event Action CourageDepleted;

    // Start is called before the first frame update
    void Start()
    {
        lm = GetComponent<LightManager>();
        pControl = FindObjectOfType<AlexanderController>();
        sd = FindObjectOfType<ShadowDetector>();
        courageGauge.minValue = minHealth;
        courageGauge.maxValue = maxHealth;
        //courageGauge.value = courageGauge.maxValue;
        courageGauge.value = currentHealth;
        //StartCoroutine(IncreaseCourage());
    }

    void IncreaseCourage()
    {
        if (lm.CurrentCharge > 0f)
        {
            currentHealth += (increasePerSecond * Time.deltaTime);
            courageGauge.value = currentHealth;
            lm.DecreaseLight();
            Debug.Log(courageGauge.value);
            return;
        }
    }

    /*public void CourageDepleted()
    {

    }*/
    void DecreaseCourage()
    {
        currentHealth -= (increasePerSecond * Time.deltaTime);
        courageGauge.value = currentHealth;
        //Debug.Log(courageGauge.value);
        if (currentHealth <= 0f)
        {
            CourageDepleted();
        }
    }

    
    private bool Waited(float seconds)
    {
        timerMax = seconds;
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            timerMax = 0f;
            timer = 0f;
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        //If the player summoned the ball to his side to heal courage, do it
        if (pControl.hasCalledBoL == true)
        {
            Invoke("IncreaseCourage", waitUntilBallArrives);
        }

        //If the player is in enough darkness, start decreasing courage
        //if (sd.hidden)
        //{
        //    if (hasWaited == false)
        //    {
        //        hasWaited = Waited(secondsToWaitBeforeDarkness);
        //    }
        //    else if (hasWaited == true)
        //    {
        //        DecreaseCourage();
        //    }
        //    //DecreaseCourage();
        //}

        //else if (!sd.hidden)
        //{
        //    hasWaited = false;
        //}

        if (pControl.isDarknened)
        {
            DecreaseCourage();
        }
    }
        
}


    // Update is called once per frame
//    void Update()
//    {
//        //If the player is in enough darkness
//        if (sd.hidden)
//        {

//        }
//    }

//}
