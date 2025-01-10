using TMPro;
using UnityEngine;

public class ResultText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_defenceTextGUI = null;

    [SerializeField]
    private TextMeshProUGUI m_offencesTextGUI = null;

    private int m_defencesScore = 0;

    private int m_offencesScore = 0;

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
        m_defencesScore = PointManager.FinalDefScore;
        m_offencesScore = PointManager.FinalOffScore;
    }
    private void DrawResult()
    {
        m_defenceTextGUI.text = m_defencesScore.ToString();
        m_offencesTextGUI.text = m_offencesScore.ToString();
    }
}
