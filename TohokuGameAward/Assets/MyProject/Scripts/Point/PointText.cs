using TMPro;
using UnityEngine;

public class PointText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_defenceTextGUI = null;

    [SerializeField]
    private TextMeshProUGUI m_offencesTextGUI = null;

    [SerializeField]
    PlayerManager m_playerManager = null;

    [SerializeField]
    private int[] m_score = null;

    PointManager m_pointManager = null;

    private int m_defencesScore = 0;

    private int m_offencesScore = 0;

    void Start()
    {
        m_pointManager = GetComponent<PointManager>();
    }

    void Update()
    {
        DrawPoint();
    }

    void DrawPoint()
    {
        m_defencesScore = m_pointManager.DefencesScore;
        m_offencesScore = m_pointManager.OffencesScore;
        m_defenceTextGUI.text = m_defencesScore.ToString();
        m_offencesTextGUI.text = m_offencesScore.ToString();
    }
}