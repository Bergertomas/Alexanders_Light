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

