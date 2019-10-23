using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool DidInteract = false;
    public UnityEvent OnInteract;
    public virtual void Interact()
    {
        OnInteract.Invoke();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            Debug.Log("OnTriggerStayInteract");
            Interact();
        }
    }
}
