using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DarknessParticles : MonoBehaviour
{
    ParticleSystem ps;
    public event Action PlayerIsInsideMe;
    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        //Debug.Log("particles..");
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        if (numInside > 0)
        {
            PlayerIsInsideMe.Invoke();
        }
    }

}
