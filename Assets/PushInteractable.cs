﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushInteractable : Interactable
{

    public override void Interact()
    {
        base.Interact();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
