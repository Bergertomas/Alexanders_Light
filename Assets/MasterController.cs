using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour
{
    CourageManager cm;
    event Action GameOver;

    // Start is called before the first frame update
    void Start()
    {
        cm = GetComponent<CourageManager>();
        cm.CourageDepleted += ResetGame;
    }
    public void ResetGame()
    {
        Debug.Log("GAy mover!!!!!");
        SceneManager.LoadScene("DevScene");
        
    }
    //game
    // Update is called once per frame
    void Update()
    {
    }
}
