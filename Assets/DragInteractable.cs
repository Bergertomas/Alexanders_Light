using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DragInteractable : Interactable
{
    public Rigidbody rigidbody;
    private void Start()
    {
        rigidbody=this.GetComponent<Rigidbody>();
    }
    public override void Interact(PlayerController player)
    {
        base.Interact(player);
        player.Grab(this);
    }
}
