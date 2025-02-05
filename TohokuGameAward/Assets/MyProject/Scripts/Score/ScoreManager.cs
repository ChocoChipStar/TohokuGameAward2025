using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance = null;

    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private DrawScoreText m_drawScoreText = null;

    [SerializeField]
    private ScoreData m_scoreData = null;

    private static int[] m_alphaRoundScore = new int[(int)RoundManager.RoundState.Max];
    private static int[] m_bravoRoundScore = new int[(int)RoundManager.RoundState.Max];

    public static int[] AlphaRoundScore { get { return m_alphaRoundScore; } }
    public static int[] BravoRoundScore { get { return m_bravoRoundScore; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// スコアの更新処理を行います
    /// </summary>
    /// <param name="teamName"> スコア獲得するチーム名 </param>
    /// <param name="newScore"> 追加するスコア値 </param>
    public void UpdateScore(string teamName, int newScore)
    {
        // アルファか、ブラボーかチームを調べます
        var alphaTeamName = TeamGenerator.TeamName[(int)TeamGenerator.TeamType.Alpha];
        if (alphaTeamName == teamName)
        {
            m_alphaRoundScore[RoundManager.CurrentRound] += newScore;
        }
        else
        {
            m_bravoRoundScore[RoundManager.CurrentRound] += newScore;
        }
        m_drawScoreText.UpdateText();
    }
}
