using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    //シリアライズフィールドで生成開始時間を変更できるようにしておく
    [SerializeField]
    private GameObject normalBombPrefab;

    [SerializeField]
    private GameObject impulseBombPrefab;

    [SerializeField]
    private GameObject miniBombPrefab;

    [SerializeField,Tooltip("アイテム生成間隔/秒")]
    float nextSpawnTime = 60.0f; 

    //各プレハブの出現率
    [SerializeField, Tooltip("アイテムの出現率です。各アイテムの出現率は、合計が100になるように設定してください。")]
    int chanceOfNormal = 50;
    [SerializeField, Tooltip("アイテムの出現率です。各アイテムの出現率は、合計が100になるように設定してください。")]
    int chanceOfImpulse = 25;
    [SerializeField, Tooltip("アイテムの出現率です。各アイテムの出現率は、合計が100になるように設定してください。")]
    int chanceOfMini = 25;

    private float spawnTimer = 0.0f;// タイマー

    //アイテム生成範囲
    private float maxRange = 5.0f;
    private float minRange = -5.0f;
    

    // Start is called before the first frame update
    void Start()
    {
     
        
        

    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= nextSpawnTime)
        {
            //nextSpawnTime秒ごとにアイテムのプレハブをランダム生成
            Instantiate(SelectRandomPrefab(), new Vector2(Random.Range(minRange, maxRange), 0.0f), Quaternion.identity);
            spawnTimer = 0;
        }
       
    }

 

    private GameObject SelectRandomPrefab()
    {
        GameObject normalBomb = normalBombPrefab;
        GameObject ImpulseBomb = impulseBombPrefab;
        GameObject miniBomb = miniBombPrefab;



        //0～プレハブの出現確率の合計までの数字をランダム生成
        
        int totalChance = chanceOfNormal + chanceOfImpulse + chanceOfMini;
        int numberToSelect = Random.Range(0, totalChance);

   
        bool isNormal = 0 <= numberToSelect && numberToSelect < chanceOfNormal;
        bool isImpulse = chanceOfNormal <= numberToSelect && numberToSelect < chanceOfNormal + chanceOfImpulse;
        bool ismini = chanceOfNormal + chanceOfImpulse <= numberToSelect && numberToSelect < totalChance; 

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

        return null;
    }
}
