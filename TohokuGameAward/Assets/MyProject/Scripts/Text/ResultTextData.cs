using UnityEngine;

[CreateAssetMenu(fileName = "ResultTextData", menuName = "ScriptableObjects/ResultTextData")]


public class ResultTextData : ScriptableObject
{
    [SerializeField]
    private EffectData m_effectData = new EffectData();

    [SerializeField]
    private ParamsData m_paramsData = new ParamsData();

    public EffectData Effect { get { return m_effectData; } private set { value = m_effectData; } }

    public ParamsData Params { get { return m_paramsData; } private set { value = m_paramsData; } }

}

[System.Serializable]
public class EffectData
{
    [SerializeField, Header("スコア表示の間")]
    private float[] m_waitForNextRound = null;

    [SerializeField, Header("ランダム演出の時間")]
    private float[] m_revealDelay = null;

    [SerializeField, Header("勝者を表示するまでの時間")]
    private float m_renderWinnerDelay = 0.0f;

    public float[] WaitForNextRound { get { return m_waitForNextRound; } private set { m_waitForNextRound = value; } }

    public float[] RevealDelay { get { return m_revealDelay; } private set { m_revealDelay = value; } }

    public float winnerTextureDelay { get { return m_renderWinnerDelay; } private set { m_renderWinnerDelay = value; } }
}

[System.Serializable]
public class ParamsData
{
    [SerializeField, Header("スコアシャッフル時の最小値")]
    private int m_shaffuleValueMin = 0;

    [SerializeField, Header("スコアのランダム表示の最大値")]
    private int m_shaffuleValueMax = 0;

    public int ShaffuleValueMin { get { return m_shaffuleValueMin; } private set { value = m_shaffuleValueMin; } }

    public int ShaffuleValueMax { get { return m_shaffuleValueMax; } private set { value = m_shaffuleValueMax; } }
}