using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticleBehaviour : MonoBehaviour
{
    [SerializeField] private ParticleSystem waterParticle;
    public void WaterStart(Component sender, object data)
    {
        if (data is bool)
        {
            if ((bool)data)
            {
                waterParticle.Play();
            } 
            else
            {
                waterParticle.Stop();
            }
        }
    }
}
