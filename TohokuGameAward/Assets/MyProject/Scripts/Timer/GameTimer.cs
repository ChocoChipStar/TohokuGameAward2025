using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_timerTextField = null;

    [SerializeField]
    private RoundManager m_roundManager = null;

    [SerializeField]
    private float m_limitTime = 0.0f;

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
        m_limitTime -= Time.deltaTime;

        if(m_limitTime < 0)
        {
            m_limitTime = 0;
            m_isTimeLimit = true;
        }

        m_timerTextField.text = m_limitTime.ToString("F0");//整数で表示
    }
}
