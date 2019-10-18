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

    // Start is called before the first frame update
    void Start()
    {
        courageGauge.minValue = 0;
        courageGauge.maxValue = 100;
        //courageGauge.value = courageGauge.maxValue;
        courageGauge.value = 50;
        //StartCoroutine(IncreaseCourage());
    }


    IEnumerator IncreaseCourage()
    {
        //while (pControl.hasCalledBoL == true)
        //{
        if (courageGauge.value < courageGauge.maxValue)
        {
            courageGauge.value += 1;
            Debug.Log(courageGauge.value);
            yield return new WaitForSeconds(1);
        }
        //else yield return null;
    }
    //}


    void DecreaseCourage()
    {
        // TODO: GetFear should raycast sphere (in update) and find whether the player is around an enemy / spooky shadows / in darkness.
        // TODO: Find out how to use events for this shit
    }



    private void FixedUpdate()
    {
        if (pControl.hasCalledBoL == true)
        {
            IncreaseCourage();
        }
    }





    // Update is called once per frame
    void Update()
    {

    }

}