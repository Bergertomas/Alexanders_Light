using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightballController : MonoBehaviour
{

    public float ballspeed = 8f;
    float light;
    bool isControlled;
    bool isAmp;
    public bool isCharging;
    bool isCalled;
    bool turretMode;
    [SerializeField]
    private LightManager lm;


    [SerializeField]
    public Transform targetSpot;
    [SerializeField] public float moveSpeed;


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay()");
        if (isCharging==true)//We want to allow charging only when isCharging is true (which happens when PlayerController does (Input.GetButton("Charge"))
        {
            var c = other.gameObject.GetComponent<Charger>();
            if (c != null)
            {
               // Debug.Log("charger is being touched by me balls");
                lm.HandleCharge(c);//if a charger is touching the ball, send it to the light manager so it handles stuff
            }
        }
    }
    //void RecognizeChargable()
    //{
    //    //var forward = transform.TransformDirection(Vector3.forward);
    //    //RaycastHit hit;
    //    //Vector3 BoLpos = this.transform.position;
    //    //float distance = 1f;
    //    //if (Physics.Raycast(BoLpos, forward, distance, hit) && hit.transform.tag == "Chargable")
    //    //{
    //    //    Debug.Log("first stage");
    //    //    if (hit.collider.CompareTag("Chargable"))
    //    //    {
    //    //        Debug.Log("NOW");
    //    //    }
    //    }
    //    //yield return null;
    //}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //
    }
}

