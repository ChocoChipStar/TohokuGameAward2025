using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxData", menuName = "ScriptableObjects/BoxData")]
public class BoxData : ScriptableObject
{
    [SerializeField]
    private PositionData m_positionData = new PositionData();

    [SerializeField]
    private PrefabData m_prefabData = new PrefabData();

    [SerializeField]
    private ParamsData m_paramData = new ParamsData();

    public PositionData Positions { get { return m_positionData; } private set { value = m_positionData; } }

    public ParamsData Params { get { return m_paramData; } private set { value = m_paramData; } }

    public PrefabData Prefabs {  get { return m_prefabData; }private set { value = m_prefabData; } }

    [System.Serializable]
    public class PositionData
    {
        [SerializeField, Header("箱が出現する位置")]
        private Vector3[] m_boxSpawnPos = null;

        public Vector3[] BoxSpawnPos { get { return m_boxSpawnPos; } private set { value = m_boxSpawnPos; } }
    }

    [System.Serializable]
    public class PrefabData
    {
        [SerializeField, Header("boxSpawnPos[i]に出現させる箱のプレハブ")]
        private GameObject[] m_box;

        public GameObject[] Box { get { return m_box; }private set { value = m_box; } }

    }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("一度のスポーン処理で箱が出現する数")]
        private int m_spawnAtOnce = 0;

        [SerializeField, Header("箱がステージ上に存在できる最大数")]
        private int m_spawnMax = 0;

        [SerializeField, Header("箱が出現する秒数")]
        private float m_spawnInterval = 0;

        public int SpawnAtOnce { get { return m_spawnAtOnce; } private set { value = m_spawnAtOnce; } }

        public int SpawnMax { get { return m_spawnMax; } private set { value = m_spawnMax; } }

        public float SpawnInterval { get { return m_spawnInterval; } private set { value = m_spawnInterval; } }
    }
}