using UnityEngine;
using UnityEngine.InputSystem;

public class InputData : MonoBehaviour
{
    public static readonly float MovementDeadZoneRange = 0.2f;
    public static readonly float ThrowDeadZoneRange = 0.7f;

    public static readonly int PlayerMax = 4;

    public int SelfNumber { get; private set; } = 0;

    public enum ActionsName
    {
        Jump,
        Attack,
        Throw,
        RightRail,
        LeftRail,
        Shoot
    }

    public enum UserInterfaceName
    {
        Decision,
        SwitchLeft,
        SwitchRight,
    }

    public Vector2 GetLeftStickValue(int playerNum)
    {
        return Gamepad.all[playerNum].leftStick.ReadValue();
    }

    public bool WasPressedActionButton(ActionsName actionNum, int playerNum)
    {
        switch(actionNum)
        {
            case ActionsName.Jump:
                return Gamepad.all[playerNum].aButton.wasPressedThisFrame;

            case ActionsName.Attack:
                return Gamepad.all[playerNum].xButton.wasPressedThisFrame;

            case ActionsName.Throw:
                return Gamepad.all[playerNum].rightTrigger.wasPressedThisFrame;

            case ActionsName.RightRail:
                return Gamepad.all[playerNum].rightShoulder.isPressed;

            case ActionsName.LeftRail:
                return Gamepad.all[playerNum].leftShoulder.isPressed;

            case ActionsName.Shoot:
                return Gamepad.all[playerNum].bButton.wasPressedThisFrame;

            default:
                return false;
        }
    }

    public bool WasPressedUIButton(UserInterfaceName uiNum, int index)
    {
        switch(uiNum)
        {
            case UserInterfaceName.Decision:
                return Gamepad.all[index].bButton.wasPressedThisFrame;

            case UserInterfaceName.SwitchLeft:
                return Gamepad.all[index].leftShoulder.wasPressedThisFrame;

            case UserInterfaceName.SwitchRight:
                return Gamepad.all[index].rightShoulder.wasPressedThisFrame;

            default:
                return false;
        }
    }

    public void SetNumber(int playerNum)
    {
        SelfNumber = playerNum;
    }
}
