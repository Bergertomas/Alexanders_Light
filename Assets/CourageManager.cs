using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourageManager : MonoBehaviour
{

    [SerializeField]
    Slider courageGauge;
    [SerializeField]
    PlayerController pControl;
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
        courageGauge.minValue = minHealth;
        courageGauge.maxValue = maxHealth;
        //courageGauge.value = courageGauge.maxValue;
        courageGauge.value = currentHealth;
        //StartCoroutine(IncreaseCourage());
    }


    void IncreaseCourage()
    {
        currentHealth += (increasePerSecond * Time.deltaTime);
        //courageGauge.value += (increasePerSecond * Time.deltaTime);
        courageGauge.value = currentHealth;
        Debug.Log(courageGauge.value);
        return;
    }


    void DecreaseCourage()
    {
        // TODO: GetFear should raycast sphere (in update) and find whether the player is around an enemy / spooky shadows / in darkness.
        // TODO: Find out how to use events for this shit
    }


    // TODO: Understand why health keeps on increasing after hasCalledBoL turns False.
    private void FixedUpdate()
    {
        if (pControl.hasCalledBoL == true)
        {
            Invoke("IncreaseCourage", waitUntilBallArrives);
        }
    }





    // Update is called once per frame
    void Update()
    {

    }

}
