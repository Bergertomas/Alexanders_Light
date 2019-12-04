using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : MonoBehaviour
{
    public bool unlocked = false;
    public KeyDoor targetDoor;
    [SerializeField]
    GameObject unlockedPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "KeyLock";
    }

    // Update is called once per frame
    void Update()
    {
        if (unlocked == true)
        {
            targetDoor.CheckForUnlock();
            //TODO: replace the empty lock model with a new model with a key in it
            //Instantiate(unlockedPrefab, this.transform.position, this.transform.rotation);
        }
    }
}
