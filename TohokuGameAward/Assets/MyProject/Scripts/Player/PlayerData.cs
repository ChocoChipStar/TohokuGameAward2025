using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    private PositionData m_positionData = new PositionData();

    [SerializeField]
    private ParamsData m_paramsData = new ParamsData();

    [SerializeField]
    private ThrowData m_throwData = new ThrowData();

    public PositionData Positions { get { return m_positionData; } private set { value = m_positionData; } }
    public ParamsData Params { get { return m_paramsData; } private set { value = m_paramsData; } }
    public ThrowData Throw { get { return m_throwData; } private set { value = m_throwData; } }

    [System.Serializable]
    public class PositionData
    {
        [SerializeField, Header("ゲーム開始時座標を設定 P1~P4")]
        private Vector3[] m_startPos = new Vector3[4];

        public Vector3[] StartPos { get { return m_startPos; } private set { value = m_startPos; } }
    }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("重さ（質量）")]
        private int m_weight = 0;

        [SerializeField, Header("摩擦の強さ")]
        private float m_powerFrictional = 0.0f;

        [SerializeField, Header("左右の移動速度")]
        private float m_moveSpeed = 0.0f;

        [SerializeField, Header("ジャンプの強さ（高さ）")]
        private float m_powerJump = 0.0f;

        [SerializeField, Header("爆弾生成の時間（秒）")]
        private float m_bombGenerateTime = 0.0f;
        [SerializeField, Header("右向いた時の角度")]
        private float m_rightBodyAngle = 0.0f;

        [SerializeField, Header("左向いた時の角度")]
        private float m_leftBodyAngle = 0.0f;

        public int Weight { get { return m_weight; } private set { value = m_weight; } }
        public float PowerFrictional { get { return m_powerFrictional; } private set { value = m_powerFrictional; } }
        public float MoveSpeed { get { return m_moveSpeed; } private set { value = m_moveSpeed; } }
        public float PowerJump { get { return m_powerJump; } private set { value = m_powerJump; } }
        public float BombGenereareTime { get { return m_bombGenerateTime;} private set { value = m_bombGenerateTime; } }
        public float RightBodyAngle { get { return m_rightBodyAngle; } private set { value = m_rightBodyAngle; } }
        public float LeftBodyAngle { get { return m_leftBodyAngle; } private set { value = m_leftBodyAngle; } }
    }

    [System.Serializable]
    public class ThrowData
    {
        [SerializeField, Header("横投げ角度")]
        private float m_angleSide = 0.0f;

        [SerializeField, Header("横投げ強さ")]
        private float m_powerSide = 0.0f;

        [SerializeField, Header("上投げ角度")]
        private float m_angleUpper = 0.0f;

        [SerializeField, Header("上投げ強さ")]
        private float m_powerUpper = 0.0f;

        [SerializeField, Header("下投げ角度")]
        private float m_angleUnder = 0.0f;

        [SerializeField, Header("下投げ強さ")]
        private float m_powerUnder = 0.0f;

        public float AngleSide { get { return m_angleSide; } private set { value = m_angleSide; } }
        public float PowerSide { get { return m_powerSide; } private set { value = m_powerSide;} }
        public float AngleUpper { get { return m_angleUpper; } private set { value = m_angleUpper; } }
        public float PowerUpper { get { return m_powerUpper; } private set { value = m_powerUpper; } }
        public float AngleUnder { get { return m_angleUnder; } private set { value = m_angleUnder; } }
        public float PowerUnder { get { return m_powerUnder; } private set { value = m_powerUnder; } }
    }
}
