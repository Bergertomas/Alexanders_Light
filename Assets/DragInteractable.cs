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

    private Vector3 positionOnLastCheckPoint;
    private RigidbodyConstraints constraintsOnLastCheckPoint;
    private bool useGravityOnLastCheckPoint;
    private bool isKinematicOnLastCheckPoint;

    internal override void OnStart()
    {
        base.OnStart();
        RayCastOrigins.VerticalRayCount = 5;
        RayCastOrigins.HorizontalRayCount = 4;
        RB = this.GetComponent<Rigidbody>();
        Collider = this.GetComponent<Collider>();
        MoveToFreeState();
    }
    public override void RecordCurrentState(Transform t)
    {
        base.RecordCurrentState(t);
        positionOnLastCheckPoint = transform.position;
        constraintsOnLastCheckPoint = RB.constraints;
       isKinematicOnLastCheckPoint=  RB.isKinematic;
       useGravityOnLastCheckPoint= RB.useGravity;
    }
    public override void RevertToPreviousCheckPoint()
    {
        base.RevertToPreviousCheckPoint();
        transform.position = positionOnLastCheckPoint;
        RB.constraints = constraintsOnLastCheckPoint;
        RB.isKinematic = isKinematicOnLastCheckPoint;
        RB.useGravity = useGravityOnLastCheckPoint;
    }
    public void MoveToDraggedState()
    {
        RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        RB.isKinematic = true;
        RB.useGravity = false;
    }
    public void MoveToFreeState()
    {
        //RB.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;// | RigidbodyConstraints.FreezePositionX;
        RB.isKinematic = false;
        RB.useGravity = true;
    }
}
