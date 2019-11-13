using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
//[RequireComponent(typeof(FixedJoint))]
public class DragInteractable : Interactable
{
    public Rigidbody RB;
    public RayCastOrigins RayCastOrigins;
    public Collider Collider;
    private void Start()
    {
        RayCastOrigins.VerticalRayCount = 5;
        RayCastOrigins.VerticalRayCount = 4;
        RB = this.GetComponent<Rigidbody>();
        Collider= this.GetComponent<Collider>();
        MoveToFreeState();
    }
    public void MoveToDraggedState()
    {
        RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
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
