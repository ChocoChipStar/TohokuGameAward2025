using UnityEngine;

[CreateAssetMenu(fileName = "ResultTextData", menuName = "ScriptableObjects/ResultTextData")]


public class ResultTextData : ScriptableObject
{
    [SerializeField]
    private ParamsData m_paramsData = new ParamsData();

    public ParamsData Params { get { return m_paramsData; } private set { value = m_paramsData; } }
}


[System.Serializable]
public class ParamsData
{
    [SerializeField, Header("スコアシャッフル時の最小値")]
    private int m_shuffleValueMin = 0;

    [SerializeField, Header("スコアのランダム表示の最大値")]
    private int m_shuffleValueMax = 0;

    [SerializeField, Header("次のラウンドスコアを表示させるまでの待機時間")]
    private float[] m_drawNextRoundDelayTime = null;

    [SerializeField, Header("演出スコアシャッフルの時間")]
    private float[] m_scoreShuffleTime = null;

    [SerializeField, Header("勝利チームのUIを表示させるまでの待機時間")]
    private float m_drawWinnerUIDelayTime = 0.0f;

    public int ShuffleValueMin { get { return m_shuffleValueMin; } private set { value = m_shuffleValueMin; } }
    public int ShuffleValueMax { get { return m_shuffleValueMax; } private set { value = m_shuffleValueMax; } }
    public float[] NextRoundDelayTime { get { return m_drawNextRoundDelayTime; } private set { m_drawNextRoundDelayTime = value; } }
    public float[] ScoreShuffleTime { get { return m_scoreShuffleTime; } private set { m_scoreShuffleTime = value; } }
    public float DrawWinnerUIDelayTime { get { return m_drawWinnerUIDelayTime; } private set { m_drawWinnerUIDelayTime = value; } }
}