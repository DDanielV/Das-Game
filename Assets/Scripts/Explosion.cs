using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float _maxParticleDuration;
    private void Start()
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            _maxParticleDuration = particleSystem.main.duration > _maxParticleDuration
                ? particleSystem.main.duration
                : _maxParticleDuration;
         
            particleSystem.Play();
        }
        Destroy(gameObject, _maxParticleDuration);
    }
}
