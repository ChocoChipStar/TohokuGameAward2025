using Unity.VisualScripting;
using UnityEngine;
using static PlayerAnimator;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_rigidbody = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    [SerializeField]
    private InputData m_inputData = null;

    // [SerializeField]
    // private PlayerProduceBomb m_produceBomb = null;
    
    [SerializeField]
    private PlayerAnimator m_animator = null;

    [SerializeField]
    private WallDetector m_detectorR = null;

    [SerializeField]
    private WallDetector m_detectorL = null;

    public bool IsGrounded { get; private set; } = false;

    private bool m_isGetExplosion = false;

    private const float FallingMinValue = 0.1f;

    private void Update()
    {
        var stickValue = m_inputData.GetLeftStickValue(m_inputData.SelfNumber);
        if (CanMove(stickValue) && !m_isGetExplosion)
        {
            MoveOperation(stickValue);
        }
        else
        {
            m_animator.TransferableState(top: TopState.Move);
            m_animator.TransferableState(under: UnderState.Move);
        }

        if(CanJump())
        {
            JumpOperation();
        }

        if(!IsGrounded && m_rigidbody.velocity.y <= FallingMinValue)
        {
            m_animator.ChangeTopState(TopState.Falling);
            m_animator.ChangeUnderState(UnderState.Falling);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (TagManager.Instance.SearchedTagName(collision.gameObject, TagManager.Type.Ground))
        {
            m_animator.TransferableState(top: TopState.Falling);
            m_animator.TransferableState(under: UnderState.Falling);
            IsGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (TagManager.Instance.SearchedTagName(collision.gameObject, TagManager.Type.Ground))
        {
            IsGrounded = false;
        }
    }

    /// <summary>
    /// スティック入力値を基にプレイヤーを動かす処理を実行します
    /// </summary>
    private void MoveOperation(Vector2 stickValue)
    {
        var speedX = m_playerData.Params.MoveSpeed * stickValue.x;
        this.transform.position += new Vector3(speedX, 0.0f, 0.0f) * Time.deltaTime;

        m_animator.ChangeTopState(TopState.Move);
        m_animator.ChangeUnderState(UnderState.Move);
    }

    /// <summary>
    /// プレイヤーが移動できる状態かを確認します
    /// </summary>
    private bool CanMove(Vector2 stickValue)
    {
        // ボムを生成している。または、爆発を受けている
        //if(m_produceBomb.isGenerating && !m_isGetExplosion)
        //{
        //    return false;
        //}

        // 右の壁に衝突していたら右への移動入力を不可にする
        if (stickValue.x > InputData.MovementDeadZoneRange && m_detectorR.IsHitWall())
        {
            return false;
        }

        // 左の壁に衝突していたら左への移動入力を不可にする
        if (stickValue.x < -InputData.MovementDeadZoneRange && m_detectorL.IsHitWall())
        {
            return false;
        }

        if (stickValue.x > InputData.MovementDeadZoneRange || stickValue.x < -InputData.MovementDeadZoneRange)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ジャンプ挙動の処理を実行します
    /// </summary>
    private void JumpOperation()
    {
        m_rigidbody.AddForce(new Vector3(0.0f, m_playerData.Params.PowerJump, 0.0f), ForceMode.Impulse);

        m_animator.ChangeTopState(TopState.Jump);
        m_animator.ChangeUnderState(UnderState.Jump);
    }

    /// <summary>
    /// プレイヤーが今ジャンプ挙動を実行できる状態か確認します。
    /// </summary>
    private bool CanJump()
    {
        if(m_inputData.WasPressedActionButton(InputData.ActionsName.Jump, m_inputData.SelfNumber) 
        && IsGrounded && !m_isGetExplosion /*&& !m_produceBomb.isGenerating*/)
        {
            return true;
        }

        return false;
    }

    public void GetExplosion(bool ifGetExplosion)
    {
        m_isGetExplosion = ifGetExplosion;
    }
}
