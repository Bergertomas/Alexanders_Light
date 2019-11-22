using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifiedMovingPlatform : BallInteractable
{
    private const float SMALL_DISTANCE = 0.05f;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private Transform[] amplifiedTransforms;
    [SerializeField]
    private Transform depressedTransform;
    private Vector3 currentDestination;
    private int currentDestinationIndex = 0;
    private bool isAmplified = false;
    [SerializeField]
    private bool isAmplifiedOnStart = false;

    private RayCastOrigins rayCastOrigins;
    private Collider collider;
    private GameObject parent;
    [SerializeField]
    private LayerMask collisionMask;

    protected override void Start()
    {
        base.Start();
        rayCastOrigins.VerticalRayCount = 4;
        rayCastOrigins.HorizontalRayCount = 4;
        collider = this.GetComponent<Collider>();
        parent = transform.parent.gameObject;
        isAmplified= isAmplifiedOnStart;
        
        currentDestination = (isAmplified? amplifiedTransforms[currentDestinationIndex].position:depressedTransform.position);
    }


    protected override void Update()
    {
        base.Update();
        //Vector3 newDestination;
        Collisions.UpdateRayCastOrigins(collider, ref rayCastOrigins);//maybe we shouldnt do this every Move()
        Collisions.CalculateRaySpacing(collider, ref rayCastOrigins);//maybe we shouldnt do this every Move()
        if (beingInteractedWith)
        {
            isAmplified = true;
        }
         
        if (isAmplified)
        {
            if (VerticalCollisions() != null)
            {
                isAmplified = false;
            }
            if (Vector3.Distance(parent.transform.position, currentDestination) < SMALL_DISTANCE)
            {
                //change destination
                currentDestinationIndex++;
                if (currentDestinationIndex >= amplifiedTransforms.Length)
                {
                    currentDestinationIndex = 0;
                }
                currentDestination = amplifiedTransforms[currentDestinationIndex].position;
            }
        }
        else
        {
            currentDestination = depressedTransform.position;
        }

        parent.transform.position = Vector3.MoveTowards
        (parent.transform.position, currentDestination, Time.deltaTime * speed);
    }

    private GameObject VerticalCollisions(/*ref Vector3 velocity*/)
    {
        GameObject collisionObject = null;
        float YDirection = 1;//Mathf.Sign(velocity.y);
        float rayLength = /*Mathf.Abs(velocity.y)*/Collisions.SKIN_WIDTH*2;
        for (int i = 0; i < rayCastOrigins.VerticalRayCount; i++)
        {
            Vector3 rayOrigin = (YDirection == -1) ? rayCastOrigins.BottomLeft : rayCastOrigins.TopLeft;
            rayOrigin += Vector3.right * (/*velocity.x + */(rayCastOrigins.VerticalRaySpacing * i));
            RaycastHit hit;
            Physics.Raycast(rayOrigin, Vector3.up * YDirection, out hit, rayLength, collisionMask);
            Debug.DrawLine(rayOrigin, rayOrigin + (Vector3.up * YDirection * rayLength), Color.red);
            if (hit.collider != null)
            {
                collisionObject = hit.collider.gameObject;
                /*velocity.y = (hit.distance - Collisions.SKIN_WIDTH) * YDirection;
                rayLength = hit.distance;
                if (collisionInfo.ClimbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisionInfo.CurrentSlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }
                collisionInfo.Below = (YDirection == -1);
                collisionInfo.Above = (YDirection == 1);*/
            }
        }
        
        return collisionObject;
    }
}
