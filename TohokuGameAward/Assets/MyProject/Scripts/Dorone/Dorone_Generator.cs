using System;
using System.Linq;
using UnityEngine;

public class Dorone_Generator : MonoBehaviour
{
    [SerializeField]
    GameObject m_doronePrefab = null;

    [SerializeField]
    GameObject[] m_crownPrefabs = null;

    [SerializeField]
    private int[] m_chanceOfCrowns = null;

    [SerializeField]
    float m_doroneSpawnTime = 0.0f;

    [SerializeField]
    Vector2 m_doroneSpawnPosition = Vector2.zero;

    [SerializeField]
    Vector2 m_crownSpawnPosition = Vector2.zero;

    float m_spawnTimer = 0.0f;

    int m_totalCrownChance = 0;

    GameObject m_parentObject = null;

    private Tuple<GameObject, int>[] m_crownData = null;

    private void Start()
    {
        m_totalCrownChance = TotalCrownChance();
        m_crownData = MakebombData();
    }
    void Update()
    {
        if(IsSpawnTimeReached())
        {
            m_parentObject = CreateDorone();

            CreateCrown(m_parentObject);
        }
    }

    private GameObject CreateDorone()
    {
        GameObject dorone = null;
        dorone = Instantiate(m_doronePrefab, m_doroneSpawnPosition, Quaternion.identity,this.transform);
        return dorone;
    }

    private void CreateCrown(GameObject dorone)
    {
        Instantiate(ChooseCrownGanre(MakeRandomNumber()), m_crownSpawnPosition, Quaternion.identity, dorone.transform);
    }

    private bool IsSpawnTimeReached()
    {
        m_spawnTimer += Time.deltaTime;

        if(m_doroneSpawnTime < m_spawnTimer)
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

    private Tuple<GameObject, int>[] MakebombData()
    {
        var makeTuple = m_crownPrefabs.Zip(m_chanceOfCrowns, (prefab, chance) => Tuple.Create(prefab, chance));
        makeTuple.OrderByDescending(item => item.Item2);
        var maketupleArray = makeTuple.ToArray();
        return maketupleArray;
    }

    private int MakeRandomNumber()
    {
        int randomNumber = UnityEngine.Random.Range(0, m_totalCrownChance);
        return randomNumber;
    }

    private GameObject ChooseCrownGanre(int numberToChoose)
    {
        int numberToChoose_ = numberToChoose;

        for (int i = 0; i < m_crownPrefabs.Length; i++)
        {
            if (numberToChoose_ < m_crownData[i].Item2)
            {
                return m_crownData[i].Item1;
            }
            else
            {
                numberToChoose_ -= m_crownData[i].Item2;
            }
        }
        return m_crownData[^1].Item1;
    }
    private void OnValidate()
    {
        if (m_chanceOfCrowns == null || m_crownPrefabs == null)
        { return; }
        if (m_chanceOfCrowns.Length != m_crownPrefabs.Length)
        {
            System.Array.Resize(ref m_chanceOfCrowns, m_crownPrefabs.Length);
        }
    }
}