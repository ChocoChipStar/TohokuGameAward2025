using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator = null;

    [SerializeField]
    private PlayerPickup m_playerPickup = null;

    private static readonly string[] StateNames = new string[] 
    { 
        "TopBodyState", "UnderBodyState"
    };

    /// <summary> 上半身アニメーションの現在状態を管理 </summary>
    private ReactiveProperty<TopState> m_currentTopState = new ReactiveProperty<TopState>(TopState.Idle);
    /// <summary> 下半身アニメーションの現状状態を管理 </summary>
    private ReactiveProperty<UnderState> m_currentUnderState = new ReactiveProperty<UnderState>(UnderState.Idle);

    /// <summary> 上半身アニメーションの遷移可能状態を管理 </summary>
    private Dictionary<TopState, bool> m_isTransferableTopState = new Dictionary<TopState, bool>()
    {
        { TopState.Idle,       false },
        { TopState.Move,       false },
        { TopState.Jump,       false },
        { TopState.Pickup,     false },
        { TopState.ThrowFront, false },
        { TopState.ThrowUpper, false },
        { TopState.ThrowUnder, false }
    };
    /// <summary> 下半身アニメーションの遷移可能状態を管理 </summary>
    private Dictionary<UnderState, bool> m_isTransferableUnderState = new Dictionary<UnderState, bool>()
    {
        { UnderState.Idle, false },
        { UnderState.Move, false },
        { UnderState.Jump, false }
    };

    /// <summary> 上半身アニメーションの優先度 </summary>
    private Dictionary<TopState, int> m_topStatePriority = new Dictionary<TopState, int>()
    {
        { TopState.Idle,       0 },
        { TopState.Move,       1 },
        { TopState.Jump,       2 },
        { TopState.Pickup,     3 },
        { TopState.ThrowFront, 4 },
        { TopState.ThrowUpper, 5 },
        { TopState.ThrowUnder, 6 }
    };
    /// <summary> 下半身アニメーションの優先度 </summary>
    private Dictionary<UnderState, int> m_underStatePriority = new Dictionary<UnderState, int>()
    {
        { UnderState.Idle, 0 },
        { UnderState.Move, 1 },
        { UnderState.Jump, 2 }
    };

    public enum StateType
    {
        Top,
        Under
    }
    public enum TopState
    {
        Idle,
        Move,
        Jump,
        Pickup,
        ThrowFront,
        ThrowUpper,
        ThrowUnder
    }
    public enum UnderState
    {
        Idle,
        Move,
        Jump
    }

    // intのステート実装するには難しい　状態管理しつつ、優先度を設定しアニメーションを終了が取れない
    private void Start()
    {
        m_currentTopState.Subscribe(SwitchTopState).AddTo(this);
        m_currentUnderState.Subscribe(SwitchUnderState).AddTo(this);
    }

    private void Update()
    {
        ChangeTopState(TopState.Idle);
        ChangeUnderState(UnderState.Idle);

        m_animator.SetBool("IsMirror", m_playerPickup.GetDirection());
    }

    /// <summary>
    /// 上半身が次のアニメーションに遷移出来るか確認します
    /// </summary>
    /// <returns> true->遷移可能 false->遷移不能 </returns>
    private bool CanChangeTopState(TopState newState)
    {
        if (m_topStatePriority[newState] >= m_topStatePriority[m_currentTopState.Value])
        {
            return true; // 今のより優先度の高いアニメーションなら
        }
        
        if (!m_isTransferableTopState[m_currentTopState.Value])
        {
            return true; // 優先度の低いアニメーションでも再生が終了していれば
        }

        return false;
    }

    /// <summary>
    /// 下半身が次のアニメーションに遷移出来るか確認します
    /// </summary>
    /// <returns> true->遷移可能 false->遷移不能 </returns>
    private bool CanChangeUnderState(UnderState newState)
    {
        if (m_underStatePriority[newState] >= m_underStatePriority[m_currentUnderState.Value])
        {
            return true; // 今のより優先度の高いアニメーションなら
        }

        if (!m_isTransferableUnderState[m_currentUnderState.Value])
        {
            return true; // 優先度の低いアニメーションでも再生が終了していれば
        }

        return false;
    }

    private void SwitchTopState(TopState state)
    {
        switch (state)
        {
            case TopState.Idle:
                m_animator.SetInteger(StateNames[(int)StateType.Top], (int)TopState.Idle);
                break;

            case TopState.Move:
                m_animator.SetInteger(StateNames[(int)StateType.Top], (int)TopState.Move);
                break;

            case TopState.Jump:
                m_animator.SetInteger(StateNames[(int)StateType.Top], (int)TopState.Jump);
                break;

            case TopState.Pickup:
                m_animator.SetInteger(StateNames[(int)StateType.Top], (int)TopState.Pickup);
                break;

            case TopState.ThrowFront:
                m_animator.SetInteger(StateNames[(int)StateType.Top], (int)TopState.ThrowFront);
                break;

            case TopState.ThrowUpper:
                m_animator.SetInteger(StateNames[(int)StateType.Top], (int)TopState.ThrowUpper);
                break;

            case TopState.ThrowUnder:
                m_animator.SetInteger(StateNames[(int)StateType.Top], (int)TopState.ThrowUnder);
                break;

            default:
                break;
        }
    }

    private void SwitchUnderState(UnderState state)
    {
        switch (state)
        {
            case UnderState.Idle:
                m_animator.SetInteger(StateNames[(int)StateType.Under], (int)UnderState.Idle);
                break;

            case UnderState.Move:
                m_animator.SetInteger(StateNames[(int)StateType.Under], (int)UnderState.Move);
                break;

            case UnderState.Jump:
                m_animator.SetInteger(StateNames[(int)StateType.Under], (int)UnderState.Jump);
                break;
        }
    }

    private void InitializeTransferableTopState(TopState newState)
    {
        m_isTransferableTopState[m_currentTopState.Value] = false;
        m_isTransferableTopState[newState] = true;
    }

    private void InitializeTransferableUnderState(UnderState newState)
    {
        m_isTransferableUnderState[m_currentUnderState.Value] = false;
        m_isTransferableUnderState[newState] = true;
    }

    /// <summary>
    /// 引数指定に応じて上半身のアニメーションを変更します
    /// </summary>
    public void ChangeTopState(TopState newState)
    {
        if(CanChangeTopState(newState))
        {
            InitializeTransferableTopState(newState);
            m_currentTopState.Value = newState;
        }
    }

    /// <summary>
    /// 引数指定に応じて下半身のアニメーションを変更します
    /// </summary>
    public void ChangeUnderState(UnderState newState)
    {
        if(CanChangeUnderState(newState))
        {
            InitializeTransferableUnderState(newState);
            m_currentUnderState.Value = newState;
        }
    }

    /// <summary>
    /// 次アニメーションに遷移可能な状態にします（ループアニメーションのみ）
    /// </summary>
    public void TransferableState(TopState? top = null, UnderState? under = null)
    {
        if (top.HasValue)
        {
            m_isTransferableTopState[top.Value] = false;
        }

        if(under.HasValue)
        {
            m_isTransferableUnderState[under.Value] = false;
        }
    }
}
