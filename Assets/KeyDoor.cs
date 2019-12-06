using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class KeyDoor : MonoBehaviour
{
    public KeyLock[] lockset;
    public UnityEvent OnUnlock;
    //check if all locks are unlocked
    public void CheckForUnlock()
    {
        for (int i = 0; i < lockset.Length; i++)
        {
            if (lockset[i].unlocked == false)
            {
                Debug.Log("not enough unlocks");
                return;
            }
        }
        //replace this line with door opening instead of destroying itself
        //  Destroy(this.gameObject);
        Unlock();
    }

    private void Unlock()
    {
        OnUnlock.Invoke();
    }
}
