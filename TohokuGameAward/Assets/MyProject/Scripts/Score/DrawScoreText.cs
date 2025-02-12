using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawScoreText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_alphaScoreText = null;
    [SerializeField]
    private TextMeshProUGUI m_bravoScoreText = null;

    private void SetScoreText(int currentAlphaScore, int currentBravoScore) 
    {
        m_alphaScoreText.SetText(currentAlphaScore.ToString());
        m_bravoScoreText.SetText(currentBravoScore.ToString());
    }

    public void UpdateText()
    {
        var currentAlphaScore = ScoreManager.AlphaRoundScore[RoundManager.CurrentRound];
        var currentBravoScore = ScoreManager.BravoRoundScore[RoundManager.CurrentRound];
        SetScoreText(currentAlphaScore, currentBravoScore);
    }

    public void SetDrawing(bool isEnabled)
    {
        m_alphaScoreText.enabled = isEnabled;
        m_bravoScoreText.enabled = isEnabled;
    }
}