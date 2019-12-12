using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class KeyLock : MonoBehaviour
{
    public bool unlocked = false;
    private bool toldDoorToCheckForUnlock = false;
    public KeyDoor targetDoor;
    [SerializeField]
    GameObject unlockedPrefab;
    public UnityEvent OnUnlock;

    void Start()
    {
        gameObject.tag = "KeyLock";
    }

    void Update()
    {
        if (!toldDoorToCheckForUnlock&&unlocked == true)
        {
            targetDoor.CheckForUnlock();
            toldDoorToCheckForUnlock = true;
            OnUnlock.Invoke();
            //TODO: replace the empty lock model with a new model with a key in it
            //Instantiate(unlockedPrefab, this.transform.position, this.transform.rotation);
        }
    }
}
