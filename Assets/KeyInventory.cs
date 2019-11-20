using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInventory : MonoBehaviour
{
    KeyItem collectedKey;
    [SerializeField]
    Dictionary<string, bool> keyBag = new Dictionary<string, bool>();

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
            collectedKey = other.gameObject.GetComponent<KeyItem>();
            Destroy(other.gameObject);
            keyBag.Add(collectedKey.keyname, collectedKey.keyActive);
            collectedKey = null;
        }
    }
}
