using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_timerTextField = null;

    [SerializeField]
    private TextMeshProUGUI m_limitTextField = null;

    [SerializeField]
    private float m_limitTime = 0.0f;

    [SerializeField]
    private string m_timeLimitText = null;

    private bool m_isTimeLimit = false;

    private void Update()
    {
        ShowCountDown();

        if (m_isTimeLimit)
        {
            RenderTimeLimitText();
        }
    }

    private void ShowCountDown()
    {
        m_limitTime -= Time.deltaTime;

        if(m_limitTime < 0)
        {
            m_limitTime = 0;
            m_isTimeLimit = true;
        }

        m_timerTextField.text = m_limitTime.ToString("F0");//整数で表示
    }

    private void RenderTimeLimitText()
    {
        m_limitTextField.text = m_timeLimitText;
    }
}
