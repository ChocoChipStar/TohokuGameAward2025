using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator = null;

    private static readonly string[] UpperStateNames = new string[]
    {
        "Idle", "Move", "Jump", "PickupIdle", "PickupMove", "PickupJump"
    };

    public enum UpperState
    {
        Idle,
        Move,
        Jump,
        PickupIdle,
        PickupMove,
        PickupJump,
    }

    public enum LowerState
    {
        Idle,
        Move,
        Jump
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetUpperBodyState(UpperState.Idle);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetUpperBodyState(UpperState.Move);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetUpperBodyState(UpperState.Jump);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetUpperBodyState(UpperState.PickupIdle);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetUpperBodyState(UpperState.PickupMove);
        }
    }

    public void SetUpperBodyState(UpperState state)
    {
        int stateNum = (int)state;
        m_animator.SetInteger("UpperBodyState", stateNum);
    }
}
