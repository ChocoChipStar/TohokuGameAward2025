using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PointData", menuName = "ScriptableObjects/PointData")]
public class PointData : ScriptableObject
{
    [SerializeField]
    private PosData m_posData = new PosData();

    [SerializeField]
    private ParamsData m_paramsData = new ParamsData();

    [SerializeField]
    private ItemData m_itemData = new ItemData();

    [SerializeField]
    private ChanceData m_chanceData = new ChanceData();

    public PosData Positions { get { return m_posData; } private set { value = m_posData; } }
    public ParamsData Params { get { return m_paramsData; } private set { value = m_paramsData; } }
    public ItemData Items {  get { return m_itemData; } private set { value = m_itemData; } }
    public ChanceData Chances { get { return m_chanceData; } private set { value = m_chanceData; } }

    [System.Serializable]
    public class PosData
    {
        [SerializeField,Header("回転させるアイテムの出現座標")]
        private List<Vector3> m_rotatingItemPos = new List<Vector3>();

        [SerializeField, Header("回転させないアイテムの出現座標")]
        private List<Vector3> m_staticItemPos = new List<Vector3>();

        public List<Vector3> RotatingItemPos { get { return m_rotatingItemPos; } private set { value = m_rotatingItemPos; } }

        public List<Vector3> StaticItemPos { get { return m_staticItemPos; } private set { value = m_staticItemPos; } }
    }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("爆弾を当てるときに大砲側に追加されるポイント")]
        private int m_cannonPoint = 0;

        [SerializeField, Header("爆弾に当たった時に引かれるポイント")]
        private int m_penaltyPoint = 0;

        [SerializeField, Header("爆弾でポイントが引かれた後のインターバル")]
        private float m_penaltyInterval = 0;

        [SerializeField, Header("死後のポイント増減のインターバル")]
        private float m_deadPointInterVal = 0;

        [SerializeField, Header("アイテムを回転させる角度")]
        private float m_rotatingAngle = 0f;

        [SerializeField, Header("アイテムが一度に出現する数")]
        private int m_maxItem = 0;

        [SerializeField, Header("アイテムが出現する頻度（秒）")]
        private float m_spawnInterval = 0f;

        [SerializeField, Header("アイテムが消滅する時間（秒）")]
        private float m_itemLifetime = 0;

        public int CannonPoint { get { return m_cannonPoint; } private set { value = m_cannonPoint; } }
        public int PenaltyPoint { get { return m_penaltyPoint; } private set { value = m_penaltyPoint; } }

        public float PenaltyInterval { get { return m_penaltyInterval; } private set { value = m_penaltyInterval; } }

        public float DeadPointInterval { get { return m_deadPointInterVal; } private set { value = m_deadPointInterVal; } }

        public float RotatingAngle { get { return m_rotatingAngle; } private set { value = m_rotatingAngle; } }

        public int MaxItem { get { return m_maxItem; } private set { value = m_maxItem; } }

        public float SpawnInterval { get { return m_spawnInterval; } private set { value = m_spawnInterval; } }

        public float ItemLifeTime { get { return m_itemLifetime; } private set { value = m_itemLifetime; } }
    }

    [System.Serializable]
    public class ItemData
    {
        [SerializeField, Header("プレハブ")]
        private GameObject[] m_pointItemPrefab = null;

        public GameObject[] Prefab { get { return m_pointItemPrefab; } private set { value = m_pointItemPrefab; } }
    }

    [System.Serializable]
    public class ChanceData
    {
        [SerializeField,Header("種類ごとの確率")]
        private float[] m_chanceOfPointItem = null;

        public float[] ChanceOfItem { get { return m_chanceOfPointItem; } private set { value = m_chanceOfPointItem; } }
    }

}