using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    //public bool DidInteract = false;
    public UnityEvent OnInteract;
    public UnityEvent OnPlayerEnter;

    private void Start()
    {
        OnStart();
    }
    internal virtual void OnStart(/*PlayerController player*/)
    {
        MasterController.Instance.CheckPointReached += RecordCurrentState;
        MasterController.Instance.RevertToPreviousCheckPoint += RevertToPreviousCheckPoint;
    }
    public virtual void Interact(/*PlayerController player*/)
    {
        OnInteract.Invoke();
    }
    public virtual void RecordCurrentState(Transform t)
    {
        Debug.Log("Interactable RecordCurrentState");
    }
    public virtual void RevertToPreviousCheckPoint()
    {
        Debug.Log("Interactable RevertToPreviousCheckPoint");

        /*if (isDead)
        {
            // Die();
        }
        else
        {
            OnResurrection.Invoke();
        }*/
    }

    /* private void OnTriggerStay(Collider other)
     {
         if (other.GetComponent<PlayerController>() && Input.GetButtonDown("Interact"))
         {
             Interact(other.GetComponent<PlayerController>());
         }
     }*/
    /* private void OnTriggerEnter(Collider other)
     {
         Debug.Log("OnTriggerEnter Interactabe");
         if (other.tag == "Player")
         {
             Debug.Log("other.tag == Player Interactabe");
             //Vector3 intractablePosition = new Vector3(this.transform.x, this.transform.position.y)
             //OnPlayerEnter.Invoke();
             //Vector3.MoveTowards(this.transform.position, other.attachedRigidbody.transform.position, 1f);
         }
     }*/
}
