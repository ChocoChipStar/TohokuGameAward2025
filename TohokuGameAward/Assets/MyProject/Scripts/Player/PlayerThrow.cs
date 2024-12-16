using System.Collections;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField]
    private PlayerPickup m_pickup = null;

    [SerializeField]
    private PlayerAnimator m_animator = null;

    [SerializeField]
    private PlayerInputData m_inputData = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    private float m_throwPower = 0.0f;

    private const float FixedInverseAngle = 180.0f;
    private const float ThrowingIntervalTime = 0.75f;

    public bool IsThrow { get; private set; } = false;

    public enum ThrowDirection
    {
        Upper,
        Side,
        Under
    }

    private void Update()
    {
        var stickValue = m_inputData.GetLeftStickValue(m_inputData.SelfNumber);
        if (CanThrow(stickValue))
        {
            ThrowHoldingItem(stickValue);
        }
    }

    /// <summary>
    /// プレイヤーが手持ちアイテムを投げれる状態か確認します
    /// </summary>
    private bool CanThrow(Vector2 stickValue)
    {
        if (m_pickup.DetectedItemObj == null)
        {
            return false;
        }

        var wasPressedRT = m_inputData.WasPressedButton(PlayerInputData.ActionsName.Throw, m_inputData.SelfNumber);
        if (wasPressedRT && m_pickup.IsHoldingItem)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 手に持っているアイテムを投げる処理を実行します
    /// </summary>
    private void ThrowHoldingItem(Vector2 stickValue)
    {
        m_pickup.InitializedPickup();

        var throwDirection = GetThrowDirection(stickValue);
        var throwMomentum = throwDirection * m_throwPower; 

        var bombBase = m_pickup.DetectedItemObj.GetComponent<BombBase>();
        bombBase.OnThrow(throwMomentum);
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
        var fixedDirectionValue = 0.0f;
        if (m_pickup.IsRight)
        {
            fixedDirectionValue = 0.0f;
        }
        else
        {
            fixedDirectionValue = FixedInverseAngle;
        }

        if (stickValue.y > deadZone)
        {
            SavedThrowPower(m_playerData.Throw.PowerUpper);
            m_animator.ChangeTopState(PlayerAnimator.TopState.ThrowUpper);
            return Mathf.Abs(m_playerData.Throw.AngleUpper - fixedDirectionValue);
        }
        else if (stickValue.y < -deadZone)
        {
            SavedThrowPower(m_playerData.Throw.PowerUnder);
            m_animator.ChangeTopState(PlayerAnimator.TopState.ThrowUnder);
            return Mathf.Abs(m_playerData.Throw.AngleUnder - fixedDirectionValue);
        }

        SavedThrowPower(m_playerData.Throw.PowerSide);
        m_animator.ChangeTopState(PlayerAnimator.TopState.ThrowFront);
        if (m_pickup.IsRight)
        {
            return m_playerData.Throw.AngleSide;
        }
        else
        {
            return FixedInverseAngle - m_playerData.Throw.AngleSide;
        }
    }

    /// <summary>
    /// 投げ角度に合わせて投げ威力を再設定します
    /// </summary>
    /// <param name="powerValue"></param>
    private void SavedThrowPower(float powerValue)
    {
        m_throwPower = powerValue;
    }
}
