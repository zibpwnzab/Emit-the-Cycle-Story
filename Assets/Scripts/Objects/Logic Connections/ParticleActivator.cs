using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActivator : MonoBehaviour
{
    [SerializeField] ISignal signal;
    [SerializeField] List<ParticleSystem> ParticleSystems;


    // Update is called once per frame
    void Update()
    {
        if (!signal) return;
        bool _signal = signal.Signal();
        foreach (var s in ParticleSystems)
        {
            if (_signal) { 
                if(s.isStopped)
                s.Play();
            }
            else s.Stop();
        }

    }
}
