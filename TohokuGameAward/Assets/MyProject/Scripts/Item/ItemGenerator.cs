using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    
    [SerializeField]
    private GameObject m_normalBombPrefab;

    [SerializeField]
    private GameObject m_impulseBombPrefab;

    [SerializeField]
    private GameObject m_miniBombPrefab;

    [SerializeField,Tooltip("アイテム生成間隔/秒")]
    float m_nextSpawnTime = 60.0f; 

    //各プレハブの出現率
    [SerializeField, Tooltip("アイテムの出現率です。各アイテムの出現率は、合計が100になるように設定してください。")]
    int m_chanceOfNormal = 50;
    [SerializeField, Tooltip("アイテムの出現率です。各アイテムの出現率は、合計が100になるように設定してください。")]
    int m_chanceOfImpulse = 25;
    [SerializeField, Tooltip("アイテムの出現率です。各アイテムの出現率は、合計が100になるように設定してください。")]
    int m_chanceOfMini = 25;

    private float m_spawnTimer = 0.0f;// タイマー

    //アイテム生成範囲
    private float m_maxRange = 5.0f;
    private float m_minRange = -5.0f;
    private float m_itemHeight = 7.6f; //ギリギリ画面外に生成
    

    // Start is called before the first frame update
    void Start()
    {
     
        
        

    }

    // Update is called once per frame
    void Update()
    {
        m_spawnTimer += Time.deltaTime;

        if (m_spawnTimer >= m_nextSpawnTime)
        {
            //m_nextSpawnTime秒ごとにアイテムのプレハブをランダム生成

            Instantiate(SelectRandomPrefab(), new Vector2(Random.Range(m_minRange, m_maxRange), m_itemHeight), Quaternion.identity,transform);
            
            m_spawnTimer = 0;
        }
       
    }

    private void FixedUpdate()
    {
        
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
