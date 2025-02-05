using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_effectPrefab;

    public enum Type
    {
        Death,
        Finish
    }

    public void OnPlayEffect(Vector3 position, float angle, Type effectType)
    {
        var instance = Instantiate(m_effectPrefab[(int)effectType], position, Quaternion.identity);
        var childInstance = instance.transform.GetChild(0);

        var eulerAngles = childInstance.transform.eulerAngles;
        eulerAngles.x = angle + 180.0f;
        childInstance.transform.eulerAngles = eulerAngles;

        var effectPlayer = instance.GetComponent<EffectPlayer>();
        effectPlayer.OnPlay();
    }
}

