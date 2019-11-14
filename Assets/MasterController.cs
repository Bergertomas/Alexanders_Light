using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour
{
    CourageManager cm;
    event Action GameOver;

    private void Start()
    {
        cm = GetComponent<CourageManager>();
        cm.CourageDepleted += ResetGame;
    }
    private void ResetGame()
    {
        Debug.Log("GAy mover!!!!!");
        SceneManager.LoadScene("Alpha1");
        
    }
}
