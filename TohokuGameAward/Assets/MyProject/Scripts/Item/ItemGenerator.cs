using System.Globalization;
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
    private float m_nextSpawnTime = 0.0f;

    [SerializeField]
    private int m_chanceOfNormal = 0;

    [SerializeField]
    private int m_chanceOfImpulse = 0;

    [SerializeField]
    private int m_chanceOfMini = 0;

    private int m_totalChance = 0;

    private float m_spawnTimer = 0.0f;

    private const float m_maxRange = 5.0f;

    private const float m_minRange = -5.0f;

    private const float m_itemHeight = 7.6f; //ƒMƒŠƒMƒŠ‰æ–ÊŠO

    private void Start()
    {
        m_totalChance = m_chanceOfNormal + m_chanceOfImpulse + m_chanceOfMini;
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
    private void GenerateBomb()
    {
        Instantiate(SelectRandomPrefab(), new Vector2(Random.Range(m_minRange, m_maxRange), m_itemHeight), Quaternion.identity, transform);
    }
    private enum GenreOfBomb
    {
        Normal,
        Impulse,
        Mini
    }
    private GameObject SelectRandomPrefab()
    {
        GameObject normalBomb = m_normalBombPrefab;
        GameObject impulseBomb = m_impulseBombPrefab;
        GameObject miniBomb = m_miniBombPrefab;

        GenreOfBomb genreOfBomb = SerectGenreOfBomb();

        if (genreOfBomb == GenreOfBomb.Normal)
        {
            return normalBomb;
        }
        if (genreOfBomb == GenreOfBomb.Impulse)
        {
            return impulseBomb;
        }
        if (genreOfBomb == GenreOfBomb.Mini)
        {
            return miniBomb;
        }

        return normalBomb;
    }
   
    private GenreOfBomb SerectGenreOfBomb()
    {
        int numberToSelect = NumberToSerectBomb();

        bool isNormal = 0 <= numberToSelect && numberToSelect < m_chanceOfNormal;
        bool isImpulse = m_chanceOfNormal <= numberToSelect && numberToSelect < m_chanceOfNormal + m_chanceOfImpulse;
        bool isMini = m_chanceOfNormal + m_chanceOfImpulse <= numberToSelect && numberToSelect < m_totalChance;

        if (isNormal)
        {return GenreOfBomb.Normal;}
        
        if (isImpulse)
        { return GenreOfBomb.Impulse;}

        if (isMini)
        { return GenreOfBomb.Mini;}

        return GenreOfBomb.Normal;
    }

    private int NumberToSerectBomb()
    {
        int numberToSelect = Random.Range(0, m_totalChance);
        return numberToSelect;
    }
}
