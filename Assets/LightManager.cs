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
    private float increasePerSecond = 10f;
    private float decreasePerSecond = 0.5f;
    [SerializeField]
    float maxCharge = 100f;
    [SerializeField]
    float minCharge = 0f;
    [SerializeField]
    float currentCharge = 50f;
    public float sphereRadius = 100f;



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


    void DecreaseLight()
    {
        currentCharge -= (decreasePerSecond * Time.deltaTime);
        lightGauge.value = currentCharge;
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



    void RecognizeChargable()
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


    private void FixedUpdate()
    {
        //RecognizeChargable();
    }

    // Update is called once per frame
    void Update()
    {
        RecognizeChargable();
    }
}
