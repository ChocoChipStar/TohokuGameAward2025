using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField]
    private float m_sceneChangeDelay = 0.0f;

    private bool m_isTimeLimit = false;

    public bool IsTimeLimit { get { return m_isTimeLimit; } } 

    private void Update()
    {
        ShowCountDown();

        if (m_isTimeLimit)
        {
            RenderTimeLimitText();
            LoadResultScene();
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

    private void LoadResultScene()
    {
        m_sceneChangeDelay -= Time.deltaTime;
        if (m_sceneChangeDelay < 0)
        { SceneManager.LoadScene("ResultScene"); }
    }
}
