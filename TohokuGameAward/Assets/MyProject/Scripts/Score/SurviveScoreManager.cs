﻿using UnityEngine;

public class SurviveScoreManager : MonoBehaviour
{
    [SerializeField]
    RespawnManager m_respawnManager = null;

    [SerializeField]
    ScoreData m_scoreData = null;

    private bool[] m_isDead = new bool [HumanoidManager.HumanoidMax];

    private int[] m_scoreLevel = new int [HumanoidManager.HumanoidMax];

    private float[] m_alivingTime = new float[HumanoidManager.HumanoidMax];

    private float[] m_upgradeScoreTime = new float[HumanoidManager.HumanoidMax];

    void Update()
    {
        for(int i = 0; i < HumanoidManager.HumanoidMax; i++)
        {
         
            if (!m_isDead[i])
            {
                TimeUpdate();
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
        for(int i = 0; i < HumanoidManager.HumanoidMax; i++)
        {
            m_alivingTime[i] += Time.deltaTime;
            m_upgradeScoreTime[i] += Time.deltaTime;
        }
    }

    void ScoreUp(int i)
    {
        if(m_scoreData.Params.UpgradeScoreTime < m_upgradeScoreTime[i])
        {

            if(m_scoreLevel[i] < m_scoreData.Params.AlivingScore.Length - 1)
            {
                m_scoreLevel[i] += 1;
            }

            m_upgradeScoreTime[i] = 0;
        }
    }

    void AddAliveScore(int i)
    {
        if (m_alivingTime[i] >= m_scoreData.Params.AliveScoreTime)
        {
            ScoreManager.Instance.UpdateScore(TeamGenerator.Instance.GetCurrentHumanoidTeamName(),
                                              m_scoreData.Params.AlivingScore[m_scoreLevel[i]]);
            m_alivingTime[i] = 0;
        }
    }

    public void UpdateIsDead(int Index,bool isDead)
    {
        m_isDead[Index] = isDead;
    }
}
//ヒューマノイドのisDeadを取得、IsDeadになったプレイヤーの数字をリセット
//ヒューマノイドの死でboolをオフにし、数字をリセット
//ヒューマノイドのリスポーン時にboolをオンにする。
//死ぬとリセット(死ぬ処理の時に呼び出す）