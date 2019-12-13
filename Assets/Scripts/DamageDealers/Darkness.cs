using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour
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

    /*private void OnParticleTrigger()
    {
        Debug.Log("Particle trigger");
        //ParticleSystem ps = GetComponent<ParticleSystem>();

        // particles
        List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

        // get
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        if (numInside > 0)
        {
            Debug.Log("Particle trigger");
        }
        // iterate
        /* for (int i = 0; i < numInside; i++)
         {
             ParticleSystem.Particle p = inside[i];
             p.startColor = new Color32(255, 0, 0, 255);
             inside[i] = p;
         }

         // set
         ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
         ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
     }
    }*/
}
