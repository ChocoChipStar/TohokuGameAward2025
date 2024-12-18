using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    private ParticleSystem[] m_particleSystems = null;
    private void Update()
    {
        if (m_particleSystems == null)
        {
            return;
        }     

        bool m_stoppedAll = true;

        foreach (ParticleSystem particleSystem in m_particleSystems)
        {
            if (particleSystem.IsAlive())
            {
                m_stoppedAll = false;
                break;
            }
        }

        if (m_stoppedAll)
        {
            Destroy(gameObject);
        }
    }

    public void OnPlay()
    {
        m_particleSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem particleSystem in m_particleSystems)
        {
            particleSystem.Play();
        }
    }
}
