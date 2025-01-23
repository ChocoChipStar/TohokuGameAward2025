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

    private PointManager m_pointManager = null;

    private int m_totalAlphaScore = 0;
    private int m_totalBravoScore = 0;

    private void Start()
    {
        m_pointManager = GetComponent<PointManager>();
    }

    private void SetScoreText(int currentAlphaScore, int currentBravoScore) 
    {
        m_alphaTextGUI.text  = PointManager.AlphaRoundScore[RoundManager.CurrentRound].ToString();
        m_bravoTextGUI.text =  PointManager.BravoRoundScore[RoundManager.CurrentRound].ToString();

        m_bravoTotalTextGUI.text = m_totalAlphaScore.ToString();
        m_alphaTotalTextGUI.text  =  m_totalBravoScore.ToString();
    }

    private void SetTotalScore(int currentAlphaScore, int currentBravoScore)
    {
        m_totalAlphaScore = m_totalAlphaScore + currentAlphaScore;
        m_totalBravoScore = m_totalBravoScore + currentBravoScore;
    }

    public void UpdateText()
    {
        var currentAlphaScore = PointManager.AlphaRoundScore[RoundManager.CurrentRound];
        var currentBravoScore = PointManager.BravoRoundScore[RoundManager.CurrentRound];
        SetTotalScore(currentAlphaScore, currentBravoScore);
        SetScoreText(currentAlphaScore, currentBravoScore);
    }
}