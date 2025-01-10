using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager m_playerManager = null;

    [SerializeField]
    Timer m_timer = null;

    [SerializeField]
    private int[] m_score = null;

    private int[] m_defencesIndex = null;

    private int m_defencesScore = 0;

    private int[] m_offencesIndex = null;

    private int m_offencesScore = 0;

    static public int m_finaldefScore = 0;

    static public int m_finaloffScore = 0;

    public int DefencesScore { get { return m_defencesScore; } }

    public int OffencesScore { get { return m_offencesScore; } }

    public static int FinalDefScore { get { return m_finaldefScore; } }

    public static int FinalOffScore { get {return m_finaloffScore; } }

    void Start()
    {
        Array.Resize(ref m_score, m_playerManager.PlayerCount.Length);
        m_defencesIndex = new int[m_score.Length];
        m_offencesIndex = new int [m_score.Length];
    }
    private void Update()
    {
        AddPoint();

        m_defencesScore = m_score[0];

        if (m_timer.IsTimeLimit)
        {
            SaveScore();
        }
    }

    void AddPoint()
    {
        for (int i = 0; i < m_score.Length; i++)
        {
            if (i < 2)
            {
                m_defencesIndex[i] = i + 1;
            }
            else
            {
                m_offencesIndex[i] = i + 1;
            }

        }
        //↑ここでm_defencesIndexとm_offencesIndexに攻守のIndexを入れ、
        //その値をもとにポイントを追加しようと考えています。
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
            bool existsInOff = Array.Exists(m_defencesIndex, element => element == i);
            if(existsInOff)
            {
                offencesScore += m_score[i];
                continue;
            }
        }
            m_defencesScore = defencesScore;
            m_offencesScore = offencesScore;
    }

    private void SaveScore()
    {
        m_finaldefScore = m_defencesScore;
        m_finaloffScore = m_offencesScore;
    }

    public int[] GetScore()
    {
        return m_score; 
    }

    public void AddScore(int playerIndex,int score)
    {
        m_score[playerIndex] += score;
    }
}
