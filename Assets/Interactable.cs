using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool DidInteract = false;
    public UnityEvent OnInteract;
    public Texture2D buttonImage;
    private bool nearInteractible = false;
    public virtual void Interact()
    {
        OnInteract.Invoke();
    }

    private void OnGui()
    {
        if (nearInteractible == true)
        {
            GUI.Label(new Rect(1f, 1f, buttonImage.width / 3f, buttonImage.height / 3f), buttonImage);
        }
    }
        
    
    private void OnTriggerStay(Collider other)
    {
        if (DidInteract == false)
        {
            nearInteractible = true;
        }
        else if (DidInteract == true)
        {
            nearInteractible = false;
        }
        nearInteractible = true;
        if (other.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            Debug.Log("OnTriggerStayInteract");
            Interact();
        }
    }

    

}
