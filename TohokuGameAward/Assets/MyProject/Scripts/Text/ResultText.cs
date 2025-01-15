using TMPro;
using UnityEngine;
using System.Collections;

public class ResultText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_defeTotalText = null;

    [SerializeField]
    private TextMeshProUGUI m_offeTotalText = null;

    [SerializeField]
    private TextMeshProUGUI[] m_defeScoreText = null;

    [SerializeField]
    private TextMeshProUGUI[] m_offeScoreText = null;

    [SerializeField]
    private float[] m_waitForNextRender = null;

    [SerializeField]
    private float[] m_revealDelay = null;

    [SerializeField]
    private int m_randomMin = 0;

    [SerializeField]
    private int m_randomMax = 0;

    private int[] m_deffencesScore = null;

    private int[] m_offencesScore = null;

    private int m_defTotalScore = 0;

    private int m_offTotalScore = 0;

    private int m_RenderingRound = 0;

    private bool m_isResultEnded = false;

    public bool IsResultEnded { get { return m_isResultEnded; } }

    IEnumerator Start()
    {
        GetFinalScore();

        yield return StartCoroutine(RenderScoreAfterDelay(m_RenderingRound));
        yield return new WaitForSeconds(m_waitForNextRender[m_RenderingRound]);
        m_RenderingRound++;

        yield return StartCoroutine(RenderScoreAfterDelay(m_RenderingRound));
        yield return new WaitForSeconds(m_waitForNextRender[m_RenderingRound]);
        m_RenderingRound++;

        yield return StartCoroutine(RenderTotalAfterDelay());
        yield return StartCoroutine(SetResultEndFlig());
    }

    private void GetFinalScore()
    {
        m_deffencesScore = PointManager.DefRoundScore;
        m_offencesScore = PointManager.OffRoundScore;
        m_defTotalScore = TotalScore(m_deffencesScore);
        m_offTotalScore = TotalScore(m_offencesScore);
    }

    IEnumerator RenderScoreAfterDelay(int Round)
    {
        float revealDelay = m_revealDelay[Round];

        while (revealDelay > 0)
        {
            revealDelay -= Time.deltaTime;
            m_defeScoreText[Round].text = MakeRandomNum();
            m_offeScoreText[Round].text = MakeRandomNum();
            yield return null;
        }
        m_defeScoreText[Round].text = m_deffencesScore[Round].ToString();
        m_offeScoreText[Round].text = m_offencesScore[Round].ToString();
    }

    IEnumerator RenderTotalAfterDelay()
    {
        float revealDelay = m_revealDelay[m_RenderingRound];

        while (revealDelay > 0)
        {
            revealDelay -= Time.deltaTime;
            m_offeTotalText.text = MakeRandomNum();
            m_defeTotalText.text = MakeRandomNum();
            yield return null;
        }
        m_offeTotalText.text = m_defTotalScore.ToString();
        m_defeTotalText.text = m_offTotalScore.ToString();
    }
    private string MakeRandomNum()
    {
        int random;
        random = Random.Range(m_randomMin, m_randomMax);
        return random.ToString();
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

    IEnumerator SetResultEndFlig()
    {
        m_isResultEnded = true;
        yield return null;
    }
}
