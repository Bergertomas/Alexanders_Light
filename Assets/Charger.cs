using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    public float Charge;
    [SerializeField]
    private Light lum;
    private float maxIntensity;
    private float maxCharge;

    //public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        maxCharge = Charge;
        maxIntensity=lum.intensity;
      //  mat = this.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        lum.intensity = (maxIntensity * (Charge/maxCharge));
    }
}
