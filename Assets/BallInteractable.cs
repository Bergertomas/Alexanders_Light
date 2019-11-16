﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BallInteractable : MonoBehaviour
{
    //public bool DidInteract = false;
    public UnityEvent WhenInteracting;
    public UnityEvent OnInteract;
    public UnityEvent OnInteractionExit;
    public UnityEvent OnDeath;

    public bool isDead = false;
    private bool interactedPreviousFrame = false;
    [SerializeField]
    private float life;
    private bool beingInteractedWith;

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
