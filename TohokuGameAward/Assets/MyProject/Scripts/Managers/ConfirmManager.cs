using TMPro;
using UnityEngine;

public class ConfirmManager : MonoBehaviour
{
    [SerializeField]
    private float m_confirmTime = 0.0f;

    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private ScreenFade m_screenFade = null;

    [SerializeField]
    private TextMeshProUGUI m_timerText = null;

    private float m_elapsedTimer = 0.0f;

    private bool m_isStop = false;

    private void Update()
    {
        if(!m_screenFade.IsFinishFadeIn)
        {
            return;
        }

        if(m_isStop)
        {
            return;
        }

        m_elapsedTimer += Time.deltaTime;
        m_timerText.SetText(((int)m_confirmTime - (int)m_elapsedTimer).ToString());
        if(m_elapsedTimer >= m_confirmTime)
        {
            m_sceneChanger.LoadNextScene();
            m_elapsedTimer = 0.0f;
            m_isStop = true;
        }
    }
}
