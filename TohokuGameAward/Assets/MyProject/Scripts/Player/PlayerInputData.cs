using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputData : MonoBehaviour
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
        LeftRail
    }

    public Vector2 GetLeftStickValue(int playerNum)
    {
        return Gamepad.all[playerNum].leftStick.ReadValue();
    }

    public bool WasPressedButton(ActionsName actionNum, int playerNum)
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

            default:
                return false;
        }
    }

    public void SetNumber(int playerNum)
    {
        SelfNumber = playerNum;
    }
}
