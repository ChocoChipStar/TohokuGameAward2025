using TMPro;
using UnityEngine;

public class PointText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_defenceTextGUI = null;

    [SerializeField]
    private TextMeshProUGUI m_offencesTextGUI = null;

    [SerializeField]
    private TextMeshProUGUI m_ofeTotalTextGUI = null;

    [SerializeField]
    private TextMeshProUGUI m_defeTotalTextGUI = null;

    [SerializeField]
    PlayerManager m_playerManager = null;

    [SerializeField]
    GameTimer m_timer = null;

    private int[] m_score = null;

    PointManager m_pointManager = null;

    private int[] m_deffRoundScore = null;

    private int[] m_offeRoundScore = null;

    private int m_currentDefeScore = 0;

    private int m_currentOffeScore = 0;

    private int m_defeTotalScore = 0;

    private int m_offeTotalScore = 0;

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
        m_deffRoundScore  = new int[(int)RoundManager.RoundState.Max];
        m_offeRoundScore  = new int[(int)RoundManager.RoundState.Max];
    }

    void GetLastRoundPoints()
    {
        m_deffRoundScore  = PointManager.DefRoundScore;
        m_offeRoundScore  = PointManager.OffRoundScore;
    }

    void DrawPoint()
    {
        GetPoint();

        m_defenceTextGUI.text  = m_currentDefeScore.ToString();
        m_offencesTextGUI.text = m_currentOffeScore.ToString();

        m_defeTotalTextGUI.text = m_defeTotalScore.ToString();
        m_ofeTotalTextGUI.text  = m_offeTotalScore.ToString();
    }

    void GetPoint()
    {
        if (RoundManager.CurrentRound == (int)RoundManager.RoundState.One)
        {
            m_currentDefeScore = PointManager.DefRoundScore[0];
            m_currentOffeScore = PointManager.OffRoundScore[0];

            m_defeTotalScore = m_currentDefeScore;
            m_offeTotalScore = m_currentOffeScore;

            return;
        }
        if (RoundManager.CurrentRound < (int)RoundManager.RoundState.Two)
        {
            m_currentDefeScore = PointManager.DefRoundScore[RoundManager.CurrentRound];
            m_currentOffeScore = PointManager.OffRoundScore[RoundManager.CurrentRound];

            m_defeTotalScore = m_pointManager.GetTotalScore(PointManager.DefRoundScore);
            m_offeTotalScore = m_pointManager.GetTotalScore(PointManager.OffRoundScore);
        }
    }
}