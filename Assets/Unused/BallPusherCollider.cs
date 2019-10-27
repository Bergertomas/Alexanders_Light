using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPusherCollider : MonoBehaviour
{
    public bool IsPushed = false;
    public Rigidbody RigidBody;
    public bool DetectOverlappingObjects()
    {
        Collider[] collidersFound = Physics.OverlapBox(this.transform.position, new Vector3(1, 1, 2));
        foreach(Collider C in collidersFound)
        {
            if (C.GetComponent<MeshRenderer>() && C.tag != "BOL")
            {
                return true;
            }
        }

            return false;
    }


    private void OnTriggerEnter(Collider other)
    {
         Debug.Log("BallPusherCollider Enter");
        
            if (other.GetComponent<MeshRenderer>())
            {
                IsPushed = true;
            }
    }
    private void OnTriggerExit(Collider other)
     {
         Debug.Log("BallPusherCollider Exit");
         if (other.GetComponent<MeshRenderer>())
         {
             IsPushed = false;
         }
     }
    /*private void OnTriggerStay(Collider other)
    {
        IsPushed = (other.GetComponent<MeshRenderer>());
    }*/

}
