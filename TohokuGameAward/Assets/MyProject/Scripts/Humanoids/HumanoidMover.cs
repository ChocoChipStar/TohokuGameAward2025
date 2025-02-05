using UnityEngine;
using UnityEngine.EventSystems;
using static HumanoidAnimator;

public class HumanoidMover : MonoBehaviour
{
    [SerializeField]
    private HumanoidData m_humanoidData = null;

    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private SelfData m_selfData = null;

    [SerializeField]
    private Rigidbody m_rigidbody = null;

    [SerializeField]
    private BoxCollider m_collider = null;

    [SerializeField]
    private HumanoidAnimator m_animator = null;

    private float m_movementValue = 0.0f;

    private bool m_isOperable = true;

    private RaycastHit hitInfo;

    private const float FallingMinValue = 0.1f;
    private const float RayDistance = 0.6f;

    private static readonly Vector3 FixedRayStartPos = new Vector3(0.0f, 0.5f, 0.0f);

    private void Update()
    {
        if(!m_isOperable)
        {
            return;
        }

        if (CanMove())
        {
            MoveOperation();
        }

        if(CanJump())
        {
            JumpOperation();
        }

        if(!IsGrounded() && m_rigidbody.velocity.y <= FallingMinValue)
        {
            m_animator.ChangeTopState(TopState.Falling);
            m_animator.ChangeUnderState(UnderState.Falling);
        }
    }

    /// <summary>
    /// ヒューマノイドの移動処理を行います
    /// </summary>
    private void MoveOperation()
    {
        var speedX = m_humanoidData.Params.MoveSpeed * m_movementValue;
        this.transform.position += new Vector3(speedX, 0.0f, 0.0f) * Time.deltaTime;

        m_animator.ChangeTopState(TopState.Move);
        m_animator.ChangeUnderState(UnderState.Move);
    }

    /// <summary>
    /// プレイヤーが移動できる状態かを確認します
    /// </summary>
    private bool CanMove()
    {
        m_movementValue = m_inputData.GetHumanoidMoveInput(m_selfData.Number);
        if (m_movementValue > InputData.MovementDeadZoneRange || m_movementValue < -InputData.MovementDeadZoneRange)
        {
            return true;
        }

        m_animator.TransferableState(top: TopState.Move);
        m_animator.TransferableState(under: UnderState.Move);

        return false;
    }

    /// <summary>
    /// ジャンプ挙動の処理を行います
    /// </summary>
    private void JumpOperation()
    {
        m_rigidbody.AddForce(new Vector3(0.0f, m_humanoidData.Params.PowerJump, 0.0f), ForceMode.Impulse);

        m_animator.ChangeTopState(TopState.Jump);
        m_animator.ChangeUnderState(UnderState.Jump);
    }

    /// <summary>
    /// ジャンプ挙動を実行できる状態か確認します。
    /// </summary>
    private bool CanJump()
    {
        if(!IsGrounded())
        {
            return false;
        }

        if(m_inputData.WasPressedActionInput(InputData.ActionsType.Jump, m_selfData.Number))
        {
            return true;
        }
        return false;
    }

    private bool IsGrounded()
    {
        var startPos = this.transform.position + FixedRayStartPos;
        var ray = new Ray(startPos, transform.up * -1);

        if(Physics.Raycast(ray, out hitInfo, RayDistance))
        {
            m_animator.TransferableState(top: TopState.Falling);
            m_animator.TransferableState(under: UnderState.Falling);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 操作可能状態の指定します
    /// </summary>
    public void SetOperable(bool isEnabled)
    {
        m_isOperable = isEnabled;
    }

    /// <summary>
    /// 物理挙動の有無を指定します
    /// </summary>
    public void SetPhysicalOperable(bool isActive)
    {
        m_rigidbody.useGravity = isActive;
        m_rigidbody.isKinematic = !isActive;

        m_collider.enabled = isActive;
    }
}
