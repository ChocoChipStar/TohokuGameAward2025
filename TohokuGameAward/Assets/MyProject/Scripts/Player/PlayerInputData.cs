using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputData : MonoBehaviour
{
    public static readonly float MovementDeadZoneRange = 0.2f;
    public static readonly float ThrowDeadZoneRange = 0.7f;

    public static readonly int PlayerMax = 4;

    public int SelfNumber { get; private set; } = 0;

    public enum InputButton
    {
        Jump,
        Attack,
        Throw
    }

    public Vector2 GetLeftStickValue(int playerNum)
    {
        return Gamepad.all[playerNum].leftStick.ReadValue();
    }

    public bool WasPressedButton(InputButton buttonNum, int playerNum)
    {
        switch(buttonNum)
        {
            case InputButton.Jump:
                return Gamepad.all[playerNum].aButton.wasPressedThisFrame;

            case InputButton.Attack:
                return Gamepad.all[playerNum].xButton.wasPressedThisFrame;

            case InputButton.Throw:
                return Gamepad.all[playerNum].rightTrigger.wasPressedThisFrame;

            default:
                return false;
        }
    }

    public void SetNumber(int playerNum)
    {
        SelfNumber = playerNum;
    }
}
