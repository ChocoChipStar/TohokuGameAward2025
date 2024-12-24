using System.Collections;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField]
    private PlayerPickup m_pickup = null;

    [SerializeField]
    private PlayerAnimator m_animator = null;

    [SerializeField]
    private PlayerDirectionRotator m_directionRotator = null;

    [SerializeField]
    private PlayerInputData m_inputData = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    private float m_throwPower = 0.0f;

    private Vector3 m_forceVector = Vector3.zero;

    private const float FixedInverseAngle = 180.0f;

    public bool IsThrow { get; private set; } = false;

    public enum ThrowDirection
    {
        Upper,
        Side,
        Under
    }

    private void Update()
    {
        if(!IsThrow)
        {
            CanThrow();
            return;
        }
        
        if (m_animator.IsFinishedThrowFirst)
        {
            ThrowHoldingItem();
        }
    }

    /// <summary>
    /// プレイヤーが手持ちアイテムを投げれる状態か確認します
    /// </summary>
    private void CanThrow()
    {
        if (m_pickup.DetectedItemObj == null)
        {
            return;
        }

        var wasPressedRT = m_inputData.WasPressedButton(PlayerInputData.ActionsName.Throw, m_inputData.SelfNumber);
        if (wasPressedRT && m_pickup.IsHoldingItem)
        {
            m_forceVector = CalculateForceVector();
            IsThrow = true;
        }
    }

    /// <summary>
    /// 投げ威力と投げ角度から力のベクトルを計算します
    /// </summary>
    private Vector3 CalculateForceVector()
    {
        var stickValue = m_inputData.GetLeftStickValue(m_inputData.SelfNumber);
        return GetThrowDirection(stickValue) * m_throwPower;
    }

    /// <summary>
    /// 手に持っているアイテムを投げる処理を実行します
    /// </summary>
    private void ThrowHoldingItem()
    {
        m_pickup.InitializedPickup();
        m_animator.InitializeThrowFirst();
        IsThrow = false;

        if(m_pickup.DetectedItemObj == null)
        {
            return;
        }

        var bombBase = m_pickup.DetectedItemObj.GetComponent<BombBase>();
        bombBase.OnThrow(m_forceVector);
    }

    /// <summary>
    /// 投げ角度を方向に変換し取得します
    /// </summary>
    private Vector3 GetThrowDirection(Vector2 stickValue)
    {
        var radian = GetThrowAngle(stickValue) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0.0f);
    }

    /// <summary>
    /// スティックの入力値から投げ角度を決定します
    /// </summary>
    private float GetThrowAngle(Vector2 stickValue)
    {
        var deadZone = PlayerInputData.ThrowDeadZoneRange;
        if (stickValue.y > deadZone)
        {
            InitializeThrow(m_playerData.Throw.PowerUpper, PlayerAnimator.TopState.ThrowUpper);
            return ModifiedAngleByDirection(m_playerData.Throw.AngleUpper);
        }
        
        if (stickValue.y < -deadZone)
        {
            InitializeThrow(m_playerData.Throw.PowerUnder, PlayerAnimator.TopState.ThrowUnder);
            return ModifiedAngleByDirection(m_playerData.Throw.AngleUnder);
        }

        InitializeThrow(m_playerData.Throw.PowerSide, PlayerAnimator.TopState.ThrowFront);
        return ModifiedAngleByDirection(m_playerData.Throw.AngleSide);
    }

    /// <summary>
    /// 投げ処理の初期化処理を行います
    /// </summary>
    private void InitializeThrow(float throwPower, PlayerAnimator.TopState throwState)
    {
        SavedThrowPower(throwPower);
        m_animator.ChangeTopState(throwState);
    }

    /// <summary>
    /// プレイヤーの向きによる投げ角度の値を算出
    /// </summary>
    private float ModifiedAngleByDirection(float currentThrowAngle)
    {
        if (m_directionRotator.IsRight.Value)
        {
            return currentThrowAngle;
        }

        return Mathf.Abs(currentThrowAngle - FixedInverseAngle);
    }

    /// <summary>
    /// 投げ角度に合わせて投げ威力を再設定します
    /// </summary>
    private void SavedThrowPower(float powerValue)
    {
        m_throwPower = powerValue;
    }


    /// <summary>
    /// 現在プレイヤーが投げ動作を行っているか否か
    /// </summary>
    public bool IsPlaybackThrow()
    {
        return IsThrow || m_animator.IsPlaybackThrow;
    }
}
