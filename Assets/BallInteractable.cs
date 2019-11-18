using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*public struct BallInteractableState
{
    public bool isDead;
    public float life;
}*/
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BallInteractable : MonoBehaviour
{
    //public bool DidInteract = false;
    public UnityEvent WhenInteracting;
    public UnityEvent OnInteract;
    public UnityEvent OnInteractionExit;
    public UnityEvent OnDeath;
    public UnityEvent OnResurrection;

    public bool isDead = false;
    private bool interactedPreviousFrame = false;
    [SerializeField]
    private float life;
    [SerializeField]
    private float lifeAtStart;
    private bool beingInteractedWith;

    private bool isDeadOnLastCheckPoint;
    private float lifeOnLastCheckPoint;
    public virtual void Initialise()
    {
        isDead = false;
        life = lifeAtStart;
    }
    private void Start()
    {
        MasterController.Instance.CheckPointReached += RecordCurrentState;
        MasterController.Instance.RevertToPreviousCheckPoint += RevertToPreviousCheckPoint;
        Initialise();
    }
    public void RecordCurrentState(Transform t)
    {
        lifeOnLastCheckPoint = life;
        isDeadOnLastCheckPoint = isDead;
    }
    public virtual void RevertToPreviousCheckPoint()
    {
       life= lifeOnLastCheckPoint;
       isDead= isDeadOnLastCheckPoint;
        beingInteractedWith = false;
       if (isDead)
       {
           // Die();
       }
       else
       {
           OnResurrection.Invoke();
       }
    }
    public virtual void Interact()
    {
        if (life > 0)
        {
            Debug.Log("Interact Life>0");
            life -= LightManager.Instance.DecreaseLight();
            WhenInteracting.Invoke();
            if (interactedPreviousFrame == false)
            {
                interactedPreviousFrame = true;
                OnInteract.Invoke();
            }
        }
        else
        {
            Die();
        }
    }
    public virtual void Die()
    {
        OnDeath.Invoke();
        isDead = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isDead && other.GetComponent<LightballController>() && other.GetComponent<LightballController>().State==LightBallStates.Amplify)
        {
            Debug.Log("beingInteractedWith");
            beingInteractedWith = true;
        }
        else
        {
            beingInteractedWith = false;
        }
        /*else if (interactedPreviousFrame == true) {
            interactedPreviousFrame = false;
        }*/
    }
    private void Update()
    {
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
