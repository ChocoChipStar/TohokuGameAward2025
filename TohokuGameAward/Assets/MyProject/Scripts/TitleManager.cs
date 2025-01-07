using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    private void Update()
    {
        if(Gamepad.current == null)
        {
            return;
        }

        for(int i = 0; i < Gamepad.all.Count; i++)
        {
            var wasPressedStartButton = Gamepad.all[i].bButton.wasPressedThisFrame;
            if (wasPressedStartButton)
            {
                m_sceneChanger.TransitionScene(SceneChanger.SceneName.Tutorial);
            }
        }
    }
}
