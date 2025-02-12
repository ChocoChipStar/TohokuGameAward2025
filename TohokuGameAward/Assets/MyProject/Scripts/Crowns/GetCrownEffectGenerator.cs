using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCrownEffectGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject m_effectPrefab = null;

    List<GameObject> m_effects = new List<GameObject>();

    public void GenerateGetEffect(Vector3 position)
    {
        GameObject instance = Instantiate(m_effectPrefab, position, Quaternion.identity, this.transform);

        ParticleSystem effect = instance.GetComponent<ParticleSystem>();

        StartEffect(effect);
        StartCoroutine(DestroyEffect(effect));
    }

    public void StartEffect(ParticleSystem effect)
    {
        if(effect != null)
        effect.Play();
    }

    private IEnumerator DestroyEffect(ParticleSystem? effect = null)
    {
        if(effect == null)
        {
            yield break;
        }

        while (effect.isPlaying)
        {
            yield return null;
        }

        Destroy(effect.gameObject);
    }
}
