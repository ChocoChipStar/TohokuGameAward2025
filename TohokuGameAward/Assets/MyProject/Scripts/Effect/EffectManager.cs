using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_effectObject;

    public enum EffectType
    {
        test
    }

    public void OnPlayEffect(Vector3 position, EffectType effectType)
    {
        GameObject effect = Instantiate(m_effectObject[(int)effectType], position, Quaternion.identity);

        EffectPlayer effectPlayer = effect.GetComponent<EffectPlayer>();

        effectPlayer.OnPlay();
    }
}
