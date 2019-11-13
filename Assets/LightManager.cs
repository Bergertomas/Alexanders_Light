using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    [SerializeField]
    Slider lightGauge;
    [SerializeField]
    AlexanderController pControl;
    [SerializeField]
    public LightballController bControl;
    [SerializeField]
    GameObject bGraphics;
    [SerializeField]
    Light BoLAreaLight;
    [SerializeField]
    Light BoLHaloLight;
    [SerializeField]
    Material BoLMat;
    private float increasePerSecond = 10f;
    private float decreasePerSecond = 2.0f;
    [SerializeField]
    float maxCharge = 100f;
    [SerializeField]
    float minCharge = 0f;
    [SerializeField]
    private float currentCharge = 50f;
    public float CurrentCharge
    {
        get
        {
            return currentCharge;
        }
    }
    public float sphereRadius = 10f;
    private string chargeableTag = "Chargeable";
    [SerializeField]
    float chargePerTick = 4f;
    [SerializeField]
    float maxEmission = 5f;
    [SerializeField]
    float minEmission = 1f;
    [SerializeField]
    float maxALIntensity = 23f;
    [SerializeField]
    float minALIntensity = 5f;
    [SerializeField]
    float defaultAmp = 1.5f;
    [SerializeField]
    float maxAmp = 4f;
    [SerializeField]
    float steps = 2f;
    [SerializeField]
    float emissionLerpSteps = 5f;
    [SerializeField]
    float defaultHaloLightIntensity = 1f;
    [SerializeField]
    float reduceChargeWhenAmped = 3f;


    public static LightManager Instance;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Tried to create another world.");
            Destroy(this);
        }
    }

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

    void Amplify()
    {
        if (bControl.State==LightBallStates.Amplify)
        {
            //DecreaseLight();
            if (BoLHaloLight.range < maxAmp-0.01f && currentCharge > 0)
            {
                //BoLHaloLight.range += (steps * Time.deltaTime);
                //BoLHaloLight.intensity += (steps * Time.deltaTime);
                BoLHaloLight.range = Mathf.Lerp(BoLHaloLight.range, maxAmp, steps*Time.deltaTime);
                BoLHaloLight.intensity = Mathf.Lerp(BoLHaloLight.intensity, maxAmp, steps * Time.deltaTime);
               // Color newColour = new Color();
                float newEmission = Mathf.Lerp(BoLMat.GetColor("_EmissionColor").r, 20f, steps * Time.deltaTime); 
                BoLMat.SetColor("_EmissionColor", new Color(newEmission, newEmission, newEmission, 1.0f));
            }
        }
        else// if (bControl.isAmp == false)
        {
            if (BoLHaloLight.range > defaultAmp+0.01f)
            {
                BoLHaloLight.range = Mathf.Lerp(BoLHaloLight.range, defaultAmp, steps * Time.deltaTime);
                //BoLHaloLight.intensity = Mathf.Lerp(BoLHaloLight.intensity, defaultAmp, steps * Time.deltaTime);
                //BoLMat.SetColor("_EmissionColor", new Color(0f, 0f, 0f, 1.0f));
                //BoLHaloLight.range -= (steps * Time.deltaTime);
                // BoLHaloLight.intensity -= (steps * Time.deltaTime);

            }
        }
    }

    //TODO: Block ability to amplify if not enough charge;
    //TODO: Amplifying should discharge BoL.
    void Update()
    {
        lightGauge.value = currentCharge;
        //float emissionIntensity = ((currentCharge / maxCharge) * (maxEmission - minEmission)) + minEmission;
        if (bControl.State != LightBallStates.Amplify)
        {
            float emissionIntensity = Utilities.MapRange(currentCharge, maxCharge, minEmission, maxEmission);
            float newEmission = Mathf.Lerp(BoLMat.GetColor("_EmissionColor").r, emissionIntensity, emissionLerpSteps * Time.deltaTime);
            BoLMat.SetColor("_EmissionColor", new Color(newEmission, newEmission, newEmission, 1.0f));
            //BoLAreaLight.intensity = ((currentCharge / maxCharge) * (maxALIntensity - minALIntensity)) + minALIntensity;
            float intensity =  Utilities.MapRange(currentCharge, maxCharge, minALIntensity, maxALIntensity);
            float newIntensity = Mathf.Lerp(BoLAreaLight.intensity, intensity, emissionLerpSteps * Time.deltaTime);
            BoLAreaLight.intensity = newIntensity;
            BoLHaloLight.intensity = Mathf.Lerp(BoLHaloLight.intensity, defaultHaloLightIntensity, emissionLerpSteps * Time.deltaTime);
        }

        // lum.intensity = (maxIntensity * (Charge / maxCharge));
        //RecognizeChargable();
        Amplify();
    }

    void Start()
    {
        currentCharge = 50f;
        lightGauge.minValue = minCharge;
        lightGauge.maxValue = maxCharge;
        lightGauge.value = currentCharge;
    }

    void IncreaseLight()
    {
        currentCharge += (increasePerSecond * Time.deltaTime);
        lightGauge.value = currentCharge;
    }

    public float DecreaseLight()
    {
        currentCharge -= (decreasePerSecond * Time.deltaTime);
        lightGauge.value = currentCharge;
        return (decreasePerSecond * Time.deltaTime);
    }
}


