//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*public enum Directions
{
    LEFT = -1, RIGHT = 1,
}
public enum PlayerStates
{
    None, Idle, Crawl, Drag,
}*/
public struct RayCastOrigins
{
    public Vector3 TopLeft, TopRight, BottomLeft, BottomRight;
    public float HorizontalRaySpacing;
    public float VerticalRaySpacing;
    public int HorizontalRayCount;//= 5;
    public int VerticalRayCount;// = 4;
}
public struct CollisionInfo
{
    public bool Left, Right, Above, Below;
    public bool ClimbingSlope;
    public bool DescendingSlope;
    public float PreviousSlopeAngle;
    public float CurrentSlopeAngle;
    public Vector3 VelocityOld;
    public void Clear()
    {
        Left = Right = Above = Below = false;
        ClimbingSlope = false;
        DescendingSlope = false;
        PreviousSlopeAngle = CurrentSlopeAngle;
        CurrentSlopeAngle = 0;
    }
}
public class AlexanderController : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    [SerializeField]
    Rigidbody rigidbody;
    [SerializeField]
    GameObject physicalColliderObject;
    [Header("New collision system")]
    #region New collision system
    [SerializeField]
    Collider physicalCollider;
    [SerializeField]
    private float maxClimbSlopeAngle = 60f;
    [SerializeField]
    private float maxDescendSlopeAngle = 60f;
    private RayCastOrigins rayCastOrigins;
    private CollisionInfo collisionInfo;
    private GameObject collisionObject;


    [SerializeField]
    private float ascendinGravity = -18f;
    [SerializeField]
    private float descendinGravity = -25f;
    [SerializeField]
    private float jumpForce = -7f;
    private Vector3 currentVelocity;
    [SerializeField]
    private LayerMask collisionMask;
    #endregion
    [SerializeField]
    private float walkSpeed = 3.5f;
    [SerializeField]
    private float originalWalkSpeed = 3.5f;
    [SerializeField]
    private float crawlSpeed = 2f;
    [SerializeField]
    private float AccelarationPerSecond = 0.5f;
    [SerializeField]
    private float deaccelerationPerSecond = 0.5f;
    public float currentHorizontalSpeed = 0f;
    public float CameraXOffset = 0f;
    [SerializeField]
    private float CameraXOffsetWhenRunning = 5f;
    [SerializeField]
    private float CameraXOffsetMovingSpeed = 1f;
    [SerializeField]
    private float CameraXOffsetStandingSpeed = 1f;
    private float currentRightSpeed = 0f;
    private float currentLeftSpeed = 0f;
    [SerializeField]
    private float climbSpeed = 5.0f;
    [SerializeField]
    bool canClimb = false;
    private float ropeX;
    // private bool isGrounded;
    /*[SerializeField]
    float jumpHeight = 2f;
    [SerializeField]
    float artificialGravity = -0.666f;*/
    private DragInteractable draggedObject = null;
    private bool dragging = false;
    private Vector3 movedObjRelative;
    private Vector3 draggedObjectPreviousPosition = Vector3.zero;
    [SerializeField]
    private float interactionDistance = 1f;
    //bool crawling;
    float courage;
    bool isAlive;
    private bool isMovingOnX = false;
    private bool isBored = false;
    private bool isInsideDarkFire;
    public event DamageDelegates PlayerIsInsideDarkness;
    [SerializeField]
    private float timeToGetBored = 10f;
    private float timeWithoutAction = 0f;
    private bool didSomethingDurningFrame = false;
    //private bool jumped = false;
    // private float originalLocalScaleX;
    [SerializeField]
    private GameObject graphics;
    private Directions currentDirection = Directions.RIGHT;//the direction the player's currently facing
    [SerializeField]
    private PlayerStates state = PlayerStates.None;
    private Vector3 positionAtPreviousCheckPoint;

    public bool hasCalledBoL;
    [SerializeField]
    private Transform healPoint;
    public LightballController lbc;
    #region Ball Anchor:
    [Header("Ball Anchor")]
    public GameObject ballAnchor;
    private Vector3 ballAnchorDestination;
    //private float ballAnchorZ;
    //public Transform balloflight;


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
        MasterController.Instance.CheckPointReached += RecordCurrentState;
        MasterController.Instance.RevertToPreviousCheckPoint += RevertToPreviousCheckPoint;
        // originalLocalScaleX = transform.localScale.x;
        rayCastOrigins.HorizontalRayCount = 5;
        rayCastOrigins.VerticalRayCount = 4;
        ChangeAnchorPosition();

        Collisions.UpdateRayCastOrigins(physicalCollider, ref rayCastOrigins);
        Collisions.CalculateRaySpacing(physicalCollider, ref rayCastOrigins);
        DarknessParticles[] darknesses = FindObjectsOfType<DarknessParticles>();
        for (int i = 0; i < darknesses.Length; i++)
        {
            darknesses[i].PlayerIsInsideMe += BeDarkened;
        }
        //Physics.IgnoreCollision(balloflight.GetComponent<Collider>(), GetComponent<Collider>(),true);
    }

    private void BeDarkened()
    {
        PlayerIsInsideDarkness.Invoke(false);
    }

    public void RecordCurrentState(Transform checkPointTransform)
    {
        positionAtPreviousCheckPoint = new Vector3
            (checkPointTransform.position.x, checkPointTransform.position.y,0);//We dont want the checkpoint to tell us what Z to go to
    }
    public void RevertToPreviousCheckPoint()
    {
        transform.position = positionAtPreviousCheckPoint;
    }
    /* private void UpdateRayCastOrigins()
     {
         Bounds bounds = physicalCollider.bounds;
         bounds.Expand(Collisions.SKIN_WIDTH * -2);
         rayCastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
         rayCastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
         rayCastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
         rayCastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
     }
    /* private void CalculateRaySpacing()
     {
         Bounds bounds = physicalCollider.bounds;
         bounds.Expand(Collisions.SKIN_WIDTH * -2);
         horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
         verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
         horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
         verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
     }*/
    private void Move(Vector3 velocity)
    {
        collisionInfo.Clear();
        collisionInfo.VelocityOld = velocity;
        Collisions.UpdateRayCastOrigins(physicalCollider, ref rayCastOrigins);//maybe we shouldnt do this every Move()
        Collisions.CalculateRaySpacing(physicalCollider, ref rayCastOrigins);//maybe we shouldnt do this every Move()

        if (draggedObject != null)
        {
            draggedObjectPreviousPosition = draggedObject.transform.position;
            movedObjRelative = draggedObject.gameObject.transform.InverseTransformPoint(transform.position);
            Collisions.UpdateRayCastOrigins(draggedObject.Collider, ref draggedObject.RayCastOrigins);
            Collisions.CalculateRaySpacing(draggedObject.Collider, ref draggedObject.RayCastOrigins);
            if (velocity.x != 0)
            {
                DraggedHorizontalCollisions(ref velocity);
            }
            draggedObject.transform.Translate(new Vector3(velocity.x, 0, 0));
        }
        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        if (velocity.x != 0)
        {
            collisionObject = HorizontalCollisions(ref velocity);
            if (collisionObject != null)
            {
                if (collisionObject.GetComponent<Collectible>())
                {
                    collisionObject.GetComponent<Collectible>().Collect();
                }
            }
;
        }
        if (velocity.y != 0)
        {
            collisionObject = VerticalCollisions(ref velocity);
            if (collisionObject != null)
            {
                if (collisionObject.GetComponent<Collectible>())
                {
                    collisionObject.GetComponent<Collectible>().Collect();
                }
                else if (collisionObject.GetComponentInParent<MovingPlatform>()|| collisionObject.GetComponent<AmplifiedMovingPlatform>())
                {
                    this.transform.parent = collisionObject.transform.parent;
                }
            }
            else
            {
                this.transform.parent = null;
            }
        }
        transform.Translate(velocity);
        if (draggedObject != null)
        {
            draggedObject.transform.position = draggedObjectPreviousPosition;//an ugly solution to an ugly problem...
            draggedObject.transform.Translate(new Vector3(velocity.x, velocity.y, 0));
        }
    }
    private GameObject HorizontalCollisions(ref Vector3 velocity)
    {
        GameObject collisionObject=null;
        float XDirection = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + Collisions.SKIN_WIDTH;
        for (int i = 0; i < rayCastOrigins.HorizontalRayCount; i++)
        {
            Vector3 rayOrigin = (XDirection == -1) ? rayCastOrigins.BottomLeft : rayCastOrigins.BottomRight;
            rayOrigin += Vector3.up * (/*velocity.y +why not? */(rayCastOrigins.HorizontalRaySpacing * i));
            RaycastHit hit;
            Physics.Raycast(rayOrigin, Vector3.right * XDirection, out hit, rayLength, collisionMask);
            Debug.DrawLine(rayOrigin, rayOrigin + (Vector3.right * XDirection * rayLength), Color.red);
            if (hit.collider != null)
            {
                collisionObject= hit.collider.gameObject;
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxClimbSlopeAngle)
                {
                    if (collisionInfo.DescendingSlope)
                    {
                        collisionInfo.DescendingSlope = false;
                        velocity = collisionInfo.VelocityOld;
                    }
                    Debug.Log("Angle:" + slopeAngle);
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisionInfo.PreviousSlopeAngle)
                    {
                        distanceToSlopeStart = hit.distance - Collisions.SKIN_WIDTH;
                        velocity.x -= distanceToSlopeStart * XDirection;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * XDirection;
                }
                if (!collisionInfo.ClimbingSlope || slopeAngle > maxClimbSlopeAngle)
                {
                    /*if (hit.collider.gameObject.GetComponent<Drag
                     * ctable>()&& hit.collider.gameObject.GetComponent<DragInteractable>() == draggedObject)
                    {
                       transform.Translate(-SKIN_WIDTH * XDirection) =  ;
                    }*/
                    //else
                    {
                        velocity.x = (hit.distance - Collisions.SKIN_WIDTH) * XDirection;
                        rayLength = hit.distance;
                        if (collisionInfo.ClimbingSlope)
                        {
                            velocity.y = Mathf.Tan(collisionInfo.CurrentSlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                        }
                    }

                    collisionInfo.Left = (XDirection == -1);
                    collisionInfo.Right = (XDirection == 1);
                }
            }
        }
        return collisionObject;
    }
    private void DraggedHorizontalCollisions(ref Vector3 velocity)//One should merge this with HorizontalCollisions..
    {
        float XDirection = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + Collisions.SKIN_WIDTH;
        for (int i = 0; i < draggedObject.RayCastOrigins.HorizontalRayCount; i++)
        {
            Vector3 rayOrigin = (XDirection == -1) ? draggedObject.RayCastOrigins.BottomLeft : draggedObject.RayCastOrigins.BottomRight;
            rayOrigin += Vector3.up * (/*velocity.y +why not? */(draggedObject.RayCastOrigins.HorizontalRaySpacing * i));
            RaycastHit hit;
            Physics.Raycast(rayOrigin, Vector3.right * XDirection, out hit, rayLength, collisionMask);
            Debug.DrawLine(rayOrigin, rayOrigin + (Vector3.right * XDirection * rayLength), Color.red);
            if (hit.collider != null)
            {
                /* float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                 if (i == 0 && slopeAngle <= maxClimbSlopeAngle)
                 {
                     if (collisionInfo.DescendingSlope)
                     {
                         collisionInfo.DescendingSlope = false;
                         velocity = collisionInfo.VelocityOld;
                     }
                     Debug.Log("Angle:" + slopeAngle);
                     float distanceToSlopeStart = 0;
                     if (slopeAngle != collisionInfo.PreviousSlopeAngle)
                     {
                         distanceToSlopeStart = hit.distance - Collisions.SKIN_WIDTH;
                         velocity.x -= distanceToSlopeStart * XDirection;
                     }
                     ClimbSlope(ref velocity, slopeAngle);
                     velocity.x += distanceToSlopeStart * XDirection;
                 }*/
                //if (!collisionInfo.ClimbingSlope || slopeAngle > maxClimbSlopeAngle)
                {
                    velocity.x = (hit.distance - Collisions.SKIN_WIDTH) * XDirection;
                    rayLength = hit.distance;
                }

            }   
        }
    }
    private void ClimbSlope(ref Vector3 velocity, float slopeAngle)//Ori does not understand this one at all
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisionInfo.Below = true;
            collisionInfo.ClimbingSlope = true;
            collisionInfo.CurrentSlopeAngle = slopeAngle;
        }
        else
        {
            Debug.Log("Slope jump");
        }

    }
    private void DescendSlope(ref Vector3 velocity)
    {
        float XDirection = Mathf.Sign(velocity.x);
        Vector3 rayOrigin = (XDirection == 1 ? rayCastOrigins.BottomLeft : rayCastOrigins.BottomRight);
        RaycastHit hit;
        Physics.Raycast(rayOrigin, -Vector3.up, out hit, Mathf.Infinity, collisionMask);
        if (hit.collider != null)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendSlopeAngle)
            {
                if (Mathf.Sign(hit.normal.x) == XDirection)
                {
                    if (hit.distance - Collisions.SKIN_WIDTH <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.y -= descendVelocityY;
                        collisionInfo.CurrentSlopeAngle = slopeAngle;
                        collisionInfo.DescendingSlope = true;
                        collisionInfo.Below = true;
                    }
                }
            }

        }
    }
    private GameObject VerticalCollisions(ref Vector3 velocity)
    {
        GameObject collisionObject = null;
        float YDirection = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + Collisions.SKIN_WIDTH;
        for (int i = 0; i < rayCastOrigins.VerticalRayCount; i++)
        {
            Vector3 rayOrigin = (YDirection == -1) ? rayCastOrigins.BottomLeft : rayCastOrigins.TopLeft;
            rayOrigin += Vector3.right * (velocity.x + (rayCastOrigins.VerticalRaySpacing * i));
            RaycastHit hit;
            Physics.Raycast(rayOrigin, Vector3.up * YDirection, out hit, rayLength, collisionMask);
            Debug.DrawLine(rayOrigin, rayOrigin + (Vector3.up * YDirection * rayLength), Color.red);
            if (hit.collider != null)
            {
                collisionObject = hit.collider.gameObject;
                velocity.y = (hit.distance - Collisions.SKIN_WIDTH) * YDirection;
                rayLength = hit.distance;
                if (collisionInfo.ClimbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisionInfo.CurrentSlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }
                collisionInfo.Below = (YDirection == -1);
                collisionInfo.Above = (YDirection == 1);
            }
        }
        if (collisionInfo.ClimbingSlope)
        {
            float XDirection = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + Collisions.SKIN_WIDTH;
            Vector3 rayOrigin = ((XDirection == -1) ? rayCastOrigins.BottomLeft : rayCastOrigins.BottomRight) + (velocity.y * Vector3.up);
            RaycastHit hit;
            Physics.Raycast(rayOrigin, Vector3.right * XDirection, out hit, rayLength, collisionMask);
            if (hit.collider != null)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisionInfo.CurrentSlopeAngle)
                {
                    velocity.x = (hit.distance - Collisions.SKIN_WIDTH) * XDirection;
                    collisionInfo.CurrentSlopeAngle = slopeAngle;
                }
            }
        }
        return collisionObject;
    }
    private void ChangeAnchorPosition()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 newPos = new Vector3
              (UnityEngine.Random.Range(anchorBackward * (float)currentDirection, anchorForward * (float)currentDirection),
               UnityEngine.Random.Range(anchorDownward, anchorUpward),
               (lbc.IsPushed ? anchorOutWard : 0f));
            if (isMovingOnX)
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
        Invoke("ChangeAnchorPosition", UnityEngine.Random.Range(0.7f, 5f));
        //ballAnchor.transform.position = new Vector3(ballAnchor.transform.position.x, ballAnchor.transform.position.y,ballAnchorZ);
    }
    void Update()
    {
        if (isInsideDarkFire)
        {
            PlayerIsInsideDarkness(true);
        }
        didSomethingDurningFrame = false;
        //Draw player so that it looks look he's looking at the appropriate direction
        //graphics.transform.localScale = new Vector3(1 * (float)currentDirection, 1, 1);
        anim.SetBool("crawling", (state == PlayerStates.Crawl));
        anim.SetBool("holding", (state == PlayerStates.Drag));
        #region New collision system 
        if (collisionInfo.Above || collisionInfo.Below)
        {
            anim.SetBool("midair", false);
            currentVelocity.y = 0;
            if (collisionInfo.Below)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
                else if (Input.GetButtonDown("Interact"))
                {
                    //ReleaseDraggedObject();
                    
                    Interact();//Hmmm I suppose we can't interact midair
                }
                else if (Input.GetButtonDown("Crawl"))
                {
                    didSomethingDurningFrame = true;
                    // crawling = !crawling; )-;
                    if (state == PlayerStates.Crawl)
                    {
                        state = PlayerStates.None;
                        if (collisionInfo.Below)
                        {
                            transform.Translate(0, 0.5f, 0);
                        }
                    }
                    else
                    {
                        ReleaseDraggedObject();
                        state = PlayerStates.Crawl;
                    }
                }
            }
        }

        if (state == PlayerStates.Crawl || state == PlayerStates.Drag)
        {
            walkSpeed = crawlSpeed;
            /*if (state == PlayerStates.Crawl)
            {
                // physicalColliderObject.transform.localScale = new Vector3(1f, 0.5f, 1f);
                // physicalCollider.bounds.min.y += 1;
            }
            else*/
            if (state == PlayerStates.Drag)
            {
                float distanceFromDragged = 0.66f;
                if (movedObjRelative.x > 0)
                {
                    //graphics.transform.localPosition = new Vector3(0.5f, 0, 0);
                    graphics.transform.localPosition = new Vector3
                        ((draggedObject.Collider.bounds.max.x-transform.position.x) + distanceFromDragged , 0, 0);
                    Debug.Log("moved model to the right");
                }
                else
                {
                    //graphics.transform.localPosition = new Vector3(-0.5f, 0, 0);
                    graphics.transform.localPosition = new Vector3
                       ((draggedObject.Collider.bounds.min.x - transform.position.x) -  distanceFromDragged, 0, 0);
                    Debug.Log("moved model to the left");
                }
            }     
            //should the player get horter when he drags?
        }
        else
        {
            walkSpeed = originalWalkSpeed;
            // physicalColliderObject.transform.localScale = new Vector3(1f, 1f, 1f);      
        }
        

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

            /* if (CameraXOffset < CameraXOffsetWhenRunning)
             {
                 CameraXOffset += CameraXOffsetMovingSpeed * Time.deltaTime;
             }
             else
             {
                 CameraXOffset = CameraXOffsetWhenRunning;
             }*/
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

            /* if (CameraXOffset > -CameraXOffsetWhenRunning)
             {
                 CameraXOffset -= CameraXOffsetMovingSpeed * Time.deltaTime;
             }
             else
             {
                 CameraXOffset = -CameraXOffsetWhenRunning;
             }*/
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
        if (xInput != 0&&state!=PlayerStates.Climb)
        {
            anim.SetBool("walking", true);
            //if (state == PlayerStates.Drag)
            //{
            //    anim.SetBool("pushing", true);
            //}

            isMovingOnX = true;
            didSomethingDurningFrame = true;
            if (xInput < 0) //character going right
            {
                if (currentDirection != Directions.LEFT)
                {

                    ChangeAnchorPosition();
                    currentDirection = Directions.LEFT;
                }
                if (state != PlayerStates.Drag)//This part used to be inside  if (currentDirection != Directions.LEFT)
                {
                    graphics.transform.eulerAngles = new Vector3(0, 270, 0); //character face left
                }
                else
                {
                    if (movedObjRelative.x > 0)
                    {
                        anim.SetBool("pushing", true);
                        Debug.Log("anim.SetBool(pushing, true);");
                    }
                    else
                    {
                        anim.SetBool("pushing", false);
                        Debug.Log("anim.SetBool(pushing, false);");
                    }
                }
            }
            else //character going left
            {
                if (currentDirection != Directions.RIGHT)
                {           
                    ChangeAnchorPosition();
                    currentDirection = Directions.RIGHT;
                }
                if (state != PlayerStates.Drag)//This part used to be inside  if (currentDirection != Directions.RIGHT)
                {
                    graphics.transform.eulerAngles = new Vector3(0, 90, 0); //character face right                       
                }
                else
                {
                    if (movedObjRelative.x < 0)
                    {
                        anim.SetBool("pushing", true);
                        Debug.Log("anim.SetBool(pushing, true);");
                    }
                    else
                    {
                        anim.SetBool("pushing", false);
                        Debug.Log("anim.SetBool(pushing, false);");
                    }
                }
            }
        }
        else
        {
            //anim.SetBool("pushing", false);
            anim.SetBool("walking", false);
            //Debug.Log("Not Moving");
            isMovingOnX = false;
            /* if (CameraXOffset != 0 && currentHorizontalSpeed == 0)
             {
                 if ((Mathf.Abs(CameraXOffset) - ((CameraXOffset / Mathf.Abs(CameraXOffset)) * CameraXOffsetStandingSpeed * Time.deltaTime)) > 0)
                 {
                     CameraXOffset -= (CameraXOffset / Mathf.Abs(CameraXOffset)) * CameraXOffsetStandingSpeed * Time.deltaTime;
                 }
                 else
                 {
                     CameraXOffset = 0;
                 }
             }*/
        }

       
        //If the player is in rope distance and midair
        if (canClimb == true && !collisionInfo.Below)
        {
            if (Input.GetButtonDown("Vertical"))
            {
                state = PlayerStates.Climb;
            }
            /*  else if (xInput != 0)
              {
                  isMoving = false;
              }*/
        }
        
       /* if (state == PlayerStates.Climb && Input.GetAxisRaw("Vertical") == 0)
        {
            anim.SetBool("climbing", false);
            currentVelocity.y = 0f;
        }*/
       
        if (state == PlayerStates.Climb)
        {
            float yInput = Input.GetAxisRaw("Vertical");
            if (yInput != 0)//move up/down
            {
                if (yInput > 0)
                {
                    anim.SetBool("climbing", true);
                }
                
                anim.SetBool("hanging", true);
                currentVelocity.y = yInput * climbSpeed;
                #region commented out on 4.12
                //var moveY = currentVelocity.y * Time.deltaTime;
                //var moveClimb = new Vector2(0, moveY);
                //transform.Translate(moveClimb * Time.deltaTime * climbSpeed);
                #endregion

                //isMoving = true;
            }
            else //if (yInput == 0)
            {
                anim.SetBool("climbing", false);
                currentVelocity.y = 0f;
            }
            transform.position = new Vector3(ropeX, transform.position.y, 0);
            graphics.transform.eulerAngles = new Vector3(0, 0, 0);
            //currentVelocity.x = 0;
            if (Input.GetButtonDown("Jump")&&xInput!=0)
            {     
                Jump();
            }
        }
        else//Player falls when he's not climbing
        {
            currentVelocity.y += ((currentVelocity.y > 0) ? ascendinGravity : descendinGravity) * Time.deltaTime;
        }  
        currentVelocity.x = (state==PlayerStates.Climb?0: currentHorizontalSpeed);//Hmm why is this written in such a weird position? moved it down here on 4.12 
        Move(currentVelocity * Time.deltaTime);
        if (currentVelocity != Vector3.zero)
        {
            didSomethingDurningFrame = true;
        }
        #endregion
        if (!isBored)
        {
            if (CameraXOffset != CameraXOffsetWhenRunning * (float)currentDirection)
            {
                //if (Mathf.Abs(CameraXOffset) + CameraXOffsetMovingSpeed * Time.deltaTime< CameraXOffsetWhenRunning)
                //if (CameraXOffset + ((float)currentDirection) * (CameraXOffsetMovingSpeed * Time.deltaTime) < CameraXOffsetWhenRunning)

                CameraXOffset += ((float)currentDirection) * (CameraXOffsetMovingSpeed * Time.deltaTime);
                if ((currentDirection == Directions.RIGHT && CameraXOffset > CameraXOffsetWhenRunning) ||
                   (currentDirection == Directions.LEFT && CameraXOffset < -CameraXOffsetWhenRunning))//Ugly math..
                {
                    CameraXOffset = CameraXOffsetWhenRunning * (float)currentDirection;
                }

                /*else
                {
                    CameraXOffset = CameraXOffsetWhenRunning * (float)currentDirection;
                }*/
            }
        }
        else if (CameraXOffset != 0)
        {
            if ((Mathf.Abs(CameraXOffset) - (CameraXOffsetStandingSpeed * Time.deltaTime)) > 0)
            {
                CameraXOffset -= (Mathf.Abs(CameraXOffset) / CameraXOffset) * CameraXOffsetStandingSpeed * Time.deltaTime;
            }
            else
            {
                CameraXOffset = 0;
            }
        }

        if (!didSomethingDurningFrame)
        {
            timeWithoutAction += Time.deltaTime;
            if (timeWithoutAction > timeToGetBored)
            {
                isBored = true;
            }
        }
        else
        {
            isBored = false;
            timeWithoutAction = 0f;
        }
        //Vector3 ballAnchorStep = Vector3.Lerp(ballAnchor.transform.localPosition, ballAnchorDestination, Time.deltaTime * ballAnchorSpeed);
        //if (Vector3.Distance(Vector3.zero, ballAnchorStep) < ballAnchorMinDistanceFromPlayer)
        ballAnchor.transform.localPosition = Vector3.Lerp(ballAnchor.transform.localPosition, ballAnchorDestination, Time.deltaTime * ballAnchorSpeed);

        var bx = Input.GetAxis("BallHorizontal") * lbc.ControlledSpeed * Time.deltaTime;
        var by = Input.GetAxis("BallVertical") * lbc.ControlledSpeed * Time.deltaTime;
        var bmove = new Vector2(bx, by);

        if (lbc.State == LightBallStates.None || lbc.State == LightBallStates.Amplify)
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

            if (bmove.magnitude < 0.01f && lbc.State != LightBallStates.Amplify)//if (Input.GetAxisRaw("BallHorizontal") == 0.0f && Input.GetAxisRaw("BallVertical") == 0.0f)
            {
                lbc.transform.position = Vector3.Lerp(lbc.transform.position, ballAnchor.transform.position, Time.deltaTime * lbc.IdleMovementSpeed);
            }
            if (Input.GetButton("DragBOL"))//Moving with the mouse
            {
                lbc.transform.position = Camera.main.ScreenToWorldPoint(new Vector3
                     (Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(lbc.transform.position).z));
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
                    lbc.transform.position = Vector3.Lerp(lbc.transform.position, healPoint.position + new Vector3((float)currentDirection * 0.3f, 0.8f, 0), Time.deltaTime * lbc.HealMovementSpeed);
                }
            }
            else if (lbc.State == LightBallStates.Charge)
            {
                if (!Input.GetButton("Charge"))
                {
                    lbc.State = LightBallStates.None;
                }
            }

        }

        if (lbc.State == LightBallStates.Amplify)
        {
            if (!Input.GetButton("Amplify"))
            {
                lbc.State = LightBallStates.None;
                Debug.Log("No longer amplifying");
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
        // Debug.Log("mouse:" + (Input.mousePosition));
        //Debug.Log("Input X is " + bx + " and input Y is " + by);
    }

    private void Jump()
    {
        anim.SetBool("midair", true);
        anim.SetBool("climbing", false);
        anim.SetBool("hanging", false);
        state = PlayerStates.None;
        didSomethingDurningFrame = true;
        ReleaseDraggedObject();
        currentVelocity.y = jumpForce;
    }

    #region draggedObject related:
    public void Grab(DragInteractable grabbed, DragInteractable previousDragged)
    {
        if (grabbed != previousDragged)
        {
            draggedObject = grabbed;
            draggedObject.MoveToDraggedState();
            //draggedObject.transform.parent = this.gameObject.transform;
            //Joint joint= draggedObject.gameObject.AddComponent<FixedJoint>();
            //joint.connectedBody = rigidbody;
            state = PlayerStates.Drag;
        }
    }

    private void ReleaseDraggedObject()
    {
        if (draggedObject != null)
        {
            /* if (draggedObject.gameObject.GetComponent<FixedJoint>())
             {
                 Destroy(draggedObject.gameObject.GetComponent<FixedJoint>());
             }*/
            //draggedObject.transform.parent = null;
            draggedObject.MoveToFreeState();
            draggedObject = null;
            state = PlayerStates.None;
            graphics.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
    #endregion

    private void Interact()
    {
        didSomethingDurningFrame = true;
        Debug.Log("Interact");
        DragInteractable currentDraggedObject = draggedObject;
        ReleaseDraggedObject();
        RaycastHit interactionHit;
        Vector3 rayOrigin = physicalCollider.transform.position + new Vector3(0, 0.5f, 0);
        Physics.Raycast(rayOrigin, Vector3.right * (float)currentDirection, out interactionHit, interactionDistance, collisionMask);//TODO-send more than one ray
        Debug.DrawLine(rayOrigin, rayOrigin + (Vector3.right * (float)currentDirection * interactionDistance), Color.yellow, 0.5f);
        if (interactionHit.collider != null)
        {
            Debug.Log("interactionHit.collider != null");
            GameObject hitObject = interactionHit.collider.gameObject;
            if (hitObject.GetComponent<Interactable>())
            {
                hitObject.GetComponent<Interactable>().Interact();
            }
            if (hitObject.GetComponent<DragInteractable>())
            {
                Grab(hitObject.GetComponent<DragInteractable>(), currentDraggedObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            ropeX = other.gameObject.transform.position.x;
            canClimb = true;
        }

        if (other.gameObject.tag == "Evil")
        {
            isInsideDarkFire = true;
            //Debug.Log("2spooky4me");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            anim.SetBool("hanging", false);
            anim.SetBool("climbing", false);
            //ropeX = other.gameObject.transform.position.x;
            canClimb = false;
            if (state == PlayerStates.Climb)
            {
                state = PlayerStates.None;
            }

        }

        if (other.gameObject.tag == "Evil")
        {
            isInsideDarkFire = false;
        }
    }

}