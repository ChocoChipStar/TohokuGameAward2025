using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

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
    private float WaitForSecondsOfRender = 0;

    [SerializeField]
    private float WaitForSecondsOfEffect = 0;

    [SerializeField]
    int effectIncrement = 0;

    private int m_defTotalScore = 0;

    private int m_offTotalScore = 0;

    private int[] m_deffencesScore = null;

    private int[] m_offencesScore = null;

    private bool[] m_appeared = null;

    private bool m_isResultEnded = false;

    public bool IsResultEnded { get { return m_isResultEnded; } }

    IEnumerator Start()
    {
        m_appeared = new bool[2];
        GetFinalScore();
        yield return StartCoroutine(RenderRoundScore());
        yield return StartCoroutine(TotalScoreTeamOne());
        yield return new WaitForSeconds(WaitForSecondsOfRender);
        yield return StartCoroutine(TotalScoreTeamTwo());
    }

    private void GetFinalScore()
    {
        m_deffencesScore = PointManager.DefRoundScore;
        m_offencesScore = PointManager.OffRoundScore;
        m_defTotalScore = TotalScore(m_deffencesScore);
        m_offTotalScore = TotalScore(m_offencesScore);
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

    IEnumerator RenderRoundScore()
    {
        for (int i = 0; i < m_deffencesScore.Length; i++)
        {
            m_defeScoreText[i].text = m_deffencesScore[i].ToString();
            m_offeScoreText[i].text = m_offencesScore[i].ToString();
            yield return new WaitForSeconds(WaitForSecondsOfRender);
        }
    }
   IEnumerator TotalScoreTeamOne()
   {
       int effectNumber = 0;
       while (effectNumber < m_defTotalScore - effectIncrement)
       {
           effectNumber += effectIncrement;
           m_defeTotalText.text = effectNumber.ToString();
            yield return new WaitForSeconds(WaitForSecondsOfEffect);
       }
       m_appeared[0] = true; //m_appeared チームごとに「トータルスコアが表示されているか」を管理している。
       if (m_appeared[0])
       {
           m_defeTotalText.text = m_defTotalScore.ToString(); 
       }
   }

    IEnumerator TotalScoreTeamTwo()
    {
        int effectNumber = 0;
        while (effectNumber < m_offTotalScore - effectIncrement)
        {
            effectNumber += effectIncrement;
            m_offeTotalText.text = effectNumber.ToString();
            yield return new WaitForSeconds(WaitForSecondsOfEffect);
        }
        m_appeared[1] = true; //m_appeared チームごとに「トータルスコアが表示されているか」を管理している。
        if (m_appeared[1])
        {
            m_offeTotalText.text = m_offTotalScore.ToString();
        }
        m_isResultEnded = true;
    }
}
