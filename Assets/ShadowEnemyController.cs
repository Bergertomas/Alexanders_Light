using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEnemyController : MonoBehaviour
{

    [SerializeField]
    float lookoutRadius = 3f;
    private GameObject objectToFollow;
    [SerializeField]
    float moveSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        LookForPlayer();
    }


    void LookForPlayer()
    {
        Invoke("LookForPlayer", 1f);
        Collider[] objectsInRange = Physics.OverlapSphere(this.transform.position, lookoutRadius);
        foreach (Collider c in objectsInRange)
        {
            if(c.tag == "Player")
            {
                //dostuff
                objectToFollow = c.gameObject;
                return;
            }
        }
        objectToFollow = null;
    }


    void FixedUpdate()
    {
        if (objectToFollow != null)
        { 

            transform.LookAt(new Vector3(objectToFollow.transform.position.x, this.transform.position.y, 0f));
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
