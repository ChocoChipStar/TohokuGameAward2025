using UnityEngine;

[CreateAssetMenu(fileName = "NewCannonData", menuName = "ScriptableObjects/CannonDeta")]
public class CannonData : ScriptableObject
{
    [SerializeField]
    private PositionData m_positionData = new PositionData();

    [SerializeField]
    private ParamsData m_paramData = new ParamsData();

    public PositionData Positions { get { return m_positionData; } set { value = m_positionData; } }
    public ParamsData Params { get { return m_paramData; } set { value = m_paramData; } }

    [System.Serializable]
    public class PositionData
    {
        [SerializeField, Header("ゲーム開始時座標を設定(左右の割合で指定)")]
        private float[] m_startPosition = new float[2];

        public float[] StartPosition { get { return m_startPosition; } private set { value = m_startPosition; } }
    }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("大砲側人数")]
        private int m_cannonCount = 0;

        [SerializeField, Header("大砲間の距離")]
        private float m_cannonDictance = 0.0f;

        [SerializeField, Header("移動速度")]
        private float m_moveSpeed = 0.0f;

        [SerializeField, Header("爆弾発射速度")]
        private float m_shootSpeed = 0.0f;

        public int CannonCount { get { return m_cannonCount; } private set { value = m_cannonCount; } }
        public float CannonDictance { get { return m_cannonDictance; } private set { value = m_cannonDictance; } }
        public float MoveSpeed { get { return m_moveSpeed; } private set { value = m_moveSpeed; } }
        public float ShootSpeed { get { return m_shootSpeed; } private set { value = m_shootSpeed; } }

    }
}