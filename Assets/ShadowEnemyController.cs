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
    //[SerializeField]
    //float life = 40f;
    [SerializeField]
    float minMoveX, origMinMove = 1f;
    [SerializeField]
    float maxMoveX, origMaxMove = 5f;
    int direction = -1;
    private Vector3 initialPosition;
    //public Rigidbody rigidbody;
    private string bolPusher = "BOLPusher";
    private string wall = "Wall";
    private string physicalObject = "PhysicalObject";



    // Start is called before the first frame update
    void Start()
    {
        // get it's own rigidbody component
        //rigidbody = this.GetComponent<Rigidbody>();
        // store the initial position we started from so that we know to patrol around that point and no further
        initialPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        Debug.Log(initialPosition);
        // set perimiter values for the enemy
        //float origMinMove = minMoveX;
        //float origMaxMove = maxMoveX;
        minMoveX = initialPosition.x - minMoveX;
        maxMoveX = initialPosition.x + maxMoveX;
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



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == bolPusher
            || collision.gameObject.tag == wall || collision.gameObject.tag == physicalObject)
        {
            direction = -direction;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BOL")
        {
            Debug.Log("THISISIT");
            // need to insert another if that will make bol stop the enemy only if is amped 
        }
    }


    private void Move()
    { 
        if (direction == -1)
        {
            // moving left
            if (this.transform.position.x > minMoveX)
            {
                this.transform.position = new Vector3(this.transform.position.x - (moveSpeed * Time.deltaTime), this.transform.position.y);
                //this.transform.position = new Vector3(minMoveX, transform.position.y) * Time.deltaTime;
            }
            else
            {
                direction = -direction;
            }
        }
        else if (direction == 1)
        {
            // moving right
            
            if(this.transform.position.x < maxMoveX)
            {
                this.transform.position = new Vector3(this.transform.position.x + (moveSpeed * Time.deltaTime), this.transform.position.y);
                //this.rigidbody.MovePosition(new Vector3(maxMoveX, rigidbody.transform.position.y));
                //this.transform.position = new Vector3(maxMoveX, transform.position.y) * Time.deltaTime;
            }
            else
            {
                direction = -direction;
            }
        } 
    }


    void FixedUpdate()
    {
        if (objectToFollow != null)
        { 

            transform.LookAt(new Vector3(objectToFollow.transform.position.x, transform.position.y, 0f));
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            Invoke("Move", 1);
        }
    }
}
