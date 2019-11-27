using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : MonoBehaviour
{
    public string rightKey;
    public bool unlocked = false;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "KeyLock";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
