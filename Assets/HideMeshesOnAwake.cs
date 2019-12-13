using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMeshesOnAwake : MonoBehaviour
{
    private void Start()
    {
        MeshRenderer[] graphics = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].enabled = false;
        }
        GetComponent<MeshRenderer>().enabled = false;
    }
}
