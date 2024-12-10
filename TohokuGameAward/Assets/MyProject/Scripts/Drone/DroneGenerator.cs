using System;
using System.Linq;
using UnityEngine;

public class DroneGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject m_dronePrefab = null;

    [SerializeField]
    GameObject[] m_crownPrefabs = null;

    [SerializeField]
    private int[] m_chanceOfCrowns = null;

    [SerializeField]
    float m_droneSpawnTime = 0.0f;

    [SerializeField]
    Vector2 m_droneSpawnPosition = Vector2.zero;

    [SerializeField]
    Vector2 m_crownSpawnPosition = Vector2.zero;

    float m_spawnTimer = 0.0f;

    int m_totalCrownChance = 0;

    GameObject m_parentObject = null;

    private Tuple<GameObject, int>[] m_crownData = null; 

    private void Start()
    {
        m_totalCrownChance = TotalCrownChance();
        m_crownData = MakeCrownData();
    }
    void Update()
    {
        if(IsSpawnTimeReached())
        {
            m_parentObject = CreateDrone();

            CreateCrown(m_parentObject);
        }
    }

    private GameObject CreateDrone()
    {
        GameObject drone = null;
        drone = Instantiate(m_dronePrefab, m_droneSpawnPosition, Quaternion.identity,this.transform);
        return drone;
    }

    private void CreateCrown(GameObject drone)
    {
        Instantiate(ChooseCrownGanre(MakeRandomNumber()), m_crownSpawnPosition, Quaternion.identity, drone.transform);
    }

    private bool IsSpawnTimeReached()
    {
        m_spawnTimer += Time.deltaTime;

        if(m_droneSpawnTime < m_spawnTimer)
        {
            m_spawnTimer = 0.0f;

            return true;
        }
        return false;
    }

    private int TotalCrownChance()
    {
        int totalChance = 0;
        for (int i = 0; i < m_chanceOfCrowns.Length; i++)
        {
            totalChance += m_chanceOfCrowns[i];
        }
        return totalChance;
    }

    /// <summary>
    /// 王冠の種類と出現確率を対応させたデータ(Tuple)を作る。
    /// </summary>
    /// <returns></returns>
    private Tuple<GameObject, int>[] MakeCrownData()
    {
        var makeTuple = m_crownPrefabs.Zip(m_chanceOfCrowns, (prefab, chance) => Tuple.Create(prefab, chance));
        makeTuple.OrderByDescending(item => item.Item2);
        var maketupleArray = makeTuple.ToArray();
        return maketupleArray;
    }

    /// <summary>
    /// 王冠をランダム生成するための乱数を生成する。
    /// </summary>
    /// <returns></returns>
    private int MakeRandomNumber()
    {
        int randomNumber = UnityEngine.Random.Range(0, m_totalCrownChance);
        return randomNumber;
    }

    /// <summary>
    /// 乱数を使い王冠の種類を選び、対応するプレハブを返す。
    /// </summary>
    /// <returns></returns>
    private GameObject ChooseCrownGanre(int numberToChoose)
    {
        int numberToChoose_ = numberToChoose;

        //m_crownDataについて
        //m_crownData[i].Item1 は m_crownPrefabs[i]と同じ値が入っています。
        //m_crownData[i].Item2 は m_chanceOfCrowns[i] と同じ値が入っています。

        for (int i = 0; i < m_crownPrefabs.Length; i++)
        {
            if (numberToChoose_ < m_crownData[i].Item2) 
            {
                //m_crownPrefabsのi番目のプレハブを返す。
                return m_crownData[i].Item1;　
            }
            else
            {
                //numberToChoose_から今回比べた王冠の確率を引いて次のループへ。
                numberToChoose_ -= m_crownData[i].Item2;
            }
        }
        //m_crownPrefabsの最後のプレハブを返す。
        return m_crownData[^1].Item1; 
    }
}