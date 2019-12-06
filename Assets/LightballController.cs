using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LightBallStates
{
    None, Charge, Amplify, Heal
}
public class LightballController : MonoBehaviour
{
    float light;
    bool isControlled;
    public bool IsPushed = false;
    [SerializeField]
    //BallPusherCollider pusherCollider;
    public LightBallStates State = LightBallStates.None;
    /*public bool isAmp;
    public bool isCharging;*/
    //bool isCalled;
    bool turretMode;
    //[SerializeField]
    //private LightManager lm;

    //public Transform targetSpot;
    public float ControlledSpeed = 6f;
    public float IdleMovementSpeed = 4f;
    public float HealMovementSpeed = 8f;
    [SerializeField]
    float overlapSphereRadius = 1.25f;
    [SerializeField]
    float DetectOverlappingObjectsInterval = 0.1666f;

    private void Start()
    {
        InvokeRepeating("DetectOverlappingObjects", 1f, DetectOverlappingObjectsInterval);
    }
    private void OnTriggerStay(Collider other)
    {
        // Debug.Log("OnTriggerStay()");
        if (State == LightBallStates.Charge)//We want to allow charging only when isCharging is true (which happens when PlayerController does (Input.GetButton("Charge"))
        {
            var c = other.gameObject.GetComponent<Charger>();
            if (c != null)
            {
                // Debug.Log("charger is being touched by me balls");
                LightManager.Instance.HandleCharge(c);//if a charger is touching the ball, send it to the light manager so it handles stuff
            }
        }
    }
    public void DetectOverlappingObjects()
    {
       // Collider[] collidersFound = Physics.OverlapBox(this.transform.position, new Vector3(0.7f, 0.7f, 0.7f));
        Collider[] collidersFound = Physics.OverlapSphere(this.transform.position, overlapSphereRadius);
        foreach (Collider C in collidersFound)
        {
            //if (C.GetComponent<MeshRenderer>() && C.tag != "BOL")
            if ( C.tag == "BOLPusher")
            {
                IsPushed = true;
                return;
            }
        }
        IsPushed = false;
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
}