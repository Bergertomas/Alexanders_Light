using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool DidInteract = false;
    public UnityEvent OnInteract;
    public UnityEvent OnPlayerEnter;
    public virtual void Interact(/*PlayerController player*/)
    {
        OnInteract.Invoke();
    }
   /* private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() && Input.GetButtonDown("Interact"))
        {
            Interact(other.GetComponent<PlayerController>());
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter Interactabe");
        if (other.tag == "Player")
        {
            Debug.Log("other.tag == Player Interactabe");
            //Vector3 intractablePosition = new Vector3(this.transform.x, this.transform.position.y)
            //OnPlayerEnter.Invoke();
            //Vector3.MoveTowards(this.transform.position, other.attachedRigidbody.transform.position, 1f);
        }
    }
}
