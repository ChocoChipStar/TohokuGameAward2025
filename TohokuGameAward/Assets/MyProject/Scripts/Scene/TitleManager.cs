using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private SoundEffectManager m_soundEffectManager = null;

    private bool m_isGetStart = false;

    private void Update()
    {
        if(Gamepad.current == null)
        {
            return;
        }

        for(int i = 0; i < Gamepad.all.Count; i++)
        {
            var wasPressedStartButton = Gamepad.all[i].bButton.wasPressedThisFrame;
            if (wasPressedStartButton && !m_isGetStart)
            {
                m_isGetStart = true;
                m_soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.StageOut);
                m_sceneChanger.TransitionNextScene();
            }
        }
    }
}
