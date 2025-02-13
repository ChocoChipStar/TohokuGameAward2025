using TMPro;
using UnityEngine;

public class ResultAnimationController : MonoBehaviour
{
    [SerializeField]
    Animator m_animator = null;

    [SerializeField]
    Animator m_TextAnim = null;

    [SerializeField]
    Animator[] m_playerAnimator = null;

    [SerializeField]
    TextMeshProUGUI m_winnerScoreText = null;

    [SerializeField]
    TextMeshProUGUI m_loserScoreText = null;

    [SerializeField]
    Vector2 m_alphaWinTextPos = Vector2.zero;

    [SerializeField]
    Vector2 m_alphaLoseTextPos = Vector2.zero;

    [SerializeField]
    Vector2 m_bravoWinTextPos = Vector2.zero;

    [SerializeField]
    Vector2 m_bravoLoseTextPos = Vector2.zero;

    private int[] m_alphaScore = ScoreManager.AlphaRoundScore;
    private int[] m_bravoScore = ScoreManager.BravoRoundScore;
    private int m_alphaTotalScore = 0;
    private int m_bravoTotalScore = 0;

    private void CulculateTotalScore()
    {
        m_alphaScore = ScoreManager.AlphaRoundScore;
        m_bravoScore = ScoreManager.BravoRoundScore;
        m_alphaTotalScore = 0;
        m_bravoTotalScore = 0;

        if(m_alphaTotalScore == 0 && m_bravoTotalScore == 0)
        {
            int random = Random.Range(0, 2);
            if(random == 1)
            {
                m_alphaTotalScore += 10;
            }
            else
            {
                m_bravoTotalScore += 10;
            }
        }
    }

    private void SetWinnerAnimation()
    {
        CulculateTotalScore();

        if (m_alphaTotalScore > m_bravoTotalScore)
        {
            m_animator.SetBool("isAlphaWin", true);
            m_animator.SetBool("isBlaboWin", false);
        }

        if (m_alphaTotalScore < m_bravoTotalScore)
        {
            m_animator.SetBool("isAlphaWin", false);
            m_animator.SetBool("isBlaboWin", true);
        }

    }

    public void SetScoreText()
    {

        if (m_alphaTotalScore > m_bravoTotalScore)
        {
            m_winnerScoreText.text = m_alphaTotalScore.ToString();
            m_loserScoreText.text = m_bravoTotalScore.ToString();

            m_winnerScoreText.rectTransform.anchoredPosition = m_alphaWinTextPos;
            m_loserScoreText.rectTransform.anchoredPosition = m_bravoLoseTextPos;
        }

        if (m_alphaTotalScore < m_bravoTotalScore)
        {
            m_winnerScoreText.text = m_bravoTotalScore.ToString();
            m_loserScoreText.text = m_alphaTotalScore.ToString();

            m_winnerScoreText.rectTransform.anchoredPosition = m_bravoWinTextPos;
            m_loserScoreText.rectTransform.anchoredPosition = m_alphaLoseTextPos;
        }

    }

    private void SetWinnerPlayerAnimation()
    {
          if (m_alphaTotalScore > m_bravoTotalScore)
          {
          m_playerAnimator[0].SetBool("isWinner", true);
          m_playerAnimator[0].SetBool("isLoser", false);
          m_playerAnimator[2].SetBool("isWinner", true);
          m_playerAnimator[2].SetBool("isLoser", false);

          }
          if (m_alphaTotalScore < m_bravoTotalScore)
          {
          m_playerAnimator[1].SetBool("isWinner", true);
          m_playerAnimator[1].SetBool("isLoser", false);
          m_playerAnimator[3].SetBool("isWinner", true);
          m_playerAnimator[3].SetBool("isLoser", false);
          }
    }
    private void SetLoserPlayerAnimation()
    {
        if (m_alphaTotalScore > m_bravoTotalScore)
        {
            m_playerAnimator[1].SetBool("isWinner", false);
            m_playerAnimator[1].SetBool("isLoser", true);
            m_playerAnimator[3].SetBool("isWinner", false);
            m_playerAnimator[3].SetBool("isLoser", true);

        }
        if (m_alphaTotalScore < m_bravoTotalScore)
        {
            m_playerAnimator[0].SetBool("isWinner", false);
            m_playerAnimator[0].SetBool("isLoser", true);
            m_playerAnimator[2].SetBool("isWinner", false);
            m_playerAnimator[2].SetBool("isLoser", true);
        }
    }
    private void AnimationTextController()
    {
        m_TextAnim.enabled = true;
        if (m_alphaTotalScore > m_bravoTotalScore)
        {
            m_TextAnim.SetBool("Alpha", true);
            m_TextAnim.SetBool("Bravo", false);

        }
        if (m_alphaTotalScore < m_bravoTotalScore)
        {
            m_TextAnim.SetBool("Bravo", true);
            m_TextAnim.SetBool("Alpha", false);
        }
    }
}
