using System.Collections.Generic;
using UnityEngine;

public class BoxFactory : MonoBehaviour
{
    [SerializeField]
    private BoxData m_boxData = null;

    [SerializeField]
    private float m_respawnDelay = 0f;

    private float m_timeCounter = 0;

    //以下の配列はm_boxData.Positions.BoxSpawnPos.length()で初期化したいのでここではnullで初期化しています。
    private GameObject[] m_boxesSlot = null;
    private float[] m_respawnDelays = null;

    void Start()
    {
        m_boxesSlot = new GameObject[m_boxData.Positions.BoxSpawnPos.Length];
        m_respawnDelays = new float[m_boxData.Positions.BoxSpawnPos.Length];
    }

    private void Update()
    {
        SpawnBoxAtInterval();
        UpdateRespawnDelay();
    }

    private void SpawnBoxAtInterval()
    {
        if (m_timeCounter > m_boxData.Params.SpawnInterval)
        {
            SpawnBoxRandomPositionBased();
            m_timeCounter = 0f;
        }
        m_timeCounter += Time.deltaTime;
    }

    private void SpawnBoxRandomPositionBased()
    {
        for(int i = 0;i < m_boxData.Params.SpawnAtOnce;i++)
        {
            if (TotalBox() >= m_boxData.Params.SpawnMax)
            {
                return;
            }

            List<int> indexStorage = new List<int>();
            List<Vector3> selectablePos = ChooseSelectablePos(indexStorage);

            if(selectablePos.Count <= 0) { return; }
            int randomNum = Random.Range(0, selectablePos.Count - 1);

            GameObject box = GenerateBox(randomNum,indexStorage,selectablePos[randomNum]);

            //m_boxesSlotはBoxSpawnPosと同じ要素数で、その要素がnullかどうかで場所が埋まっているかを管理しています。
            m_boxesSlot[indexStorage[randomNum]] = box;
        }
    }

    /// <summary>
    /// 箱が置ける場所を選びリストにまとめます。
    /// </summary>
    private List<Vector3> ChooseSelectablePos(List<int> indexStorage)
    {
        List<Vector3> selectablePos = new List<Vector3>();

        for (int i = 0; i < m_boxData.Positions.BoxSpawnPos.Length; i++)
        {
            if (m_boxesSlot[i] == null && m_respawnDelays[i] <= 0)
            {   
                selectablePos.Add(m_boxData.Positions.BoxSpawnPos[i]);

                //BoxSpawnPosのどの場所が埋まっているか管理するため、インデックスを記憶しておきます
                indexStorage.Add(i);　　　
            }
        }
        return selectablePos;
    }

    private GameObject GenerateBox(int randomNum, List<int> IndexStorage,Vector3 pos)
    {
        GameObject box;
        box = Instantiate(m_boxData.Prefabs.Box[IndexStorage[randomNum]], pos, Quaternion.identity, this.transform);

        BoxDestroy boxDestroy = box.GetComponent<BoxDestroy>();
        boxDestroy.SetBoxIndex(IndexStorage[randomNum]);

        return box;
    }

    private int TotalBox()
    {
        int count = this.transform.childCount;
        return count;
    }

    public void SetRespawnDelay(int Index)
    {
        m_boxesSlot[Index] = null;
        m_respawnDelays[Index] = m_respawnDelay;
    }

    private void UpdateRespawnDelay()
    {
        for (int i = 0; i < m_respawnDelays.Length; i++)
        {
            if (m_respawnDelays[i] > 0)
            {
                m_respawnDelays[i] -= Time.deltaTime;
            }
        }
    }

    public void CreanBoxSlot(int i)
    {
        Destroy(m_boxesSlot[i]);
    }
}
