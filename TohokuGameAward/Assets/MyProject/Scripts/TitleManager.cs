using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private FadeManager m_fadeManager = null;

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
                
            }
        }
    }
}
