using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_timerText = null;

    [SerializeField]
    float m_limitTime = 10;

    // Update is called once per frame
    void Update()
    {
        RenderTimer();
    }

    void RenderTimer()
    {
        m_limitTime -= Time.deltaTime;
        if (m_limitTime < 0)
        {
            m_limitTime = 0;
        }
        m_timerText.text = m_limitTime.ToString("F0");//整数で表示
    }

}
