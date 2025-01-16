using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager m_playerManager = null;

    [SerializeField]
    Timer m_timer = null;

    [SerializeField]
    CanonPointManager m_canonPoint = null;

    [SerializeField]
    PointData m_pointData = null;

    [SerializeField]
    private int[] m_score = null;

    private float[] m_scoreInterval = null;

    private int[] m_defencesIndex = null;

    private int[] m_offencesIndex = null;

    static private int[] m_defroundScore = null;

    static private int[] m_offroundScore = null;

    public static int[] DefRoundScore { get { return m_defroundScore; } }

    public static int[] OffRoundScore { get { return m_offroundScore; } }

    void Start()
    {
        Array.Resize(ref m_score, m_playerManager.PlayerCount.Length);
        Array.Resize(ref m_scoreInterval, m_playerManager.PlayerCount.Length);
        if (Timer.Round == 0)
        {
            m_defroundScore = new int[m_timer.FinalRound];
            m_offroundScore = new int[m_timer.FinalRound];
        }
        m_defencesIndex = new int[m_score.Length];
        m_offencesIndex = new int [m_score.Length];
    }
    private void Update()
    {
        ShotScore();
        AddPoint();
    }

    void ShotScore()
    {
        for (int i = 0; i < m_playerManager.PlayerCount.Length; i++)
        {
            if (m_playerManager.IsShot[i])
            {
                AddCannonPoint();
                DecreaseScore();
                m_playerManager.IsShot[i] = false;
            }
        }
    }
    void AddPoint()
    {
        if(Timer.Round >= m_timer.FinalRound)
        {
            return;
        }

        for (int i = 0; i < m_score.Length; i++)
        {
            if (Timer.Round == 0)
            {
                if (!m_playerManager.IsCannon[i])//逃げる側だったら
                {
                    m_defencesIndex[i] = i;//こちらにインデックスを格納
                }
                else//大砲側だったら
                {
                    m_offencesIndex[i] = i;//こちらにインデックスを格納
                }
            }
        }

        for (int i = 0; i < m_score.Length; i++)
        {
            if (Timer.Round == 1)
            {
                if (!m_playerManager.IsCannon[i])//逃げる側だったら
                {
                    m_offencesIndex[i] = i;//こちらにインデックスを格納
                }
                else//大砲側だったら
                {
                    m_defencesIndex[i] = i;//こちらにインデックスを格納
                }
            }
        }

        int defencesScore = 0;
        int offencesScore = 0;

        for (int i = 0;i < m_score.Length; i++)
        {
            bool existsInDef = Array.Exists(m_defencesIndex, element => element == i);

            if(existsInDef)
            {
                defencesScore += m_score[i];
                continue;
            }
            bool existsInOff = Array.Exists(m_offencesIndex, element => element == i);
            if(existsInOff)
            {
                offencesScore += m_score[i];
                continue;
            }
        }
        m_defroundScore[Timer.Round] = defencesScore;
        m_offroundScore[Timer.Round] = offencesScore;
    }

    public int GetTotalScore(int[] Score)
    {
        int totalScore = 0;
        for (int i = 0; i < Score.Length; i++)
        {
            totalScore += Score[i];
        }
        return totalScore;
    }

    public void AddScore(int playerIndex, int score)
    {
        m_score[playerIndex] += score;
    }

    public void AddCannonPoint()
    {
        for (int i = 0; i < m_score.Length; i++)
        {
            if (m_playerManager.IsCannon[i])
            {
                m_score[i] += m_pointData.Params.CannonPoint;
                break;
            }
        }
    }
    
    public void DecreaseScore()
    {
        for (int i = 0; i < m_score.Length; i++)
        {
            if (!m_playerManager.IsCannon[i])
            {
                m_score[i] -= m_pointData.Params.CannonPoint;
                break;
            }
        }
    }
}
