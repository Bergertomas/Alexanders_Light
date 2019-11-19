using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRemover : MonoBehaviour
{
    void Start()
    {
        if (GetComponent<Collider>())
        {
            Destroy(GetComponent<Collider>());
        }
        Collider[] unnecessaryColliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < unnecessaryColliders.Length; i++)
        {
            Destroy(unnecessaryColliders[i]);
        }
    }

}
