using UnityEngine;
using static PlayerAnimator;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_rigidbody = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    [SerializeField]
    private PlayerInputData m_inputData = null;

    [SerializeField]
    private PlayerAnimator m_animator = null;

    [SerializeField]
    private WallDetector m_detectorR = null;

    [SerializeField]
    private WallDetector m_detectorL = null;

    public bool IsGrounded { get; private set; } = false;

    private bool m_isGetExplosion = false;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (TagManager.Instance.SearchedTagName(collision.gameObject, TagManager.Type.Ground))
        {
            m_animator.TransferableState(top: TopState.Jump);
            m_animator.TransferableState(under: UnderState.Jump);
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
        // 右の壁に衝突していたら右への移動入力を不可にする
        if (stickValue.x > PlayerInputData.MovementDeadZoneRange && m_detectorR.IsHitWall())
        {
            return false;
        }

        // 左の壁に衝突していたら左への移動入力を不可にする
        if (stickValue.x < -PlayerInputData.MovementDeadZoneRange && m_detectorL.IsHitWall())
        {
            return false;
        }

        if (stickValue.x > PlayerInputData.MovementDeadZoneRange || stickValue.x < -PlayerInputData.MovementDeadZoneRange)
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
        if(m_inputData.WasPressedButton(PlayerInputData.ActionsName.Jump, m_inputData.SelfNumber) 
        && IsGrounded && !m_isGetExplosion)
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
