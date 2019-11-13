using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
//[RequireComponent(typeof(FixedJoint))]
public class DragInteractable : Interactable
{
    public Rigidbody RB;
    private void Start()
    {
        RB = this.GetComponent<Rigidbody>();
        MoveToFreeState();
    }
    public void MoveToDraggedState()
    {
        //RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        RB.isKinematic = true;
        RB.useGravity = false;
    }
    public void MoveToFreeState()
    {
        RB.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        //RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;// | RigidbodyConstraints.FreezePositionX;
        RB.isKinematic = false;
        RB.useGravity = true;
    }
}
