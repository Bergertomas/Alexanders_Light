using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventTriggerObject : MonoBehaviour
{
    public UnityEvent OnTriggerEntered;
    [SerializeField]
    float timeToWait = 2f;
    public UnityEvent PostWaitOnTriggerEntered;
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AlexanderController>()&&!triggered)
        {
            triggered = true;
            OnTriggerEntered.Invoke();
            Invoke("PostWaitOnTriggerEnter", timeToWait);
        }
    }
    private void PostWaitOnTriggerEnter()
    {
        PostWaitOnTriggerEntered.Invoke();
    }
}
