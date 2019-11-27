using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInventory : MonoBehaviour
{
    KeyItem someKey = null;
    KeyCharger targetCharger;
    KeyLock targetLock;
    [SerializeField]
    Dictionary<string, KeyItem> keyBag = new Dictionary<string, KeyItem>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            someKey = other.gameObject.GetComponent<KeyItem>();
            Debug.Log("found keyname " + someKey.keyname);
            Destroy(other.gameObject);
            keyBag.Add(someKey.keyname, someKey);
            someKey = null;
        }

        if (other.gameObject.tag == "KeyCharger")
        {
            targetCharger = other.gameObject.GetComponent<KeyCharger>();
            if (keyBag.ContainsKey(targetCharger.chargeableKey))
            {
                keyBag[targetCharger.chargeableKey].keyActive = true;
                Debug.Log(targetCharger.chargeableKey + " is charged");
            }
            else
            {
                Debug.Log("No key to charge");
            }
            targetCharger = null;
        }

        if (other.gameObject.tag == "KeyLock")
        {
            targetLock = other.gameObject.GetComponent<KeyLock>();
            if (keyBag.ContainsKey(targetLock.rightKey))
            {
                if (keyBag[targetLock.rightKey].keyActive == true)
                {
                    targetLock.unlocked = true;
                    keyBag.Remove(targetLock.rightKey);
                    Debug.Log("unlocked with keyname " + targetLock.rightKey);
                }
                else
                {
                    Debug.Log("key is not charged");
                }
            }
            else
            {
                Debug.Log("No key");
            }
            targetLock = null;
        }
    }
}
