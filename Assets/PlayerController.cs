using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidbody;
    [SerializeField]
    GameObject physicalCollider;
    [SerializeField]
    float speed = 3.0f;
    float crawlSpeed;
    float climbSpeed;
    bool isGrounded;
    bool crawling;
    float courage;
    bool isAlive;
    public bool hasCalledBoL;
    [SerializeField]
    float jumpHeight = 2f;
    [SerializeField]
    float artificialGravity = -0.666f;

    public LightballController lbc;
    public GameObject ballAnchor;
    private float ballAnchorZ;
    public Transform balloflight;

    // TODO: SET UP CAMERA SCRIPT WHICH WILL MAKE THE CAMERA ZOOM A LITTLE OUT WHILE BOL IS FAR AWAY (ORI)


    // Start is called before the first frame update
    void Start()
    {
        //ChangeAnchorPosition();

        //Physics.IgnoreCollision(balloflight.GetComponent<Collider>(), GetComponent<Collider>(),true);
    }
    private void ChangeAnchorPosition()
    {
        return;
       //ballAnchor.transform.position = Random.insideUnitSphere * 2;
        Vector3 newPos = new Vector3(Random.Range(-3f, 3f), Random.Range(0f, 2f), 0f);
        ballAnchor.transform.localPosition = /*this.transform.position+*/newPos;
        Invoke("ChangeAnchorPosition", 5.5f);
       //ballAnchor.transform.position = new Vector3(ballAnchor.transform.position.x, ballAnchor.transform.position.y,ballAnchorZ);
    }
    void Update()
    {
        // Movement
        var x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        var move = new Vector3(x, 0);
        transform.Translate(move);

        if(isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rigidbody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
                isGrounded = false;
            }
        }

        var bx = Input.GetAxis("BallHorizontal") * lbc.ballspeed * Time.deltaTime;
        var by = Input.GetAxis("BallVertical") * lbc.ballspeed * Time.deltaTime;
        var bmove = new Vector2(bx, by);

        if (Input.GetAxisRaw("BallHorizontal") == 0.0f && Input.GetAxisRaw("BallVertical") == 0.0f && !Input.GetButton("Heal"))
        {
            if(!lbc.isCharging)
            {
                lbc.transform.position = Vector2.Lerp(lbc.transform.position, lbc.targetSpot.position, Time.deltaTime * lbc.moveSpeed);
            }
        }
        else if (Input.GetButton("Heal"))
        {
            lbc.transform.position = Vector3.Lerp(lbc.transform.position, this.transform.position, Time.deltaTime * lbc.moveSpeed);
            hasCalledBoL = true;
            // TODO: REMOVE OPTION TO MOVE BALL WHILE ISCALLED
        } 
        if (Input.GetButtonUp("Heal"))
        {
            hasCalledBoL = false;
        }
        if (Input.GetButton("Charge"))
        {
            lbc.isCharging = true;
        }
        else if (Input.GetButtonUp("Charge"))
        {
            lbc.isCharging = false;
        }
        if (Input.GetButton("Amplify"))
        {
            lbc.isAmp = true;
        }
        else if (Input.GetButtonUp("Amplify"))
        {
            lbc.isAmp = false;
        }
        if (Input.GetButtonDown("Crawl"))
        {
            crawling = !crawling;
        }
        if (crawling)
        {
            Debug.Log("CRAWRLINw!");
            physicalCollider.transform.localScale =new Vector3(1f,0.5f,1f);

        }
        else
        {
            physicalCollider.transform.localScale = new Vector3(1f, 1f, 1f);
        }
       /* else if (Input.GetButtonUp("Crawl"))
        {
            crawling = true;
        }*/

        lbc.transform.Translate(bmove);
        //Debug.Log("Input X is " + bx + " and input Y is " + by);
    }
    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            rigidbody.AddForce(new Vector3(0, artificialGravity, 0), ForceMode.Impulse);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

   /* private void Interact()
    {
        Physics.OverlapBox(this.transform.position,)
    }*/
}
