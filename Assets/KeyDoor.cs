using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public KeyLock[] lockset;

    //check if all locks are unlocked
    public void CheckForUnlock()
    {
        for (int i = 0; i < lockset.Length - 1; i++)
        {
            if (lockset[i].unlocked == false)
            {
                Debug.Log("not enough unlocks");
                return;
            }
        }
        //replace this line with door opening instead of destroying itself
        Destroy(this.gameObject);
    }
}
