using UnityEngine;

[CreateAssetMenu(fileName = "NewBombData", menuName = "ScriptableObjects/BombData", order = 1)]
public class BombData : ScriptableObject
{
    public enum BombType
    {
        [InspectorName("ノーマル")]
        Normal,
        [InspectorName("インパルス")]
        Impulse
    }

    [SerializeField, Header("爆弾の種類を選択")]
    private BombType m_bombType = new BombType();
    
    [SerializeField]
    private ParamsData m_paramsData = new ParamsData();

    [SerializeField]
    private StatusData m_statusData = new StatusData();

    public static readonly int BombMax = 2;

    public BombType Type { get { return m_bombType; } private set { value = m_bombType; } }
    public ParamsData Params { get { return m_paramsData; } private set { value = m_paramsData; } }
    public StatusData Status { get { return m_statusData; } private set { value = m_statusData; } }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("爆弾の重さ")]
        private float m_bombMass = 0.0f;

        [SerializeField, Header("回転摩擦の強さ")]
        private float m_angularDrag = 0.0f;

        [SerializeField, Header("爆弾が着火してから爆発するまでの時間")]
        private float m_explosionDelayTime = 0.0f;

        [SerializeField, Header("爆風による吹き飛びの強さ")]
        private float m_explosionPower = 0.0f;

        [SerializeField, Header("爆弾の爆風の広さ")]
        private float m_explosionRange = 0.0f;

        [SerializeField, Header("何かしらと衝突時に即起爆させるか")]
        private bool m_isPromptExplosion = false;

        public float BombMass { get { return m_bombMass; } private set { value = m_bombMass; } }
        public float AngularDrag { get { return m_angularDrag; } private set { value = m_angularDrag; } }
        public float ExplosionDelayTime { get { return m_explosionDelayTime; } private set { value = m_explosionDelayTime; } }
        public float ExplosionPower { get { return m_explosionPower; } private set { value = m_explosionPower; } }
        public float ExplosionRange { get { return m_explosionRange; } private set { value = m_explosionRange; } }
        public bool IsPromptExplosion { get { return m_isPromptExplosion; } private set { value = m_isPromptExplosion; } }
    }

    [System.Serializable]
    public class StatusData
    {
        [SerializeField, Header("爆弾の回転の中心点")]
        private float m_bombPivot = 0.0f;

        public float BombPivot { get { return m_bombPivot; } private set { value = m_bombPivot; } }
    }
}