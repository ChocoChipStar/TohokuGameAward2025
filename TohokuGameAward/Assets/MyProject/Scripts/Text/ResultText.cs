using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{
    [SerializeField]
    private ResultTextData m_effectData = null;
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

    IEnumerator Start()
    {
        GetFinalScore();

        for (int i = 0; i < (int)RoundManager.RoundState.Max; i++)
        { //ラウンドごとに時間をあけてスコアを表示
            yield return StartCoroutine(RenderScoreAfterDelay(m_RenderingRound));
            yield return new WaitForSeconds(m_effectData.EffectData.WaitForNextRound[m_RenderingRound]);
            m_RenderingRound++;
        }

        yield return StartCoroutine(RenderTotalAfterDelay());//トータルスコア表示

        yield return StartCoroutine(RenderWinnerAfterDelay());//勝者側のテクスチャを表示

        yield return StartCoroutine(SetResultEndFlug());
    }

    private void GetFinalScore()
    {
        m_alphaScore = PointManager.AlphaRoundScore;
        m_bravoScore = PointManager.BravoRoundScore;
        m_alphaTotalScore = TotalScore(m_alphaScore);
        m_bravoTotalScore = TotalScore(m_bravoScore);
    }

    IEnumerator RenderScoreAfterDelay(int Round)
    {
        float revealDelay = m_effectData.EffectData.RevealDelay[Round];

        while (revealDelay > 0)
        {
            revealDelay -= Time.deltaTime;
            m_alphaScoreText[Round].text = MakeRandomNum();
            m_bravoScoreText[Round].text = MakeRandomNum();
            yield return null;
        }
        m_alphaScoreText[Round].text = m_alphaScore[Round].ToString();
        m_bravoScoreText[Round].text = m_bravoScore[Round].ToString();
    }

    IEnumerator RenderTotalAfterDelay()
    {
        float revealDelay = m_effectData.EffectData.RevealDelay[m_RenderingRound];

        while (revealDelay > 0)
        {
            revealDelay -= Time.deltaTime;
            m_bravoTotalText.text = MakeRandomNum();
            m_alphaTotalText.text = MakeRandomNum();
            yield return null;
        }
        m_bravoTotalText.text = m_bravoTotalScore.ToString();
        m_alphaTotalText.text = m_alphaTotalScore.ToString();
    }
    private string MakeRandomNum()
    {
        int random;
        random = Random.Range(m_effectData.ParamsData.Min,m_effectData.ParamsData.Max);
        return random.ToString();
    }

    IEnumerator RenderWinnerAfterDelay()
    {
        yield return new WaitForSeconds(m_effectData.EffectData.winnerTextureDelay);
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

    IEnumerator SetResultEndFlug()
    {
        m_isResultEnded = true;
        yield return null;
    }

}
