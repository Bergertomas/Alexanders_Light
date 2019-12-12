using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GreatArcStudios;

public class MasterController : MonoBehaviour
{
    CourageManager cm;
    //public PauseManager pm;
    [SerializeField]
    private GameObject pauseObj;
    public event Action GameOverEvent;
    public event Action RevertToPreviousCheckPoint;
    public event CheckPointDelegate CheckPointReached;
    [SerializeField]
    private float timeToWaitBeforeStartOver = 3f;

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
        pauseObj.SetActive(true);
        cm = GetComponent<CourageManager>();
        cm.CourageDepleted += GameOver;
        //pm.GetComponent<PauseManager>();
        //pm.RestartGame += GameOver;
        DeathPit[] deathPits= FindObjectsOfType<DeathPit>();
        for (int i = 0; i < deathPits.Length; i++)
        {
            deathPits[i].Pitfall += GameOver;
        }
        CheckPoint[] checkPoints = FindObjectsOfType<CheckPoint>();
        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].CheckPointReached += NewCheckPoint;
        }
        
        Invoke("FirstCheckPoint", 0.7f);
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

    private void GameOver()
    {
        GameOverEvent.Invoke();
        Invoke("StartOver", timeToWaitBeforeStartOver);
        Debug.Log("GAy mover!!!!!");
        //SceneManager.LoadScene("Alpha1");      
    }

    private void StartOver()
    {
        RevertToPreviousCheckPoint.Invoke();
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
