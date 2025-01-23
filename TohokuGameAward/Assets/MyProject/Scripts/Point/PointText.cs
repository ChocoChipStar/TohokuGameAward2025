using TMPro;
using UnityEngine;

public class PointText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_alphaTextGUI = null;

    [SerializeField]
    private TextMeshProUGUI m_bravoTextGUI = null;

    [SerializeField]
    private TextMeshProUGUI m_alphaTotalTextGUI = null;

    [SerializeField]
    private TextMeshProUGUI m_bravoTotalTextGUI = null;

    PointManager m_pointManager = null;

    private int[] m_alphaRoundScore = null;

    private int[] m_bravoRoundScore = null;

    private int m_currentAlphaScore = 0;

    private int m_currentBravoScore = 0;

    private int m_alphaTotalScore = 0;

    private int m_bravoTotalScore = 0;

    void Start()
    {
        m_pointManager = GetComponent<PointManager>();

        if(RoundManager.CurrentRound == (int)RoundManager.RoundState.One)
        {
            InitArray();
        }
        if(RoundManager.CurrentRound == (int)RoundManager.RoundState.Two)
        {
            GetLastRoundPoints();
        }
    }

    void Update()
    {
        DrawPoint();
    }

    void InitArray()
    {
        m_alphaRoundScore  = new int[(int)RoundManager.RoundState.Max];
        m_bravoRoundScore  = new int[(int)RoundManager.RoundState.Max];
    }

    void GetLastRoundPoints()
    {
        m_alphaRoundScore  = PointManager.AlphaRoundScore;
        m_bravoRoundScore  = PointManager.BravoRoundScore;
    }

    void DrawPoint()
    {
        GetPoint();

        m_alphaTextGUI.text  = m_currentAlphaScore.ToString();
        m_bravoTextGUI.text = m_currentBravoScore.ToString();

        m_bravoTotalTextGUI.text = m_alphaTotalScore.ToString();
        m_alphaTotalTextGUI.text  =  m_bravoTotalScore.ToString();
    }

    void GetPoint()
    {
        if (RoundManager.CurrentRound == (int)RoundManager.RoundState.One)
        {
            //ラウンド1だけ総ポイントと現ラウンドのポイントが同じになる
             m_currentAlphaScore = PointManager.AlphaRoundScore[(int)RoundManager.RoundState.One];
             m_currentBravoScore = PointManager.BravoRoundScore[(int)RoundManager.RoundState.One];

             m_alphaTotalScore = m_currentAlphaScore;
             m_bravoTotalScore = m_currentBravoScore;

            return;
        }
         m_currentAlphaScore = PointManager.AlphaRoundScore[RoundManager.CurrentRound];
         m_currentBravoScore = PointManager.BravoRoundScore[RoundManager.CurrentRound];

         m_alphaTotalScore = m_pointManager.GetTotalScore(PointManager.AlphaRoundScore);
         m_bravoTotalScore = m_pointManager.GetTotalScore(PointManager.BravoRoundScore);
    }
}