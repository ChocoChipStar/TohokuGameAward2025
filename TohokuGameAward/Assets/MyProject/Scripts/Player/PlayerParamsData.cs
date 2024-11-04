using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParamsData", menuName = "ScriptableObjects/PlayerParamsData", order = 1)]
public class PlayerParamsData : ScriptableObject
{
    [SerializeField, Header("重さ（質量）")]
    private int m_playerWeight = 0;

    [SerializeField, Header("摩擦の強さ")]
    private float m_frictionalPower = 0.0f;

    [SerializeField, Header("左右の移動速度")]
    private float m_moveSpeed = 0.0f;

    [SerializeField, Header("ジャンプの強さ（高さ）")]
    private float m_jumpPower = 0.0f;

    [SerializeField, Header("パンチの強さ")]
    private float m_punchPower = 0.0f;

    [SerializeField, Header("投げの強さ")]
    private float m_throwPower = 0.0f;

    [SerializeField, Header("アイテム横投げ角度")]
    private float m_sideThrowAngle = 0.0f;

    [SerializeField, Header("アイテム上投げ角度")]
    private float m_upperThrowAngle = 0.0f;

    [SerializeField, Header("アイテム下投げ角度")]
    private float m_underThrowAngle = 0.0f;

    public int PlayerWeight { get { return m_playerWeight; } private set { value = m_playerWeight; } }
    public float FrictionalPower { get { return m_frictionalPower; } private set { value = m_frictionalPower; } }
    public float MoveSpeed { get { return m_moveSpeed; } private set { value = m_moveSpeed; } }
    public float JumpPower { get { return m_jumpPower; } private set { value = m_jumpPower; } }
    public float PunchPower { get { return m_punchPower; } private set { value = m_punchPower; } }
    public float ThrowPower { get { return m_throwPower; } private set { value = m_throwPower; } }
    public float SideThrowAngle { get { return m_sideThrowAngle; } private set { value = m_sideThrowAngle; } }
    public float UpperThrowAngle { get { return m_upperThrowAngle; } private set { value = m_upperThrowAngle; } }
    public float UnderThrowAngle { get { return m_underThrowAngle; } private set { value = m_underThrowAngle; } }
}
