using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawResultScoreText : MonoBehaviour
{
    [SerializeField]
    private ResultTextData m_resultTextData = null;

    [SerializeField]
    private TextMeshProUGUI[] m_alphaScoreText = null;
    [SerializeField]
    private TextMeshProUGUI[] m_bravoScoreText = null;

    [SerializeField]
    private TextMeshProUGUI m_alphaTotalScoreText = null;
    [SerializeField]
    private TextMeshProUGUI m_bravoTotalScoreText = null;

    [SerializeField]
    private Image m_winnerImage = null;
    [SerializeField]
    private Image m_drawImage = null;

    [SerializeField]
    private Sprite m_alphaWinnerSprite = null;
    [SerializeField]
    private Sprite m_bravoWinnerSprite = null;
    [SerializeField]
    private Sprite m_drawSprite = null;

    private int m_alphaTotalScore = 0;
    private int m_bravoTotalScore = 0;

    private int m_currentDrawRound = 0;

    private bool m_isResultEnded = false;

    private int[] m_alphaScore = new int[(int)RoundManager.RoundState.Max];
    private int[] m_bravoScore = new int[(int)RoundManager.RoundState.Max];
    public bool IsResultEnded { get { return m_isResultEnded; } private set { m_isResultEnded = value; } }

    private void Start()
    {
        InitializeScore();
        StartCoroutine(DrawText());
    }

    /// <summary>
    /// メインシーンで結果出たスコアを変数に代入する処理を行います
    /// </summary>
    private void InitializeScore()
    {
        m_alphaScore = ScoreManager.AlphaRoundScore;
        m_bravoScore = ScoreManager.BravoRoundScore;
        m_alphaTotalScore = TotalScore(m_alphaScore);
        m_bravoTotalScore = TotalScore(m_bravoScore);
    }

    private IEnumerator DrawText()
    {
        yield return StartCoroutine(DrawRoundScore());
        yield return StartCoroutine(DrawTotalScore());
        yield return StartCoroutine(DrawWinnerUI());

        IsResultEnded = true;
    }

    /// <summary>
    /// スコアをシャッフルする演出後にラウンドのスコアを両チーム表示させます
    /// </summary>
    private IEnumerator DrawRoundScore()
    {
        for(int i = 0; i < (int)RoundManager.RoundState.Max; i++)
        {
            yield return DrawScoreShuffle(m_resultTextData.Params.ScoreShuffleTime[i]);

            m_alphaScoreText[i].SetText(m_alphaScore[i].ToString());
            m_bravoScoreText[i].SetText(m_bravoScore[i].ToString());

            yield return new WaitForSeconds(m_resultTextData.Params.NextRoundDelayTime[i]);
        }
    }

    /// <summary>
    /// スコアをシャッフルする演出後に合計スコアを両チーム表示させます
    /// </summary>
    private IEnumerator DrawTotalScore()
    {
        yield return DrawScoreShuffle(m_resultTextData.Params.ScoreShuffleTime[m_currentDrawRound]);

        m_bravoTotalScoreText.SetText(m_bravoTotalScore.ToString());
        m_alphaTotalScoreText.SetText(m_alphaTotalScore.ToString());
    }

    /// <summary>
    /// 表示されているスコアをシャッフルする演出を行います
    /// </summary>
    private IEnumerator DrawScoreShuffle(float shuffleTime)
    {
        while (shuffleTime > 0.0f)
        {
            shuffleTime += -Time.deltaTime;
            m_alphaScoreText[m_currentDrawRound].SetText(CreateRandomValue().ToString());
            m_bravoScoreText[m_currentDrawRound].SetText(CreateRandomValue().ToString());
            yield return null;
        }
        m_currentDrawRound++;
    }

    /// <summary>
    /// スコアシャッフル中の乱数を作る処理を行います
    /// </summary>
    /// <returns> 作った乱数を返します </returns>
    private int CreateRandomValue()
    {
        return Random.Range(m_resultTextData.Params.ShuffleValueMin, m_resultTextData.Params.ShuffleValueMax);
    }

    private IEnumerator DrawWinnerUI()
    {
        yield return new WaitForSeconds(m_resultTextData.Params.DrawWinnerUIDelayTime);
        if (m_alphaTotalScore > m_bravoTotalScore)
        {
            m_winnerImage.sprite = m_alphaWinnerSprite;
            m_winnerImage.gameObject.SetActive(true);
        }
        if(m_bravoTotalScore > m_alphaTotalScore)
        {
            m_winnerImage.sprite = m_bravoWinnerSprite;
            m_winnerImage.gameObject.SetActive(true);
        }

        if(m_alphaTotalScore == m_bravoTotalScore)
        {
            m_drawImage.sprite = m_drawSprite;
            m_drawImage.gameObject.SetActive(true);
        }
 
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
