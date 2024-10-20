using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    //�V���A���C�Y�t�B�[���h�Ő����J�n���Ԃ�ύX�ł���悤�ɂ��Ă���
    [SerializeField]
    private GameObject normalBombPrefab;

    [SerializeField]
    private GameObject impulseBombPrefab;

    [SerializeField]
    private GameObject miniBombPrefab;

    [SerializeField,Tooltip("�A�C�e�������Ԋu/�b")]
    float nextSpawnTime = 60.0f; 

    //�e�v���n�u�̏o����
    [SerializeField, Tooltip("�A�C�e���̏o�����ł��B�e�A�C�e���̏o�����́A���v��100�ɂȂ�悤�ɐݒ肵�Ă��������B")]
    int chanceOfNormal = 50;
    [SerializeField, Tooltip("�A�C�e���̏o�����ł��B�e�A�C�e���̏o�����́A���v��100�ɂȂ�悤�ɐݒ肵�Ă��������B")]
    int chanceOfImpulse = 25;
    [SerializeField, Tooltip("�A�C�e���̏o�����ł��B�e�A�C�e���̏o�����́A���v��100�ɂȂ�悤�ɐݒ肵�Ă��������B")]
    int chanceOfMini = 25;

    private float spawnTimer = 0.0f;// �^�C�}�[

    //�A�C�e�������͈�
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
            //nextSpawnTime�b���ƂɃA�C�e���̃v���n�u�������_������
            Instantiate(SelectRandomPrefab(), new Vector2(Random.Range(minRange, maxRange), 0.0f), Quaternion.identity);
            spawnTimer = 0;
        }
       
    }

 

    private GameObject SelectRandomPrefab()
    {
        GameObject normalBomb = normalBombPrefab;
        GameObject ImpulseBomb = impulseBombPrefab;
        GameObject miniBomb = miniBombPrefab;



        //0�`�v���n�u�̏o���m���̍��v�܂ł̐����������_������
        
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
