using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidbody;
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

    // TODO: SET UP CAMERA SCRIPT WHICH WILL MAKE THE CAMERA ZOOM A LITTLE OUT WHILE BOL IS FAR AWAY (ORI)


    // Start is called before the first frame update
    void Start()
    {
        ChangeAnchorPosition();
    }
    private void ChangeAnchorPosition()
    {

        //ballAnchor.transform.position = Random.insideUnitSphere * 2;
        Vector3 newPos = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f);
        ballAnchor.transform.position = this.transform.position+newPos;
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
        var bmove = new Vector3(bx, by);
        if (Input.GetAxisRaw("BallHorizontal") == 0.0f && Input.GetAxisRaw("BallVertical") == 0.0f && !Input.GetButton("Heal"))
        {
            if(!lbc.isCharging)
            {
                lbc.transform.position = Vector3.Lerp(lbc.transform.position, lbc.targetSpot.position, Time.deltaTime * lbc.moveSpeed);
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
        lbc.transform.Translate(bmove);
       


//        if (bmove == new Vector3(0, 0)) {
 //           lbc.transform.position = Vector3.Lerp(lbc.transform.position, lbc.targetSpot.position, Time.deltaTime * moveSpeed);
   //     }
       // lbc.transform.Translate(bmove);

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


}
