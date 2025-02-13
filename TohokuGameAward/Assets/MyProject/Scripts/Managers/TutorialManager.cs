using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private DrawTutorialClip m_drawTutorialClip = null;

    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private ScreenFade m_screenFade = null;

    [SerializeField]
    private SoundEffectManager m_soundEffectManager = null;

    private bool m_isGetStart = false;

    private void Update()
    {
        if (!m_screenFade.IsFinishFadeIn)
        {
            return;
        }

        var padCount = Gamepad.all.Count;    
        for (int i = 0; i < padCount; i++)
        {
            if (m_drawTutorialClip.IsIndexMax && !m_isGetStart)
            {
                if(m_inputData.WasPressedMenuInteractionInput(InputData.MenuInteractionInput.Decision,i))
                {
                    m_isGetStart = true;
                    m_soundEffectManager.OnPlayOneShot((int)SoundEffectManager.MainScenePattern.Death);
                    m_sceneChanger.LoadNextScene();
                    
                    return;
                }
            }
            
            if (m_inputData.WasPressedMenuInteractionInput(InputData.MenuInteractionInput.SwitchRight, i))
            {
                m_drawTutorialClip.SwitchNextImage();
                continue;
            }

            if (m_inputData.WasPressedMenuInteractionInput(InputData.MenuInteractionInput.SwitchLeft, i))
            {
                m_drawTutorialClip.SwitchPreviousImage();
                continue;
            }
        }
    }

    
}
