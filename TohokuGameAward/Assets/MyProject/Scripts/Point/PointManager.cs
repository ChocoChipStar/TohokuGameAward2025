using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager m_playerManager = null;

    [SerializeField]
    private PointData m_pointData = null;

    [SerializeField]
    private int[] m_score = new int[InputData.PlayerMax];

    static private int[] m_alphaRoundScore = null;

    static private int[] m_bravoRoundScore = null;

    public static int[] AlphaRoundScore { get { return m_alphaRoundScore; } }

    public static int[] BravoRoundScore { get { return m_bravoRoundScore; } }

    //以下のゲッターと変数はGameManagerとのコンフリクト防止のため次のブランチで消します。

    private bool[] m_isDeadPoint = new bool[InputData.PlayerMax];

    private float[] m_deadPenartyInterVal = new float[InputData.PlayerMax];

    public bool[] IsDeadPoint { get { return m_isDeadPoint; }  set{ m_isDeadPoint = value; } }

    public float[] DeadPointInterVal { get { return m_deadPenartyInterVal; } set { m_deadPenartyInterVal = value; } }

    void Start()
    {
        if (RoundManager.CurrentRound == (int)RoundManager.RoundState.One)
        {
            m_alphaRoundScore = new int[(int)RoundManager.RoundState.Max];
            m_bravoRoundScore = new int[(int)RoundManager.RoundState.Max];
        }
    }
    private void Update()
    {
        UpdatePoint();
    }

    void UpdatePoint()
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

    public void DecreaseScoe(int playerIndex,int score)
    {
        m_score[playerIndex] -= score;
    }
}
