using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialCrownData", menuName = "ScriptableObjects/SpecialCrownData")]
public class SpecialCrownData : ScriptableObject
{
    [SerializeField, Header("生成する王冠のプレファブ")]
    private GameObject m_prefab = null;

    [SerializeField]
    private PositionData m_posData = new PositionData();
    [SerializeField]
    private ParamsData m_paramsData = new ParamsData();


    public GameObject Prefabs { get { return m_prefab; } private set { m_prefab = value; } }
    public PositionData Positions { get { return m_posData; } private set { m_posData = value; } }
    public ParamsData Params { get { return m_paramsData; } private set { m_paramsData = value; } }

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

        [SerializeField, Header("特別な王冠が初めて生成されるまでの時間（秒）")]
        private float m_generateStartTime = 0.0f;


        public int GenerateCountOneTime { get { return m_generateCountOneTime; } private set { m_generateCountOneTime = value; } }
        public float GenerateStartTime { get { return m_generateStartTime; } private set { m_generateStartTime = value; } }
    }
}
