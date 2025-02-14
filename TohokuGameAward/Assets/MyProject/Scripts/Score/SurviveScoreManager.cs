using System.Collections;
using UnityEngine;

public class SurviveScoreManager : MonoBehaviour
{
    [SerializeField]
    PlayerFlash m_playerFlash = null;

    [SerializeField]
    PlayerManager m_playerManager = null;

    [SerializeField]
    SurviveScoreText m_scoreText = null;

    [SerializeField]
    RespawnManager m_respawnManager = null;

    [SerializeField]
    ScoreData m_scoreData = null;

    private bool[] m_isAddScoreDelay = new bool[HumanoidManager.HumanoidMax];

    private bool[] m_isDead = new bool[HumanoidManager.HumanoidMax];

    private int[] m_scoreLevel = new int[HumanoidManager.HumanoidMax];

    private float[] m_alivingTime = new float[HumanoidManager.HumanoidMax];

    private float[] m_upgradeScoreTime = new float[HumanoidManager.HumanoidMax];

    void OnEnable()
    {
        for (int i = 0; i < HumanoidManager.HumanoidMax; i++)
        {
            StartCoroutine(SetScoreDelayTimer(i)); //ゲームスタート後指定秒間はポイントを加算しない。
        }
    }
    void Update()
    {
        for (int i = 0; i < HumanoidManager.HumanoidMax; i++)
        {

            if (!m_isDead[i])
            {
                TimeUpdate();

                if (m_isAddScoreDelay[i])
                {
                    continue;
                }

                ScoreUp(i);
                AddAliveScore(i);
            }
            else
            {
                m_alivingTime[i] = 0;
                m_upgradeScoreTime[i] = 0;
                m_scoreLevel[i] = 0;
            }
        }
    }

    private void TimeUpdate()
    {
        for (int i = 0; i < HumanoidManager.HumanoidMax; i++)
        {
            m_alivingTime[i] += Time.deltaTime;

            if (m_isAddScoreDelay[i])
            {
                continue;
            }

            m_upgradeScoreTime[i] += Time.deltaTime;
        }
    }

    void ScoreUp(int i)
    {
        if (m_scoreData.Params.UpgradeScoreTime < m_upgradeScoreTime[i])
        {

            if (m_scoreLevel[i] < m_scoreData.Params.AlivingScore.Length - 1)
            {
                m_scoreLevel[i] += 1;
                m_playerFlash.OnPerticle(i, m_scoreLevel[i]);
            }

            m_upgradeScoreTime[i] = 0;
        }
    }

    void AddAliveScore(int i)
    {
        if (m_alivingTime[i] >= m_scoreData.Params.AliveScoreTime)
        {
            int score = m_scoreData.Params.AlivingScore[m_scoreLevel[i]];
            ScoreManager.Instance.UpdateScore(TeamGenerator.Instance.GetCurrentHumanoidTeamName(), score);

            m_scoreText.ShowScoreEffect(score, m_playerManager.HumanoidInstances[i].transform.position);
            m_alivingTime[i] = 0;
        }
    }

    public void SetIsDead(int Index)
    {
        m_isDead[Index] = true;
    }

    public void OffIsDead(int Index)
    {
        StartCoroutine(SetScoreDelayTimer(Index));
        m_isDead[Index] = false;
        m_playerFlash.OffPerticle(Index);
    }

    private IEnumerator SetScoreDelayTimer(int Index)
    {
        m_isAddScoreDelay[Index] = true;
        yield return new WaitForSeconds(m_scoreData.Params.AddScoreDelay[Index]);
        m_isAddScoreDelay[Index] = false;
    }
}
