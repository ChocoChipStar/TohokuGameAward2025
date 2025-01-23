using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator = null;

    //Script削除したためコメントアウト
    //[SerializeField]
    //private PlayerPickup m_playerPickup = null;

    private bool m_isMirror = false;
    private bool m_isCalledAnimationEvent = false;

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
        { TopState.Falling,    false },
        { TopState.Pickup,     false },
        { TopState.ThrowFront, false },
        { TopState.ThrowUpper, false },
        { TopState.ThrowUnder, false },
        { TopState.Blow,       false }
    };
    /// <summary> 下半身アニメーションの遷移可能状態を管理 </summary>
    private Dictionary<UnderState, bool> m_isTransferableUnderState = new Dictionary<UnderState, bool>()
    {
        { UnderState.Idle, false },
        { UnderState.Move, false },
        { UnderState.Jump, false },
        { UnderState.Falling, false },
        { UnderState.Blow, false }
    };

    /// <summary> 上半身アニメーションの優先度 </summary>
    private Dictionary<TopState, int> m_topStatePriority = new Dictionary<TopState, int>()
    {
        { TopState.Idle,       0 },
        { TopState.Move,       1 },
        { TopState.Jump,       2 },
        { TopState.Falling,    3 },
        { TopState.Pickup,     4 },
        { TopState.ThrowFront, 5 },
        { TopState.ThrowUpper, 6 },
        { TopState.ThrowUnder, 7 },
        { TopState.Blow,       8 }
    };
    /// <summary> 下半身アニメーションの優先度 </summary>
    private Dictionary<UnderState, int> m_underStatePriority = new Dictionary<UnderState, int>()
    {
        { UnderState.Idle,    0 },
        { UnderState.Move,    1 },
        { UnderState.Jump,    2 },
        { UnderState.Falling, 3 },
        { UnderState.Blow,    4 }
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
        Falling,
        Pickup,
        ThrowFront,
        ThrowUpper,
        ThrowUnder,
        Blow
    }
    public enum UnderState
    {
        Idle,
        Move,
        Jump,
        Falling,
        Blow
    }

    public bool IsFinishedThrowFirst { get; private set; } = false;
    public bool IsPlaybackThrow { get; private set; } = false;

    private void Start()
    {
        m_currentTopState.Subscribe(SwitchTopState).AddTo(this);
        m_currentUnderState.Subscribe(SwitchUnderState).AddTo(this);
    }

    private void Update()
    {
        ChangeTopState(TopState.Idle);
        ChangeUnderState(UnderState.Idle);
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

            case TopState.Falling:
                m_animator.SetInteger(StateNames[(int)StateType.Top], (int)TopState.Falling);
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

            case TopState.Blow:
                m_animator.SetInteger(StateNames[(int)StateType.Top], (int)TopState.Blow);
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

            case UnderState.Falling:
                m_animator.SetInteger(StateNames[(int)StateType.Under], (int)UnderState.Falling);
                break;

            case UnderState.Blow:
                m_animator.SetInteger(StateNames[(int)StateType.Under], (int)TopState.Blow);
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
        if (CanChangeTopState(newState))
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
        if (CanChangeUnderState(newState))
        {
            InitializeTransferableUnderState(newState);
            m_currentUnderState.Value = newState;
        }
    }

    /// <summary>
    /// アニメーションのミラーと通常を切り替える処理を行います
    /// </summary>
    public void SwitchMirroring()
    {
        m_animator.SetBool("IsMirror", m_isMirror);
        m_isMirror = !m_isMirror;
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

        if (under.HasValue)
        {
            m_isTransferableUnderState[under.Value] = false;
        }
    }

    /// <summary>
    /// アイテムを投げた後の初期化を行います
    /// </summary>
    public void InitializeThrowFirst()
    {
        IsFinishedThrowFirst = false;
        TransferableState(top: m_currentTopState.Value);
        TransferableState(under: m_currentUnderState.Value);
    }

    /// <summary>
    /// 投げアニメーション開始時イベントが呼び出されます
    /// </summary>
    public void StartThrowAnimation()
    {
        if (!m_isCalledAnimationEvent)
        {
            return;
        }
        m_isCalledAnimationEvent = false;

        IsPlaybackThrow = true;
    }

    /// <summary>
    /// 投げアニメーション終了時イベントが呼び出されます（フォロースルー含まない）
    /// </summary>
    public void FinishedThrowFirstAnimation()
    {
        if (!m_isCalledAnimationEvent)
        {
            return;
        }
        m_isCalledAnimationEvent = false;

        IsFinishedThrowFirst = true;
    }

    /// <summary>
    /// フォロースルーアニメーション終了時イベントが呼び出されます
    /// </summary>
    public void InitializeThrowAnimationState(TopState state)
    {
        if (!m_isCalledAnimationEvent)
        {
            return;
        }
        m_isCalledAnimationEvent = false;

        TransferableState(top: state);
        IsPlaybackThrow = false;
    }

    /// <summary>
    /// アニメーションイベントの呼び出し用関数です
    /// アニメーションイベント以外での呼び出しはNGです
    /// </summary>
    public void OnAnimationEvent()
    {
        m_isCalledAnimationEvent = true;
    }
    
}
