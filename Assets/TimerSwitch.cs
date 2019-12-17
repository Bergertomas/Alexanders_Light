using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerSwitch : BallInteractable
{
    [SerializeField]
    private float timeSpentInSwitchedState = 3f;
    [SerializeField]
    private float timeToReallowSwitch = 3f;
    private float switchedStateTimer=0;
    private float reallowSwitchTimer = 0;
    public UnityEvent OnTimesUp;
    public UnityEvent OnSwitchReallowed;

    protected override void RevertToPreviousCheckPoint()
    {
        base.RevertToPreviousCheckPoint();
        switchedStateTimer = 1f;// we wanna make sure things start in their normal state, the updaye will make sure of it if switchedStateTimer if larger than zero
    }
    protected override void Start()
    {
        base.Start();
        switchedStateTimer = 1f;// we wanna make sure things start in their normal state, the updaye will make sure of it if switchedStateTimer if larger than zero
    }
    protected override void Interact()
    {
        WhenInteracting.Invoke();
        if (reallowSwitchTimer <= 0)
        {
            if (interactedPreviousFrame == false)
            {
                interactedPreviousFrame = true;
                OnInteract.Invoke();
            }
            if (GetComponent<cakeslice.Outline>())
            {
                GetComponent<cakeslice.Outline>().enabled = false;
            }
            cakeslice.Outline[] outlines = GetComponentsInChildren<cakeslice.Outline>();
            for (int i = 0; i < outlines.Length; i++)
            {
                outlines[i].enabled = false;
            }
            reallowSwitchTimer = timeToReallowSwitch + timeSpentInSwitchedState;
            switchedStateTimer = timeSpentInSwitchedState;
        }

    }

    protected override void Update()
    {
        base.Update();
        if (switchedStateTimer > 0)
        {
            switchedStateTimer -= Time.deltaTime;
            if (switchedStateTimer <= 0)
            {
                OnTimesUp.Invoke();
            }
        }
        if (reallowSwitchTimer > 0)
        {
            reallowSwitchTimer -= Time.deltaTime;
            if (reallowSwitchTimer <= 0)
            {
                OnSwitchReallowed.Invoke();
                if (GetComponent<cakeslice.Outline>())
                {
                    GetComponent<cakeslice.Outline>().enabled = true;
                }
                cakeslice.Outline[] outlines = GetComponentsInChildren<cakeslice.Outline>();
                for (int i = 0; i < outlines.Length; i++)
                {
                    outlines[i].enabled = true;
                }
            }
        }
    }
}
