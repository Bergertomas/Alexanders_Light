using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInventory : MonoBehaviour
{
    KeyCharger targetCharger;
    KeyItem someKey = null;
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
            Debug.Log("found key");
            someKey = other.gameObject.GetComponent<KeyItem>();
            Destroy(other.gameObject);
            keyBag.Add(someKey.keyname, someKey);
            Debug.Log(keyBag[someKey.keyname].keyActive);
            someKey = null;
        }

        if (other.gameObject.tag == "KeyCharger")
        {
            targetCharger = other.gameObject.GetComponent<KeyCharger>();
            keyBag[targetCharger.chargeableKey].keyActive = true;
            Debug.Log(keyBag[targetCharger.chargeableKey].keyActive);
            targetCharger = null;
        }
    }
}
