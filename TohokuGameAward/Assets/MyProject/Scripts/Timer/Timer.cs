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

    [SerializeField]
    private  int m_finalRound = 0;

    private bool m_isTimeLimit = false;

    private static int m_round = 0;

    private bool m_isAddRound = false;

    public bool IsTimeLimit { get { return m_isTimeLimit; } }

    public  int FinalRound { get { return m_finalRound; } }

    public static int Round { get { return m_round; } }

    private void Update()
    {
        ShowCountDown();

        if (m_isTimeLimit && m_round < m_finalRound)
        {
            RenderTimeLimitText();
            LoadNewRound();
        }
        if(m_isTimeLimit && m_round >= m_finalRound)
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

    private void LoadNewRound()
    {
        m_sceneChangeDelay -= Time.deltaTime;

        if (m_sceneChangeDelay < 0)
        {
            AddRound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void LoadResultScene()
    {
        m_sceneChangeDelay -= Time.deltaTime;
        if (m_sceneChangeDelay < 0)
        {
            AddRound();
            SceneManager.LoadScene("ResultScene");
        }
    }

    private void AddRound()
    {
        if(m_isAddRound)
        {
            return;
        }    

        m_round += 1;
        m_isAddRound = true;
    }
}
