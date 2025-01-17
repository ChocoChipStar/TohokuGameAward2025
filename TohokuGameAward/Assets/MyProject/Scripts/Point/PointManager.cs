using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager m_playerManager = null;

    [SerializeField]
    private PointData m_pointData = null;

    [SerializeField]
    GameTimer m_timer = null;

    [SerializeField]
    private int[] m_score = null;

    private float[] m_scoreInterval = null;

    private int[] m_defencesIndex = null;

    private int[] m_offencesIndex = null;

    static private int[] m_defroundScore = null;

    static private int[] m_offroundScore = null;

    private bool[] isDeadPoint = new bool[4];

    [SerializeField]
    private float[] isDeadPointInterVal = new float[4];

    public static int[] DefRoundScore { get { return m_defroundScore; } }

    public static int[] OffRoundScore { get { return m_offroundScore; } }

    public bool[] IsDeadPoint { get { return isDeadPoint; }  set{isDeadPoint = value; } }


    public float[] DeadPointInterVal { get { return isDeadPointInterVal; } set { isDeadPointInterVal = value; } }

    void Start()
    {
        Array.Resize(ref m_score, m_playerManager.Instances.Length);
        Array.Resize(ref m_scoreInterval, m_playerManager.Instances.Length);
        if (RoundManager.CurrentRound == (int)RoundManager.RoundState.One)
        {
            m_defroundScore = new int[(int)RoundManager.RoundState.Max];
            m_offroundScore = new int[(int)RoundManager.RoundState.Max];
        }
        m_defencesIndex = new int[m_score.Length];
        m_offencesIndex = new int [m_score.Length];
    }
    private void Update()
    {
        IsDeadPointInterVal();
        DeathScore();
        AddPoint();
    }

    void DeathScore()
    {
        for (int i = 0; i < m_playerManager.Instances.Length; i++)
        {
            if (isDeadPoint[i] && isDeadPointInterVal[i] > 0)
            {
                AddCannonPoint();
                DecreaseScore();
                isDeadPoint[i] = false;
            }
        }
    }
    void IsDeadPointInterVal()
    {

        for (int i = 0; i < m_playerManager.Instances.Length; i++)
        {
            if (isDeadPointInterVal[i] > 0)
            {
                isDeadPointInterVal[i] -= Time.deltaTime;
            }
        }
    }

    void AddPoint()
    {
        if(RoundManager.CurrentRound >= (int)RoundManager.RoundState.Max)
        {
            return;
        }

        for (int i = 0; i < m_score.Length; i++)
        {
            if (RoundManager.CurrentRound == (int)RoundManager.RoundState.One)
            {
                var isNotCannon = !TagManager.Instance.SearchedTagName(m_playerManager.Instances[i], TagManager.Type.Cannon);
                if (isNotCannon)//逃げる側だったら
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
            if (RoundManager.CurrentRound == (int)RoundManager.RoundState.Two)
            {
                var isNotCannon = !TagManager.Instance.SearchedTagName(m_playerManager.Instances[i], TagManager.Type.Cannon);
                if (isNotCannon)//逃げる側だったら
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
        m_defroundScore[RoundManager.CurrentRound] = defencesScore;
        m_offroundScore[RoundManager.CurrentRound] = offencesScore;
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

    public void AddScore(int playerIndex,int score)
    {
        m_score[playerIndex] += score;
    }

    public void AddCannonPoint()
    {
        for (int i = 0; i < m_score.Length; i++)
        {
            if (TagManager.Instance.SearchedTagName(m_playerManager.Instances[i], TagManager.Type.Cannon))
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
            if (TagManager.Instance.SearchedTagName(m_playerManager.Instances[i],TagManager.Type.Player))
            {
                m_score[i] -= m_pointData.Params.PenaltyPoint;
                break;
            }
        }
    }
}
