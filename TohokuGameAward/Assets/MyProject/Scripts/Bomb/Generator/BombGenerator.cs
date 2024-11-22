using UnityEngine;

public class BombGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_bombPrefab = new GameObject[BombData.BombMax];

    [SerializeField]
    private float m_nextSpawnTime = 60.0f;

    [SerializeField]
    private int m_chanceOfNormal = 50;

    [SerializeField]
    private int m_chanceOfImpulse = 25;

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
        GameObject normalBomb = m_bombPrefab[(int)BombData.BombType.Normal];
        GameObject ImpulseBomb = m_bombPrefab[(int)BombData.BombType.Impulse];

        //0～プレハブの出現確率の合計までの数字をランダム生成
        
        int totalChance = m_chanceOfNormal + m_chanceOfImpulse;
        int numberToSelect = Random.Range(0, totalChance);

        bool isNormal = 0 <= numberToSelect && numberToSelect < m_chanceOfNormal;
        bool isImpulse = m_chanceOfNormal <= numberToSelect && numberToSelect < m_chanceOfNormal + m_chanceOfImpulse;

        if (isNormal)
        {
            return normalBomb;
        }
        if (isImpulse)
        {
            return ImpulseBomb;
        }

        return normalBomb;
    }
}
