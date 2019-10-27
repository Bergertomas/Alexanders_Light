using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Charger : MonoBehaviour
{
    public float Charge;
    [SerializeField]
    private Light lum;
    private float maxIntensity;
    private float maxCharge;
    public UnityEvent OnChargeDepleted;

    //public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        maxCharge = Charge;
        maxIntensity=lum.intensity;
      //  mat = this.GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        lum.intensity = (maxIntensity * (Charge/maxCharge));
        if (Charge <= 0)
        {
            OnChargeDepleted.Invoke();
        }
    }
}
