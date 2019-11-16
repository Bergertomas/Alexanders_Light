using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour
{
    CourageManager cm;
   // event Action GameOver;

    private void Start()
    {
        cm = GetComponent<CourageManager>();
        cm.CourageDepleted += ResetGame;
        DeathPit[] deathPits= FindObjectsOfType<DeathPit>();
        for (int i = 0; i < deathPits.Length; i++)
        {
            deathPits[i].Pitfall += ResetGame;
        }
    }

    private void ResetGame()
    {
        Debug.Log("GAy mover!!!!!");
        SceneManager.LoadScene("Alpha1");
        
    }
}
