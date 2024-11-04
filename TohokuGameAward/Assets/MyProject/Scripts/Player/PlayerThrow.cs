using System.Collections;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField]
    private PlayerPickUp m_playerPickUp = null;

    [SerializeField]
    private PlayerInputData m_inputData = null;

    [SerializeField]
    private PlayerParamsData m_paramsData = null;

    private const float FixedInverseAngle = 180.0f;
    private const float ThrowingIntervalTime = 0.75f;

    public static bool IsThrow { get; private set; } = false;

    private void Update()
    {
        var stickValue = m_inputData.GetLeftStickValue(PlayerManager.SelfNumber);
        if (CanThrow(stickValue) )
        {
            ThrowHoldingItem(stickValue);
        }
    }

    /// <summary>
    /// �A�C�e������������Ԃ��m�F���܂�
    /// </summary>
    private bool CanThrow(Vector2 stickValue)
    {
        var wasPressedRT = m_inputData.WasPressedButton(PlayerInputData.InputButton.Throw, PlayerManager.SelfNumber);
        if (wasPressedRT && PlayerPickUp.IsHoldingItem)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// �A�C�e���𓊂��鏈�������s���܂�
    /// </summary>
    private void ThrowHoldingItem(Vector2 stickValue)
    {
        IsThrow = true;
        var rigidbody = m_playerPickUp.DetectedItemObj.GetComponent<Rigidbody>();
        RestoreHoldingItem(rigidbody);

        var throwDirection = ConvertAngleToDirection(stickValue) * m_paramsData.ThrowPower;
        rigidbody.AddForce(throwDirection, ForceMode.Impulse);

        Debug.Log(throwDirection);
        StartCoroutine(ResetItemProcess()); // �A�C�e���S�ʂ̏�����
    }

    /// <summary>
    /// �E���Ă����A�C�e���̏�Ԃ����̏�Ԃɖ߂����������s���܂�
    /// </summary>
    private void RestoreHoldingItem(Rigidbody rigidbody)
    {
        rigidbody.useGravity = true;

        var sphereCollider = m_playerPickUp.DetectedItemObj.GetComponent<SphereCollider>();
        sphereCollider.enabled = true;
    }

    /// <summary>
    /// �p�x������ɕϊ����鏈�����s���܂�
    /// </summary>
    private Vector3 ConvertAngleToDirection(Vector2 stickValue)
    {
        var radian = GetThrowAngle(stickValue) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0.0f);
    }

    /// <summary>
    /// �R���g���[���[�̓��͒l����ɓ����p�x���擾���܂�
    /// </summary>
    private float GetThrowAngle(Vector2 stickValue)
    {
        var deadZone = PlayerInputData.ThrowDeadZoneRange;
        var fixedDirectionValue = 0.0f;
        if (m_playerPickUp.IsRight)
        {
            fixedDirectionValue = 0.0f;
        }
        else
        {
            fixedDirectionValue = FixedInverseAngle;
        }

        if (stickValue.y > deadZone) // �㓊��
        {
            return Mathf.Abs(m_paramsData.UpperThrowAngle - fixedDirectionValue);
        }
        else if (stickValue.y < -deadZone) // ������
        {
            return Mathf.Abs(m_paramsData.UnderThrowAngle - fixedDirectionValue);
        }

        if (m_playerPickUp.IsRight) // �E����
        {
            return m_paramsData.SideThrowAngle;
        }
        else // ������
        {
            return FixedInverseAngle - m_paramsData.SideThrowAngle;
        }
    }

    public IEnumerator ResetItemProcess()
    {
        yield return new WaitForSeconds(ThrowingIntervalTime);

        m_playerPickUp.InitializedPickUp();
        IsThrow = false;
    }
}
