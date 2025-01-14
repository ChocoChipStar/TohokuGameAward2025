using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_effectPrefab;

    [SerializeField]
    private EffectPosCalculator m_effectPos = null;

    public enum EffectType
    {
        test,
        StageOut
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            OnPlayEffect(Vector3.zero,EffectType.test);
        }
    }

    public void OnPlayEffect(Vector3 position, EffectType effectType)
    {
        GameObject effect = Instantiate(m_effectPrefab[(int)effectType], position, Quaternion.identity);

        EffectPlayer effectPlayer = effect.GetComponent<EffectPlayer>();

        effectPlayer.OnPlay();
    }

    public void OnPlayStageOutEffect(Vector3 position, EffectType effectType)
    {
        Vector3 offsetPos = Vector3.zero;

        GameObject effect = Instantiate(m_effectPrefab[(int)effectType], position + m_effectPos.OffsetPos(offsetPos, position), Quaternion.identity);

        //以下4行でエフェクトを正しい角度に回転しています。
        float Zangle = m_effectPos.GetAngle(Vector3.zero, new Vector2(position.x, position.y));

        Vector3 angle = effect.transform.eulerAngles;

        angle.z = Zangle;

        effect.transform.eulerAngles = angle;

        EffectPlayer effectPlayer = effect.GetComponent<EffectPlayer>();

        effectPlayer.OnPlay();
    }
}

