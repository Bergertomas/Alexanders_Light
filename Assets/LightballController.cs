using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightballController : MonoBehaviour
{

    public float ballspeed = 8f;
    float light;
    bool isControlled;
    bool isAmp;
    bool isCharging;
    bool isCalled;
    bool turretMode;


    [SerializeField]
    public Transform targetSpot;
    [SerializeField] public float moveSpeed;



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

