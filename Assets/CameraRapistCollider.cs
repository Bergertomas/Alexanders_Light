using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRapistCollider : MonoBehaviour
{
    private CameraRapist owner;
    void Start()
    {
        this.owner = GetComponentInParent<CameraRapist>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rapist penetrated!");
        if (other.gameObject.tag == "Player")
        {
            CameraController.Instance.AddRapist(owner);      
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Rapist receded!");
        if (other.gameObject.tag == "Player")
        {
            CameraController.Instance.RemoveRapist(owner);
            //CameraController.Instance.LowerSpeed();
        }
    }
}
