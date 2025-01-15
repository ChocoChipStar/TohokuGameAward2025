using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private ImageChanger m_imageChanger = null;

    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private FadeManager m_fadeManager = null;

    private void Update()
    {
        if(!m_fadeManager.IsFinishFadeIn)
        {
            return;
        }

        if (Gamepad.current == null)
        {
            return;
        }

        var padCount = Gamepad.all.Count;
        for(int i = 0; i < padCount; i++)
        {
            if (m_imageChanger.IsIndexMax)
            {
                if(m_inputData.WasPressedUIButton(InputData.UserInterfaceName.Decision,i))
                {
                    m_sceneChanger.TransitionNextScene();
                    return;
                }
            }

            if (m_inputData.WasPressedUIButton(InputData.UserInterfaceName.SwitchRight, i))
            {
                m_imageChanger.SwitchNextImage();
                continue;
            }

            if(m_inputData.WasPressedUIButton(InputData.UserInterfaceName.SwitchLeft,i))
            {
                m_imageChanger.SwitchPreviousImage();
                continue;
            }
        }
    }

    
}
