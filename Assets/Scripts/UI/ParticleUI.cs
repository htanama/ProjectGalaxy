using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleUI : MonoBehaviour
{
    public ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();

        if (ps != null )
            StopParticles();
    }
    public void SimulateParticles()
    {
        if (ps != null && !ps.isPlaying)
        {
            ps.Simulate(Time.unscaledDeltaTime, true, false);
        }
    }
    public void PlayParticles()
    {
        if (ps != null && !ps.isPlaying)
        {
            ps.Play();
        }
    }
    public void StopParticles()
    {
        if(ps != null)
            ps.Stop();
    }
}
