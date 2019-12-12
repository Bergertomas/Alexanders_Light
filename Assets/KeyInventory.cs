using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInventory : MonoBehaviour
{
    KeyItem someKey = null;
    KeyLock targetLock;
    Stack<KeyItem> keybag1 = new Stack<KeyItem>();


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        { 
            someKey = other.gameObject.GetComponent<KeyItem>();
            if (!keybag1.Contains(someKey))
            {
                Debug.Log("found key");
                Destroy(other.gameObject);
                keybag1.Push(someKey);
                someKey = null;
            }

            //TODO: add UI element to represent number of keys held.
            //easy mode: a picture of the key with number next to it.
            //hard mode: a line of images of keys. images appear and disappear according to number of keys
        }


        if (other.gameObject.tag == "KeyLock")
        {
            targetLock = other.gameObject.GetComponent<KeyLock>();
            if (targetLock.unlocked == false)
            {
                if (keybag1.Count > 0)
                {
                    targetLock.unlocked = true;
                    keybag1.Pop();
                    Debug.Log("unlocked a lock and I have " + keybag1.Count + " keys left");
                }
                else
                {
                    Debug.Log("no keys left for this lock");
                }
            }
            else
            {
                Debug.Log("already unlocked this lock");
            }

            targetLock = null;
        }
    }
}
