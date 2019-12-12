using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class SemiPhysicalObject : MonoBehaviour
{
    private Rigidbody rb;
    //private Transform transformAtPreviousCheckPoint;
    private Vector3 positionAtPreviousCheckPoint;
    private Quaternion rotationAtPreviousCheckPoint;
    private bool isKinematicAtPreviousCheckPoint;
    //public UnityEvent OnStart

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MasterController.Instance.CheckPointReached += RecordCurrentState;
        MasterController.Instance.RevertToPreviousCheckPoint += RevertToPreviousCheckPoint;
    }

    public void DoPhysics(bool simulated)
    {
        rb.isKinematic = simulated;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kinemator")
        {
            Debug.Log("SemiPhysicalObject entered a Kinemator");
            Invoke("KinematorMet", 0.2f);
        }
    }

    private void KinematorMet()
    {
        DoPhysics(true);
    }
     public void RecordCurrentState(Transform checkPointTransform)
     {
        positionAtPreviousCheckPoint = transform.position;
        rotationAtPreviousCheckPoint = transform.rotation;
        isKinematicAtPreviousCheckPoint = rb.isKinematic;
     }
     public void RevertToPreviousCheckPoint()
     {
        Debug.Log("physical RevertToPreviousCheckPoint");
        transform.position = positionAtPreviousCheckPoint;
        transform.rotation = rotationAtPreviousCheckPoint;
        rb.isKinematic = isKinematicAtPreviousCheckPoint;
    }
}
