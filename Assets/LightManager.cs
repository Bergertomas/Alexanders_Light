using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    [SerializeField]
    Slider lightGauge;
    [SerializeField]
    PlayerController pControl;
    [SerializeField]
    public LightballController bControl;
    [SerializeField]
    GameObject bGraphics;
    private float increasePerSecond = 10f;
    private float decreasePerSecond = 2.0f;
    [SerializeField]
    float maxCharge = 100f;
    [SerializeField]
    float minCharge = 0f;
    [SerializeField]
    float currentCharge = 50f;
    public float sphereRadius = 10f;
    private string chargeableTag = "Chargeable";
    [SerializeField]
    float chargePerTick = 4f;


   public void HandleCharge(Charger c) //Recieves a charger from LightBallController and manipulates the player's current charge and the charger's chrge
    {
        //Debug.Log("HandleCharge()");
        if (currentCharge < maxCharge)
        { 
             if (c.Charge > chargePerTick)
             {
                 c.Charge -= (chargePerTick * Time.deltaTime);
                 currentCharge += (chargePerTick * Time.deltaTime);
                //c.mat.SetColor("_EmissionColor", new Color(25.68894f, 18.1571f, 1.479468f, 1.0f) * 0);
             }
             else//if the charger's charge is less than what we take per tick, take everything it's got left
             {
                currentCharge += c.Charge;
                c.Charge = 0f;
             }
        }
   }

    // Start is called before the first frame update
    void Start()
    {
        lightGauge.minValue = minCharge;
        lightGauge.maxValue = maxCharge;
        lightGauge.value = currentCharge;
    }


    void IncreaseLight()
    {
        currentCharge += (increasePerSecond * Time.deltaTime);
        lightGauge.value = currentCharge;
    }


    public void DecreaseLight()
    {
        currentCharge -= (decreasePerSecond * Time.deltaTime);
        lightGauge.value = currentCharge;
    }


    void RecognizeChargable()
    {

    }


    ////void RecognizeChargable()
    //{
    //    RaycastHit hit;
    //    Vector3 BoLpos = bControl.transform.position;
    //    float distance = 1f;
    //    if (Physics.SphereCast(BoLpos, bControl.transform.position.x, transform.forward,
    //        out hit, 1))
    //    {
    //        Debug.Log("first stage");
    //        if (hit.collider.CompareTag("Chargable"))
    //        {
    //            Debug.Log("NOW");
    //        }
    //    }
    //    //yield return null;
    //}



    //void RecognizeChargable()
    //{
    //    if (Physics.CheckSphere(bControl.transform.position, sphereRadius) &&) {
    //        Debug.Log("YEP");
    //    }
    //    else
    //    {
    //        Debug.Log("Nope");
    //    }
    //}



    /*void RecognizeChargable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(bControl.transform.position, sphereRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "Chargable")
            {
                Debug.Log("HSDFS");
            }
        }
    }
    */

    private void FixedUpdate()
    {
        //RecognizeChargable();
    }

    // Update is called once per frame
    void Update()
    {
        lightGauge.value = currentCharge;
        //RecognizeChargable();
    }
}
