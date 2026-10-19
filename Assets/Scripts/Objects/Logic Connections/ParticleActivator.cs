using System.Collections.Generic;
using UnityEngine;

public class ParticleActivator : MonoBehaviour
{
    [SerializeField] private ISignal signal;
    [SerializeField] private List<ParticleSystem> ParticleSystems = new List<ParticleSystem>();

    private void Update()
    {
        if (signal == null || ParticleSystems == null) return;

        bool isActive = signal.Evaluate();

        foreach (ParticleSystem ps in ParticleSystems)
        {
            if (ps == null) continue;

            if (isActive)
            {
                if (ps.isStopped) ps.Play();
            }
            else
            {
                if (ps.isPlaying) ps.Stop();
            }
        }
    }
}