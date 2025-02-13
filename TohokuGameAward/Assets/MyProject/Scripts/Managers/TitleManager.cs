using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private SoundFade m_soundFade = null;

    [SerializeField]
    private Animator m_animator = null;

    [SerializeField]
    private InputData m_inputData = null;

    private bool m_isGetStart = false;

    private bool m_isFinished = false;

    private void Update()
    {
        if (m_isFinished && !m_isGetStart)
        {
            m_isGetStart = true;
            GoNextScene();
        }

        if (Gamepad.current == null)
        {
            return;
        }

        if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Title_Idle"))
        {
            return;
        }

        for(int i = 0; i < Gamepad.all.Count; i++)
        {
            if (m_inputData.WasPressedMenuInteractionInput(InputData.MenuInteractionInput.Decision,i))
            {
                SoundEffectManager.Instance.OnPlayOneShot((int)SoundEffectManager.TitleScenePattern.Start);
                m_soundFade.OnPlayFade();
                m_animator.SetBool("wasPressedStartBottom", true);
            }
        }
    }

    private void GoNextScene()
    {
        m_isGetStart = true;
        m_sceneChanger.LoadNextScene();
    }

    public void SetIsFinished()
    {
        m_isFinished = true;
    }
}
