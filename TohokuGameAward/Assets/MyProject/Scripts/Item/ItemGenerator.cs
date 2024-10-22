using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_normalBombPrefab = null;

    [SerializeField]
    private GameObject m_impulseBombPrefab = null;

    [SerializeField]
    private GameObject m_miniBombPrefab = null;

    [SerializeField]
    private float m_nextSpawnTime = 60.0f;

    [SerializeField]
    private int m_chanceOfNormal = 50;

    [SerializeField]
    private int m_chanceOfImpulse = 25;

    [SerializeField]
    private int m_chanceOfMini = 25;

    private float m_spawnTimer = 0.0f;

    private float m_maxRange = 5.0f;
    private float m_minRange = -5.0f;
    private float m_itemHeight = 7.6f; //ギリギリ画面外

    // Update is called once per frame
    private void Update()
    {
        m_spawnTimer += Time.deltaTime;

        if (m_spawnTimer >= m_nextSpawnTime)
        {
            //m_nextSpawnTime秒ごとにアイテムのプレハブをランダム生成

            Instantiate(SelectRandomPrefab(), new Vector2(Random.Range(m_minRange, m_maxRange), m_itemHeight), Quaternion.identity,transform);
            
            m_spawnTimer = 0;
        }
       
    }
    private GameObject SelectRandomPrefab()
    {
        GameObject normalBomb = m_normalBombPrefab;
        GameObject ImpulseBomb = m_impulseBombPrefab;
        GameObject miniBomb = m_miniBombPrefab;

        //0～プレハブの出現確率の合計までの数字をランダム生成
        
        int totalChance = m_chanceOfNormal + m_chanceOfImpulse + m_chanceOfMini;
        int numberToSelect = Random.Range(0, totalChance);

        bool isNormal = 0 <= numberToSelect && numberToSelect < m_chanceOfNormal;
        bool isImpulse = m_chanceOfNormal <= numberToSelect && numberToSelect < m_chanceOfNormal + m_chanceOfImpulse;
        bool ismini = m_chanceOfNormal + m_chanceOfImpulse <= numberToSelect && numberToSelect < totalChance; 

        if (isNormal)
        {
            return normalBomb;
        }
        if (isImpulse)
        {
            return ImpulseBomb;
        }
        if (ismini)
        {
            return miniBomb;
        }

        return normalBomb;
    }
}
