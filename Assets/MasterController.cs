using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GreatArcStudios;

public class MasterController : MonoBehaviour
{
    CourageManager cm;
    public PauseManager pm;
    public event Action RevertToPreviousCheckPoint;
    public event CheckPointDelegate CheckPointReached;

    public static MasterController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Tried to create another MasterController.");
            Destroy(this);
        }
    }

    private void Start()
    {
        cm = GetComponent<CourageManager>();
        cm.CourageDepleted += ResetGame;
        pm.GetComponent<PauseManager>();
        pm.RestartGame += ResetGame;
        DeathPit[] deathPits= FindObjectsOfType<DeathPit>();
        for (int i = 0; i < deathPits.Length; i++)
        {
            deathPits[i].Pitfall += ResetGame;
        }
        CheckPoint[] checkPoints = FindObjectsOfType<CheckPoint>();
        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].CheckPointReached += NewCheckPoint;
        }
        
        Invoke("FirstCheckPoint", 0.1f);
    }
    private void FirstCheckPoint()
    {
        CheckPointReached.Invoke(FindObjectOfType<AlexanderController>().transform);
    }
    

    private void NewCheckPoint(Transform checkPointTransform)
    {
        CheckPointReached.Invoke(checkPointTransform);
        Debug.Log("NewCheckPoint");
        /*BallInteractable[] ballInteractables = FindObjectsOfType<BallInteractable>();
        for (int i = 0; i < ballInteractables.Length; i++)
        {
            ballInteractables[i].RecordCurrentState();
        }*/
    }

    private void ResetGame()
    {
        RevertToPreviousCheckPoint.Invoke();
        Debug.Log("GAy mover!!!!!");
        //SceneManager.LoadScene("Alpha1");
        
    }

    /*private void RevertToPreviousCheckPoint()
    {
        BallInteractable[] ballInteractables = FindObjectsOfType<BallInteractable>();
        for (int i = 0; i < ballInteractables.Length; i++)
        {
            ballInteractables[i].RevertToPreviousState();
        }
    }*/
}
