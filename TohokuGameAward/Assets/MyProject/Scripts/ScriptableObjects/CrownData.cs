using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrownData", menuName = "ScriptableObjects/CrownData")]
public class CrownData : ScriptableObject
{
    [SerializeField, Header("生成する王冠のプレファブ")]
    private GameObject[] m_prefabs = null;

    [SerializeField]
    private PositionData m_posData = new PositionData();
    [SerializeField]
    private ParamsData m_paramsData = new ParamsData();
    [SerializeField]
    private RateData m_rateData = new RateData();

    public GameObject[] Prefabs { get { return m_prefabs; } private set { m_prefabs = value; } }
    public PositionData Positions { get { return m_posData; } private set { m_posData = value; } }
    public ParamsData Params {  get { return m_paramsData; } private set { m_paramsData = value; } }
    public RateData Rate { get { return m_rateData; } private set { m_rateData = value; } }

    public enum CrownPattern
    {
        Silver,
        Gold,
        Diamond,
        Max
    }

    [System.Serializable]
    public class PositionData
    {
        [SerializeField, Header("アイテムの出現座標")]
        private List<Vector3> m_generatePos = new List<Vector3>();

        public List<Vector3> GeneratePos { get { return m_generatePos; } private set { m_generatePos = value; } }
    }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("一度に出現する数")]
        private int m_generateCountOneTime = 0;

        [SerializeField, Header("王冠の出現頻度（秒）")]
        private float m_generateSpan = 0.0f;

        [SerializeField, Header("王冠が消滅までの時間（秒）")]
        private float m_lifeTime = 0.0f;

        public int GenerateCountOneTime { get { return m_generateCountOneTime; } private set { m_generateCountOneTime = value; } }
        public float GenerateSpan { get { return m_generateSpan; } private set { m_generateSpan = value; } }
        public float LifeTime { get { return m_lifeTime; } private set { m_lifeTime = value; } }
    }

    [System.Serializable]
    public class RateData
    {
        [SerializeField, Header("シルバー王冠の生成確率")]
        private float m_silverRate = 0.0f;

        [SerializeField, Header("ゴールド王冠の生成確率")]
        private float m_goldRate = 0.0f;

        [SerializeField, Header("ダイヤモンド王冠の生成確率")]
        private float m_diamondRate = 0.0f;

        public float SilverRate { get { return m_silverRate; } private set { m_silverRate = value; } }
        public float GoldRate { get { return m_goldRate; } private set { m_goldRate = value; } }
        public float DiamondRate { get { return m_diamondRate; } private set { m_diamondRate = value; } }
    }
}
