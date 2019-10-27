using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Directions
{
    LEFT = -1, RIGHT = 1,
}
public enum PlayerStates
{
    None,Idle,Crawl,Drag,
}
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidbody;
    [SerializeField]
    GameObject physicalCollider;
    [SerializeField]
    private float walkSpeed = 3.5f;
    [SerializeField]
    private float originalWalkSpeed = 3.5f;
    [SerializeField]
    private float crawlSpeed = 2f;
    private const float BASICALLY_ZERO = 0.0001f;
    [SerializeField]
    private float AccelarationPerSecond = 0.5f;
    [SerializeField]
    private float deaccelerationPerSecond = 0.5f;
    private float currentHorizontalSpeed = 0f;
    private float currentRightSpeed = 0f;
    private float currentLeftSpeed = 0f;
    float climbSpeed;
    private bool isGrounded;
    [SerializeField]
    float jumpHeight = 2f;
    [SerializeField]
    float artificialGravity = -0.666f;
    //bool crawling;
    float courage;
    bool isAlive;
    private bool isMoving = false;
    private bool jumped = false;
    private Directions currentDirection = Directions.RIGHT;//the direction the player's currently facing
    private PlayerStates state = PlayerStates.None;
    public bool hasCalledBoL;

    public LightballController lbc;
    #region Ball Anchor:
    [Header("Ball Anchor")]
    public GameObject ballAnchor;
    private Vector3 ballAnchorDestination;
    //private float ballAnchorZ;
    public Transform balloflight;
    private DragInteractable draggedObject = null;

    [SerializeField]
    private float anchorForward = 3f;
    [SerializeField]
    private float anchorBackward = 0f;
    [SerializeField]
    private float anchorUpward = 3f;
    [SerializeField]
    private float anchorDownward = 0f;
    [SerializeField]
    private float ballAnchorMinDistanceFromPlayer = 0.2f;
    [SerializeField]
    private float ballAnchorLeadDistance = 1.5f;
    [SerializeField]
    private float anchorOutWard = 2f;
    [SerializeField]
    private float ballAnchorSpeed = 2f;
    #endregion

    // TODO: SET UP CAMERA SCRIPT WHICH WILL MAKE THE CAMERA ZOOM A LITTLE OUT WHILE BOL IS FAR AWAY (ORI)


    void Start()
    {
        ChangeAnchorPosition();
        //Physics.IgnoreCollision(balloflight.GetComponent<Collider>(), GetComponent<Collider>(),true);
    }

    private void ChangeAnchorPosition()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 newPos = new Vector3
              (Random.Range(anchorBackward * (float)currentDirection, anchorForward * (float)currentDirection),
               Random.Range(anchorDownward, anchorUpward), 
               (lbc.IsPushed ? anchorOutWard : 0f));
            if (isMoving)
            {
                newPos += new Vector3(ballAnchorLeadDistance * (float)currentDirection, 0, 0);
            }
            if (Vector3.Distance(Vector3.zero, newPos) > ballAnchorMinDistanceFromPlayer)
            {
                ballAnchorDestination = newPos;
                break;
            }
            //Try again if the randomised position aint far enough from the player..
        }
        Invoke("ChangeAnchorPosition", Random.Range(0.7f, 5f));
        //ballAnchor.transform.position = new Vector3(ballAnchor.transform.position.x, ballAnchor.transform.position.y,ballAnchorZ);
    }
    void Update()
    {
        // Movement
        float xInput = Input.GetAxisRaw("Horizontal");

        #region Controlling current speed:
        if (xInput > 0)
        {
            if (Mathf.Abs(currentRightSpeed) + (xInput * AccelarationPerSecond * Time.deltaTime) < walkSpeed)
            {
                currentRightSpeed += xInput * AccelarationPerSecond * Time.deltaTime;
            }
            else
            {
                currentRightSpeed = walkSpeed;
            }
        }
        else if (currentRightSpeed != 0)
        {
             if (Mathf.Abs(currentRightSpeed) - (deaccelerationPerSecond * Time.deltaTime) > 0)
             {
                  currentRightSpeed -= (currentRightSpeed / Mathf.Abs(currentRightSpeed)) * deaccelerationPerSecond * Time.deltaTime;
             }
             else
             {
                  currentRightSpeed = 0;
             }
        }
        if (xInput < 0)
        {
            if (Mathf.Abs(currentLeftSpeed) + (xInput * AccelarationPerSecond * Time.deltaTime) < walkSpeed)
            {
                currentLeftSpeed += xInput * AccelarationPerSecond * Time.deltaTime;
            }
            else
            {
                currentLeftSpeed = -walkSpeed;
            }
        }
        else if (currentLeftSpeed != 0)
        {
             if (Mathf.Abs(currentLeftSpeed) - (deaccelerationPerSecond * Time.deltaTime) > 0)
             {
                 currentLeftSpeed -= (currentLeftSpeed / Mathf.Abs(currentLeftSpeed)) * deaccelerationPerSecond * Time.deltaTime;
             }
             else
             {
                 currentLeftSpeed = 0;
             }
        }
        currentHorizontalSpeed = currentLeftSpeed + currentRightSpeed;
        /*if (Mathf.Abs(currentHorizontalSpeed) > walkSpeed&& currentHorizontalSpeed!=0)
        {
            currentHorizontalSpeed = walkSpeed * (Mathf.Abs(currentHorizontalSpeed) / currentHorizontalSpeed);
        }*/
        //Debug.Log("Horizontal X: " + Input.GetAxis("Horizontal"));
#endregion
        if (xInput != 0)
        {
            isMoving = true;
            if (xInput < 0)
            {
                if (currentDirection != Directions.LEFT)
                {
                    ChangeAnchorPosition();
                    currentDirection = Directions.LEFT;
                }
            }
            else
            {
                if (currentDirection != Directions.RIGHT)
                {
                    ChangeAnchorPosition();
                    currentDirection = Directions.RIGHT;
                }
            }
        }
        else
        {
            isMoving = false;
        }

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumped = true;
                //Jumpin' is handled in FixedUpdate since it deals with physics.
            }
        }

        //Vector3 ballAnchorStep = Vector3.Lerp(ballAnchor.transform.localPosition, ballAnchorDestination, Time.deltaTime * ballAnchorSpeed);
        //if (Vector3.Distance(Vector3.zero, ballAnchorStep) < ballAnchorMinDistanceFromPlayer)
        ballAnchor.transform.localPosition = Vector3.Lerp(ballAnchor.transform.localPosition, ballAnchorDestination, Time.deltaTime * ballAnchorSpeed);

        var bx = Input.GetAxis("BallHorizontal") * lbc.ControlledSpeed * Time.deltaTime;
        var by = Input.GetAxis("BallVertical") * lbc.ControlledSpeed * Time.deltaTime;
        var bmove = new Vector2(bx, by);
        if (lbc.State == LightBallStates.None)
        {
            //State can be changed only if it is currently set to None.
            if (Input.GetButton("Heal"))
            {
                lbc.State = LightBallStates.Heal;
                //hasCalledBoL = true;
            }
            else if (Input.GetButton("Charge"))
            {
                // lbc.isCharging = true;
                lbc.State = LightBallStates.Charge;
            }
            else if (Input.GetButton("Amplify"))
            {
                // lbc.isAmp = true;
                lbc.State = LightBallStates.Amplify;
            }

            if (bmove.magnitude < 0.01f)//if (Input.GetAxisRaw("BallHorizontal") == 0.0f && Input.GetAxisRaw("BallVertical") == 0.0f)
            {
                lbc.transform.position = Vector3.Lerp(lbc.transform.position, lbc.targetSpot.position, Time.deltaTime * lbc.IdleMovementSpeed);
            }
        }
        else
        {
            bmove = Vector2.zero;//I don't want the player to be able to move my balls if they is buisy doin somethin else
            if (lbc.State == LightBallStates.Heal)
            {
                if (!Input.GetButton("Heal"))
                {
                    lbc.State = LightBallStates.None;
                }
                else
                {
                    lbc.transform.position = Vector3.Lerp(lbc.transform.position, this.transform.position, Time.deltaTime * lbc.HealMovementSpeed);
                }
            }
            else if (lbc.State == LightBallStates.Charge)
            {
                if (!Input.GetButton("Charge"))
                {
                    lbc.State = LightBallStates.None;
                }
            }
            else if (lbc.State == LightBallStates.Amplify)
            {
                if (!Input.GetButton("Amplify"))
                {
                    lbc.State = LightBallStates.None;
                }
            }
        }
        hasCalledBoL = (lbc.State == LightBallStates.Heal);
        if (lbc.IsPushed)//Forcing the anchor to its appropriate Z
        {
            ballAnchor.transform.position += new Vector3(0, 0, (-ballAnchor.transform.position.z) - anchorOutWard);
            // (Z should become anchorOutWard) BUG:it becomes a different number for some reason..
        }
        else
        {
            ballAnchor.transform.position += new Vector3(0, 0, (-ballAnchor.transform.position.z));
            // (Z should become 0)
        }
        lbc.transform.Translate(bmove);

        if (Input.GetButtonDown("Crawl"))
        {
            // crawling = !crawling; )-;
            if (state == PlayerStates.Crawl)
            {
                state = PlayerStates.None;
            }
            else
            {
                draggedObject = null;
                state = PlayerStates.Crawl;
            }
        }
        if (state == PlayerStates.Crawl||state==PlayerStates.Drag)
        {
            walkSpeed = crawlSpeed;
            physicalCollider.transform.localScale = new Vector3(1f, 0.5f, 1f);

        }
        else
        {
            walkSpeed = originalWalkSpeed;
            physicalCollider.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        //Debug.Log("Input X is " + bx + " and input Y is " + by);
    }
    private void FixedUpdate()
    {
        if (jumped)
        {
            rigidbody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            isGrounded = false;
            jumped = false;
            state = PlayerStates.None;
            draggedObject = null;
        }

        if (!isGrounded)
        {
             rigidbody.AddForce(new Vector3(0, artificialGravity, 0), ForceMode.Impulse);
        }
        var move = new Vector3(currentHorizontalSpeed * Time.deltaTime, 0, 0);
        rigidbody.MovePosition(transform.position + move);
        //rigidbody.velocity = move;
        if (state == PlayerStates.Drag)// && draggedObject != null)
        {
            if (draggedObject != null)
            {
                //Drag
                draggedObject.rigidbody.MovePosition(transform.position);
            }
            else
            {
                state = PlayerStates.None;
            }
        }
    }
    public void Grab(DragInteractable grabbed)
    {
        draggedObject = grabbed;
        state = PlayerStates.Drag;
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