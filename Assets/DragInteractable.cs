using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FixedJoint))]
public class DragInteractable : Interactable
{
    public Rigidbody RB;
    public FixedJoint FixedJoint;
    private void Start()
    {
        RB = this.GetComponent<Rigidbody>();
        FixedJoint = this.GetComponent<FixedJoint>();
    }
    /*public override void Interact(PlayerController player)
    {
        base.Interact(player);
        //player.Grab(this);
    }*/
}
