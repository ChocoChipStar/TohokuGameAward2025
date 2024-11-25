using UnityEngine;
using System.Linq;
using System;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_bombPrefab = null;

    [SerializeField]
    private int[] m_chanceByGanre = null;

    [SerializeField]
    private float m_nextSpawnTime = 0.0f;

    private int m_totalChance = 0;

    private float m_spawnTimer = 0.0f;

    private const float m_maxRange = 5.0f;

    private const float m_minRange = -5.0f;

    private const float m_itemHeight = 7.6f; //ギリギリ画面外

    private Tuple<GameObject, int>[] m_bombData = null;

    private void Start()
    {
        CalculateTotalChance();
        m_bombData = MakebombData();//m_bombPrefabとm_chanceByGanreの値をまとめ、確率が高い順に並び替える。
    }
    // Update is called once per frame
    private void Update()
    {
        m_spawnTimer += Time.deltaTime;

        if (m_spawnTimer >= m_nextSpawnTime)
        {
            GenerateBomb();            
            m_spawnTimer = 0;
        }
    }
    private void CalculateTotalChance()
    {
        for(int i = 0; i < m_chanceByGanre.Length;i++)
        {
            m_totalChance += m_chanceByGanre[i];
        }
    }
    private void GenerateBomb()
    {
        Instantiate(ChooseGanre(RandomNumber()), new Vector2(UnityEngine.Random.Range(m_minRange, m_maxRange), m_itemHeight), Quaternion.identity, transform);
    }
    private int RandomNumber()
    {  
        int randomNumber = UnityEngine.Random.Range(0, m_totalChance);
        return randomNumber;
    }
    private GameObject ChooseGanre(int numberToChoose)
    {
        int numberToChoose_ = numberToChoose;

        for (int i = 0; i < m_bombData.Length; i++)
        {
            if (numberToChoose_ < m_bombData[i].Item2)
            {
                return m_bombData[i].Item1;
            }
            else
            {
                numberToChoose_ -= m_bombData[i].Item2;
            }
        }
        return m_bombData[^1].Item1;
    }
    private Tuple<GameObject,int>[] MakebombData()
    {
        var makeTuple = m_bombPrefab.Zip(m_chanceByGanre, (prefab, chance) => Tuple.Create(prefab, chance));
        makeTuple.OrderByDescending(item => item.Item2);
        var maketupleArray = makeTuple.ToArray();
        return maketupleArray;
    }
    private void OnValidate()
    {
        if (m_chanceByGanre == null || m_bombPrefab == null)
        { return; }
     //インスペクターにプレハブをひとつ登録すると確率の欄も自動で増える。
        if(m_chanceByGanre.Length != m_bombPrefab.Length)
        {
            System.Array.Resize(ref m_chanceByGanre, m_bombPrefab.Length);
        }
    }
}
