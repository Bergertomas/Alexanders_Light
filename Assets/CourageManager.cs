using System.Collections;
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
    PlayerController pControl;
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



    // Start is called before the first frame update
    void Start()
    {
        lm = GetComponent<LightManager>();
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


    void DecreaseCourage()
    {
        currentHealth -= (increasePerSecond * Time.deltaTime);
        courageGauge.value = currentHealth;
        Debug.Log(courageGauge.value);
        return;
        // TODO: GetFear should raycast sphere (in update) and find whether the player is around an enemy / spooky shadows / in darkness.
        // TODO: Do we need events for this? if so, figure it out
    }

    private void FixedUpdate()
    {
        //If the player summoned the ball to his side to heal courage, do it
        if (pControl.hasCalledBoL == true)
        {
            Invoke("IncreaseCourage", waitUntilBallArrives);
        }

        //If the player is in enough darkness, start decreasing courage
        if (sd.hidden)
        {
            DecreaseCourage();
        }
    }





    // Update is called once per frame
    void Update()
    {
        //If the player is in enough darkness
        if (sd.hidden)
        {

        }
    }

}
