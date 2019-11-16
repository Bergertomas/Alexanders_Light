using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRapist : MonoBehaviour
{
    public Transform Centre;
    public float RapistStrength = 1;
    public float PlayerStrength = 1;
    public float TimeToGetToMaxStrength;
    private float rapistMaxStrength;
    public float FieldOfView = 90;
    
    void Start()
    {
        MeshRenderer[] graphics= GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].enabled = false;  
        }
        rapistMaxStrength = RapistStrength;
    }
    public void RapistUpdate()
    {
        if(RapistStrength<rapistMaxStrength)
        {
            RapistStrength += rapistMaxStrength * (Time.deltaTime / TimeToGetToMaxStrength);//Seems right..
        }
        else
        {
            RapistStrength = rapistMaxStrength;
        }
    }
}
