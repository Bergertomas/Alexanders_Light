using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BallInteractable : MonoBehaviour
{
    public UnityEvent WhenInteracting;
    public UnityEvent OnInteract;
    public UnityEvent OnInteractionExit;

    private int triggerStayCounter = 0;
    private bool interactedPreviousFrame = false;
    protected bool beingInteractedWith;

    protected virtual void Initialise(){ }

    protected virtual void Start()
    {
        gameObject.layer = 11;//BOLInteractable is 11..
        MasterController.Instance.CheckPointReached += RecordCurrentState;
        MasterController.Instance.RevertToPreviousCheckPoint += RevertToPreviousCheckPoint;
        Initialise();
    }

    protected virtual void RecordCurrentState(Transform t) { }

    protected virtual void RevertToPreviousCheckPoint()
    {
        beingInteractedWith = false;
    }

    protected virtual void Interact()
    {
      /*  if (life > 0)
        {
            Debug.Log("Interact Life>0");
            life -= LightManager.Instance.DecreaseLight();*/
            WhenInteracting.Invoke();
            if (interactedPreviousFrame == false)
            {
                interactedPreviousFrame = true;
                OnInteract.Invoke();
            }
            /*
        }
        else
        {
            Die();
        }*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (/*!isDead && */other.GetComponent<LightballController>() && other.GetComponent<LightballController>().State==LightBallStates.Amplify)
        {
            Debug.Log("beingInteractedWith");
            if (triggerStayCounter < 10)//Update vs FixedUpdate thingy...
            {
                triggerStayCounter+=3;
            }
            //beingInteractedWith = true;
        }
        /*else
        {
            beingInteractedWith = false;
        }*/
        /*else if (interactedPreviousFrame == true) {
            interactedPreviousFrame = false;
        }*/
    }
   /* private void LateUpdate()
    {
        beingInteractedWith = false;
    }*/
   
    protected virtual void Update()
    {
        if (triggerStayCounter > 0)//Update vs FixedUpdate thingy...
        {
            triggerStayCounter--;
        }
        beingInteractedWith = (triggerStayCounter > 0);
        if (beingInteractedWith)
        {
            Interact();
        }
        else if(interactedPreviousFrame)
        {
            OnInteractionExit.Invoke();
            interactedPreviousFrame = false;
        }
    }

}
