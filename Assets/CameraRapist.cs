using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRapist : MonoBehaviour
{
    public Transform Centre;
    public int RapistStrength = 1;
    public int PlayerStrength = 1;
    void Start()
    {
        MeshRenderer[] graphics= GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].enabled = false;  
        }
    }
}
