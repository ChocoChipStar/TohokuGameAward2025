using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DebugHUD : MonoBehaviour
{
    [SerializeField]
    private GameObject m_menuUI = null;

    [SerializeField]
    private Image[] m_paramsImage = new Image[(int)SelectedMenu.Max];

    [SerializeField]
    private Toggle[] m_paramsToggle = new Toggle[(int)SelectedMenu.Max];

    [SerializeField]
    private Image[] m_grayOutImage = new Image[(int)SelectedMenu.Max];

    private int m_currentSelect = 0;
    private int m_connectedPadCount = 0;

    private bool m_isMenu = false;

    private bool m_isEnabledPlayerParams = false;
    private bool m_isEnabledBombParams = false;
    private bool m_isEnabledExplosionParams = false;

    private SelectedMenu m_lastFrameSelected;

    private enum SelectedMenu
    {
        Player,
        Bomb,
        Explosion,
        Max
    }
    private enum ActionsName
    {
        Up,
        Down,
        Start,
        Selected
    }

    private SelectedMenu[] m_selectedMenus =
    {
        SelectedMenu.Player,
        SelectedMenu.Bomb,
        SelectedMenu.Explosion,
        SelectedMenu.Max
    };

    private void Start()
    {
        m_connectedPadCount = Gamepad.all.Count;
    }

    private void Update()
    {
        for(int i = 0; i < m_connectedPadCount; i++)
        {
            UpdateMenuState(i);
        }
    }

    private void UpdateMenuState(int playerNum)
    {
        if (WasPressedButton(ActionsName.Start, playerNum))
        {
            m_isMenu = !m_isMenu;
            m_menuUI.SetActive(m_isMenu);
        }

        if (!m_isMenu)
        {
            return;
        }

        if (WasPressedButton(ActionsName.Selected,playerNum))
        {
            SwitchParamsMenu(m_selectedMenus[m_currentSelect]);
        }

        if (WasPressedButton(ActionsName.Up, playerNum))
        {
            m_currentSelect--;
            SwitchSelectMenu(m_currentSelect);
        }
        else if (WasPressedButton(ActionsName.Down, playerNum))
        {
            m_currentSelect++;
            SwitchSelectMenu(m_currentSelect);
        }
    }

    private void SwitchSelectToggle(SelectedMenu state)
    {
        switch(state)
        {
            case SelectedMenu.Player:
                SwitchGrayOutMenu(SelectedMenu.Player);
                break;

            case SelectedMenu.Bomb:
                SwitchGrayOutMenu(SelectedMenu.Bomb);
                break;

            case SelectedMenu.Explosion:
                SwitchGrayOutMenu(SelectedMenu.Explosion);
                break;
        }
    }

    private void SwitchParamsMenu(SelectedMenu currentState)
    {
        switch(currentState)
        {
            case SelectedMenu.Player:
                SwitchParamsMenuColor(currentState, ref m_isEnabledPlayerParams);
                return;

            case SelectedMenu.Bomb:
                SwitchParamsMenuColor(currentState, ref m_isEnabledBombParams);
                return;

            case SelectedMenu.Explosion:
                SwitchParamsMenuColor(currentState, ref m_isEnabledExplosionParams);
                return;

            default:
                return;
        }
    }

    private void SwitchParamsMenuColor(SelectedMenu currentSelect, ref bool isEnabled)
    {
        isEnabled = !isEnabled;
        if(isEnabled)
        {
            m_paramsToggle[(int)currentSelect].isOn = true;
            m_paramsImage[(int)currentSelect].color = Color.green;
            return;
        }

        m_paramsToggle[(int)currentSelect].isOn = false;
        m_paramsImage[(int)currentSelect].color = Color.white;
    }

    private void SwitchGrayOutMenu(SelectedMenu currentState)
    {
        m_grayOutImage[(int)currentState].enabled = true;
        m_grayOutImage[(int)m_lastFrameSelected].enabled = false;

        m_lastFrameSelected = currentState;
    }

    private void SwitchSelectMenu(int stateNum)
    {
        if (stateNum > (int)SelectedMenu.Max - 1)
        {
            stateNum = 0;
        }

        if(stateNum < 0)
        {
            stateNum = (int)SelectedMenu.Max - 1;
        }

        m_currentSelect = stateNum;
        SwitchSelectToggle(m_selectedMenus[m_currentSelect]);
    }

    private bool WasPressedButton(ActionsName state, int padNum)
    {
        var keyCurrent = Keyboard.current;
        
        switch (state)
        {
            case ActionsName.Up:
                return keyCurrent.upArrowKey.wasPressedThisFrame || WasPressedControllerButton(state, padNum);

            case ActionsName.Down:
                return keyCurrent.downArrowKey.wasPressedThisFrame || WasPressedControllerButton(state, padNum);

            case ActionsName.Start:
                return keyCurrent.escapeKey.wasPressedThisFrame || WasPressedControllerButton(state, padNum);

            case ActionsName.Selected:
                return keyCurrent.spaceKey.wasPressedThisFrame || WasPressedControllerButton(state, padNum);

            default:
                return false;
        }
    }

    private bool WasPressedControllerButton(ActionsName state, int padNum)
    {
        if(Gamepad.current == null)
        {
            return false;
        }

        var padCurrent = Gamepad.all[padNum];
        switch (state)
        {
            case ActionsName.Up:
                return padCurrent.dpad.up.wasPressedThisFrame;

            case ActionsName.Down:
                return padCurrent.dpad.down.wasPressedThisFrame;

            case ActionsName.Start:
                return padCurrent.startButton.wasPressedThisFrame;

            case ActionsName.Selected:
                return padCurrent.aButton.wasPressedThisFrame;

            default: 
                return false;
        }
    }
}
