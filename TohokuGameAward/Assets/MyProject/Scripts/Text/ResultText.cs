using TMPro;
using UnityEngine;

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

    private int m_defTotalScore = 0;

    private int m_offTotalScore = 0;

    private int[] m_deffencesScore = null;

    private int[] m_offencesScore = null;

    // Start is called before the first frame update
    void Start()
    {
        GetFinalScore();
    }

    // Update is called once per frame
    void Update()
    {
        DrawResult();
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
    private void DrawResult()
    {
        m_defeTotalText.text = m_defTotalScore.ToString();
        m_offeTotalText.text = m_offTotalScore.ToString();

        for(int i = 0;i < m_deffencesScore.Length;i++)
        {
            m_defeScoreText[i].text = m_deffencesScore[i].ToString();
        }
        for (int i = 0; i < m_offencesScore.Length; i++)
        {
            m_offeScoreText[i].text = m_offencesScore[i].ToString();
        }
    }


}
