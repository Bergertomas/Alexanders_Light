using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//public delegate Transform CheckPointDelegate(Transform transform);
public delegate void CheckPointDelegate(Transform transform);
public class CheckPoint : KillableBallInteractable
{
    public event CheckPointDelegate CheckPointReached;
    public Transform CheckPointTransform;
    protected override void Die()
    {
        base.Die();
        CheckPointReached.Invoke(CheckPointTransform!=null?CheckPointTransform:transform);
    }
    protected override void RevertToPreviousCheckPoint()
    {
        Debug.Log("CheckPoints do not revert");
    }

}