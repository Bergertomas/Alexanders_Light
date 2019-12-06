using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkFire : MonoBehaviour
{
    [SerializeField]
   private ParticleSystem ps;
    [SerializeField]
    private GameObject scaler;

    void Start()
    {
        if (scaler.GetComponent<MeshRenderer>())
        {
            scaler.GetComponent<MeshRenderer>().enabled = false;
        }

        UnityEngine.ParticleSystem.ShapeModule editableShape = ps.shape;
        editableShape.scale = scaler.transform.localScale;
    }

}
