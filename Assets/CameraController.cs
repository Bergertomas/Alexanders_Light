using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speedRecoverySpeed = 1f;
    [SerializeField]
    private float normalXSpeed = 5f;
    [SerializeField]
    private float normalYSpeed = 5f;
    [SerializeField]
    private float fieldOfViewSpeed = 2f;
    [SerializeField]
    private bool ZMovement = true;
    private bool ZMovementAdaptations = false;
    [SerializeField]
    private float currentXSpeed = 5f;
    [SerializeField]
    private float currentYSpeed = 5f;
    [SerializeField]
    private float yOffsetFromPlayer = 1;
    private float originalFieldOfView;
    private float originalZ;
    [SerializeField]
    private float riseSpeed = 0.5f;
    // private float currentFieldOfView;
    private Vector3 positionAtPreviousCheckPoint;
    private bool followAlex = true;
    private void LowerSpeed(CameraRapist rapist)
    {
        //if (rapists.Count < 1)
        {
            float speedDivider = ((rapist.PlayerStrength + rapist.RapistStrength) / rapist.PlayerStrength)*2.4f;
            currentXSpeed = normalXSpeed / speedDivider;
            currentYSpeed = normalYSpeed / speedDivider;
        }
    }

    [SerializeField]
    private AlexanderController player;
    private List<CameraRapist> rapists = new List<CameraRapist>();

    public static CameraController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Tried to create another camera.");
            Destroy(this);
        }
    }
    private void Start()
    {
        player = FindObjectOfType<AlexanderController>();
        originalFieldOfView = Camera.main.fieldOfView;
        originalZ = transform.position.z;
        MasterController.Instance.CheckPointReached += RecordCurrentState;
        MasterController.Instance.RevertToPreviousCheckPoint += RevertToPreviousCheckPoint;
        MasterController.Instance.GameOverEvent += delegate () { followAlex = false; };
    }

    public void RecordCurrentState(Transform checkPointTransform)
    {
        positionAtPreviousCheckPoint = this.transform.position;//We dont want the checkpoint to tell us what Z to go to
    }

    public void RevertToPreviousCheckPoint()
    {
        transform.position = positionAtPreviousCheckPoint;
        followAlex = true;
    }

    public void AddRapist(CameraRapist rapist)
    {
        if (!rapists.Contains(rapist))
        {
            rapists.Add(rapist);
            if (rapists.Count <= 1)
            {
                rapist.RapistStrength = 0;
            }

            // LowerSpeed();
        }
        else
        {
            Debug.Log("Tried to add a rapist more than once");
        }
    }

    public void RemoveRapist(CameraRapist rapist)
    {
        if (rapists.Contains(rapist))
        {
            rapists.Remove(rapist);
            LowerSpeed(rapist);
        }
        else
        {
            Debug.Log("Tried to remove a rapist more than once");
        }
    }

    void Update()
    {
        if (followAlex)
        {
            //speed recovery: 
            if (currentXSpeed < normalXSpeed)
            {
                currentXSpeed += speedRecoverySpeed * Time.deltaTime;
            }
            else
            {
                currentXSpeed = normalXSpeed;
            }
            if (currentYSpeed < normalYSpeed)
            {
                currentYSpeed += speedRecoverySpeed * Time.deltaTime;
            }
            else
            {
                currentYSpeed = normalYSpeed;
            }
            float newX = 0; //this.transform.position.x;
            float newY = 0;// this.transform.position.y;
            float newFieldOfView = 0;
            float fieldOfViewModifier = 0;
            //float playerXOffset = player.currentHorizontalSpeed * xOffsetWhenRunning;
            if (rapists.Count > 0)
            {
                float membersX = 0f;
                float membersY = 0f;
                float numberOfMembers = 0;
                CameraRapist removableRapist = null;
                foreach (CameraRapist r in rapists)
                {

                    numberOfMembers += r.PlayerStrength + r.RapistStrength;
                    membersX +=
                      (((player.transform.position.x + player.CameraXOffset) - this.transform.position.x) * r.PlayerStrength) +
                      ((r.Centre.position.x - this.transform.position.x) * r.RapistStrength);
                    membersY +=
                      (((player.transform.position.y + yOffsetFromPlayer) - this.transform.position.y) * r.PlayerStrength) +
                      ((r.Centre.position.y - this.transform.position.y) * r.RapistStrength);
                    fieldOfViewModifier += r.FieldOfViewModifier;
                    r.RapistUpdate();
                    if (!r.gameObject.activeSelf)
                    {
                        removableRapist = r;
                    }
                }
                newX += (membersX / (float)numberOfMembers);
                newY += (membersY / (float)numberOfMembers);
                fieldOfViewModifier /= (float)rapists.Count;
                if (removableRapist != null)//We're doing it this way cause we cant modify a list while going through it..
                {
                    RemoveRapist(removableRapist);
                }
            }
            else//Normal noncoersive camera movement
            {
                newX += ((player.transform.position.x + player.CameraXOffset) - this.transform.position.x);
                newY += ((player.transform.position.y + yOffsetFromPlayer) - this.transform.position.y);
            }

            //TODO- we might wanna change the speed on certain occasions, like existing a rapist
            newX = (newX * currentXSpeed * Time.deltaTime) + this.transform.position.x;
            newY = (newY * currentYSpeed * Time.deltaTime) + this.transform.position.y;
            if (ZMovement)
            {
              /*  if (!ZMovementAdaptations)
                {
                    //Camera.main.fieldOfView = 60;
                    originalZ -= 5;
                    ZMovementAdaptations = true;
                }*/
                float newZ = transform.position.z+
                    (((originalZ-(fieldOfViewModifier/5.3f))-transform.position.z)*Time.deltaTime* fieldOfViewSpeed);

                transform.position = new Vector3(newX, newY, newZ);
            }
            else
            {
                newFieldOfView =
                  (((originalFieldOfView + fieldOfViewModifier) - Camera.main.fieldOfView) * Time.deltaTime * fieldOfViewSpeed)
                  + Camera.main.fieldOfView;
                transform.position = new Vector3(newX, newY, transform.position.z);
                Camera.main.fieldOfView = newFieldOfView;
                //Camera.main.fieldOfView = originalFieldOfView;
            }

        }
        else
        {
            transform.position = new Vector3
                (transform.position.x, transform.position.y + (riseSpeed * Time.deltaTime), transform.position.z);
        }
    }
}
