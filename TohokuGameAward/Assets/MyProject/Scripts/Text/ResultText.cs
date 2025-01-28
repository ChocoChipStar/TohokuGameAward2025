using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{
    [SerializeField]
    private ResultTextData m_resultTextData = null;
    [SerializeField]
    private TextMeshProUGUI m_alphaTotalText = null;

    [SerializeField]
    private TextMeshProUGUI m_bravoTotalText = null;

    [SerializeField]
    private TextMeshProUGUI[] m_alphaScoreText = null;

    [SerializeField]
    private TextMeshProUGUI[] m_bravoScoreText = null;

    [SerializeField]
    private Image m_winnerTexture = null;

    [SerializeField]
    private Sprite m_bravoWinSprite = null;

    [SerializeField]
    private Sprite m_alphaWinSprite = null;

    private int[] m_alphaScore = new int[(int)RoundManager.RoundState.Max];

    private int[] m_bravoScore = new int[(int)RoundManager.RoundState.Max];

    private int m_alphaTotalScore = 0;

    private int m_bravoTotalScore = 0;

    private int m_RenderingRound = 0;

    private bool m_isResultEnded = false;

    public bool IsResultEnded { get { return m_isResultEnded; } }

    private IEnumerator Start()
    {
        GetFinalScore();

        for (int i = 0; i < (int)RoundManager.RoundState.Max; i++)
        { //ラウンドごとに時間をあけてスコアを表示
            yield return StartCoroutine(RenderScoreAfterDelay());
            yield return new WaitForSeconds(m_resultTextData.Effect.WaitForNextRound[m_RenderingRound]);
            m_RenderingRound++;
        }

        yield return StartCoroutine(RenderTotalAfterDelay());//トータルスコア表示

        yield return StartCoroutine(RenderWinnerAfterDelay());//勝者側のテクスチャを表示

        m_isResultEnded = true;
    }

    private void GetFinalScore()
    {
        m_alphaScore = PointManager.AlphaRoundScore;
        m_bravoScore = PointManager.BravoRoundScore;
        m_alphaTotalScore = TotalScore(m_alphaScore);
        m_bravoTotalScore = TotalScore(m_bravoScore);
    }

    private IEnumerator RenderScoreAfterDelay()
    {
        yield return ScoreShuffler(m_resultTextData.Effect.RevealDelay[m_RenderingRound]);

        m_alphaScoreText[m_RenderingRound].text = m_alphaScore[m_RenderingRound].ToString();
        m_bravoScoreText[m_RenderingRound].text = m_bravoScore[m_RenderingRound].ToString();
    }

    private IEnumerator RenderTotalAfterDelay()
    {
        yield return ScoreShuffler(m_resultTextData.Effect.RevealDelay[m_RenderingRound]);

        m_bravoTotalText.text = m_bravoTotalScore.ToString();
        m_alphaTotalText.text = m_alphaTotalScore.ToString();
    }

    private IEnumerator ScoreShuffler(float revealDelay)
    {
        while (revealDelay > 0)
        {
            revealDelay -= Time.deltaTime;
            m_alphaScoreText[m_RenderingRound].text = MakeRandomNum();
            m_bravoScoreText[m_RenderingRound].text = MakeRandomNum();
            yield return null;
        }
    }

    private string MakeRandomNum()
    {
        return Random.Range(m_resultTextData.Params.ShaffuleValueMin, m_resultTextData.Params.ShaffuleValueMax).ToString();
    }

    private IEnumerator RenderWinnerAfterDelay()
    {
        yield return new WaitForSeconds(m_resultTextData.Effect.winnerTextureDelay);
        if (m_alphaTotalScore > m_bravoTotalScore)
        {
            m_winnerTexture.sprite = m_alphaWinSprite;
        }
        else
        {
            m_winnerTexture.sprite = m_bravoWinSprite;
        }
        m_winnerTexture.gameObject.SetActive(true);
    }

    private int TotalScore(int[] Score)
    {
        int TotalScore = 0;
        for(int i = 0;i < Score.Length;i++)
        {
            TotalScore += Score[i];
        }
        return TotalScore;
    }
}
