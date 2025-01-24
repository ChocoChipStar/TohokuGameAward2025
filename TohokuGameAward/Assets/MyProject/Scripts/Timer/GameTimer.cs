using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_timerTextField = null;

    [SerializeField]
    private RoundManager m_roundManager = null;

    [SerializeField]
    private float m_timelimit = 0.0f;

    private bool m_isTimeLimit = false;

    public bool IsTimeLimit { get { return m_isTimeLimit; } }

    private void Update()
    {
        if(m_isTimeLimit || !m_roundManager.IsRoundStart)
        {
            return;
        }

        StartGameTimer();
    }

    private void StartGameTimer()
    {
        m_timelimit -= Time.deltaTime;

        if(m_timelimit < 0)
        {
            m_timelimit = 0;
            m_isTimeLimit = true;
        }

        m_timerTextField.text = m_timelimit.ToString("F0");//整数で表示
    }
}
