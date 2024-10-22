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

    [SerializeField,Tooltip("�A�C�e�������Ԋu/�b")]
    float m_nextSpawnTime = 60.0f; 

    //�e�v���n�u�̏o����
    [SerializeField, Tooltip("�A�C�e���̏o�����ł��B�e�A�C�e���̏o�����́A���v��100�ɂȂ�悤�ɐݒ肵�Ă��������B")]
    int m_chanceOfNormal = 50;
    [SerializeField, Tooltip("�A�C�e���̏o�����ł��B�e�A�C�e���̏o�����́A���v��100�ɂȂ�悤�ɐݒ肵�Ă��������B")]
    int m_chanceOfImpulse = 25;
    [SerializeField, Tooltip("�A�C�e���̏o�����ł��B�e�A�C�e���̏o�����́A���v��100�ɂȂ�悤�ɐݒ肵�Ă��������B")]
    int m_chanceOfMini = 25;

    private float m_spawnTimer = 0.0f;// �^�C�}�[

    //�A�C�e�������͈�
    private float m_maxRange = 5.0f;
    private float m_minRange = -5.0f;
    private float m_itemHeight = 7.6f; //�M���M����ʊO�ɐ���
    

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
            //m_nextSpawnTime�b���ƂɃA�C�e���̃v���n�u�������_������

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



        //0�`�v���n�u�̏o���m���̍��v�܂ł̐����������_������
        
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
