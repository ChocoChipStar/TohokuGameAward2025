using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmManager : MonoBehaviour
{
    [SerializeField]
    private float m_confirmTime = 0.0f;

    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private FadeManager m_fadeManager = null;

    [SerializeField]
    private TextMeshProUGUI m_timerText = null;

    private float m_elapsedTimer = 0.0f;

    private bool m_isStop = false;

    private void Update()
    {
        if(!m_fadeManager.IsFinishFadeIn)
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
            m_sceneChanger.TransitionNextScene();
            m_elapsedTimer = 0.0f;
            m_isStop = true;
        }
    }
}
