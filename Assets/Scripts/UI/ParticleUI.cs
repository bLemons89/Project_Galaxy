using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleUI : MonoBehaviour
{
    public ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        StopParticles();
    }
    public void SimulateParticles()
    {
        if (!ps.isPlaying)
        {
            ps.Simulate(Time.unscaledDeltaTime, true, false);
        }
    }
    public void PlayParticles()
    {
        if (!ps.isPlaying)
        {
            ps.Play();
        }
    }
    public void StopParticles()
    {
        ps.Stop();
    }
}
