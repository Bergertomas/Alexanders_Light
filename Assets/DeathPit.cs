using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeathPit : MonoBehaviour
{
    public event Action Pitfall;
    void Start()
    {
        if (GetComponent<MeshRenderer>())
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pitfall");
        if (other.gameObject.tag == "Player")
        {
            Pitfall.Invoke();
        }
    }
}
