using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushInteractable : Interactable
{
    public override void Interact()
    {
        base.Interact();
    }

    void Update()
    {

    }
    /*void ApplyForce()
    {
        var rend = GetComponent<Renderer>();
        Rigidbody body = GetComponent<Rigidbody>();
        Debug.Log("Applied force with me balls");
        Vector3 direction = body.transform.position - transform.position;
        body.AddForceAtPosition(direction.normalized, new Vector3(rend.bounds.min.x, rend.bounds.max.y, 0f), ForceMode.Impulse);
    }*/
}
