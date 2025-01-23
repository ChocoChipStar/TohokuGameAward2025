using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager m_playerManager = null;

    [SerializeField]
    private PointData m_pointData = null;

    [SerializeField]
    private int[] m_score = new int[InputData.PlayerMax];

    private static List<int> m_alphaRoundScore  = new List<int>();

    private static List<int> m_bravoRoundScore = new List<int>();

    public static List<int> AlphaRoundScore { get { return m_alphaRoundScore; } }

    public static List<int> BravoRoundScore { get { return m_bravoRoundScore; } }

    private void Start()
    {
        m_alphaRoundScore.Add(1);
        m_bravoRoundScore.Add(1);
    }
    private void Update()
    {
        UpdatePoint();
    }

    private void UpdatePoint()
    {
        if(RoundManager.CurrentRound >= (int)RoundManager.RoundState.Max)
        {
            return;
        }

        int alphaScore = 0;
        int bravoScore = 0;

        for (int i = 0;i < m_score.Length; i++)
        {
            bool isAlphaTeam = PlayerManager.AlphaTeamNumber.Contains(i);
            bool isBravoTeam = PlayerManager.BravoTeamNumber.Contains(i);

            if(isAlphaTeam)
            {
                alphaScore += m_score[i];
                continue;
            }
            if(isBravoTeam)
            {
                bravoScore += m_score[i];
                continue;
            }
        }
        m_alphaRoundScore[RoundManager.CurrentRound] = alphaScore;
        m_bravoRoundScore[RoundManager.CurrentRound] = bravoScore;
    }

    public int GetTotalScore(List<int> Score)
    {
        int totalScore = 0;
        for (int i = 0; i < Score.Count; i++)
        {
            totalScore += Score[i];
        }
        return totalScore;
    }

    public void AddScore(int playerIndex,int score)
    {
        m_score[playerIndex] += score;
    }

    public void DecreaseScore(int playerIndex,int score)
    {
        m_score[playerIndex] -= score;
    }
}
