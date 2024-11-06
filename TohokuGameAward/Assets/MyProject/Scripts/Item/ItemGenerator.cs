using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_BombPrefab = null;

    [SerializeField]
    private int[] ChanceByGanre = null;

    [SerializeField]
    private float m_nextSpawnTime = 0.0f;

    private int m_totalChance = 0;

    private float m_spawnTimer = 0.0f;

    private const float m_maxRange = 5.0f;

    private const float m_minRange = -5.0f;

    private const float m_itemHeight = 7.6f; //ギリギリ画面外

    private void Start()
    {
        CalculateTotalChance();
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
        for(int i = 0; i < ChanceByGanre.Length;i++)
        {
            m_totalChance += ChanceByGanre[i];
        }
    }
    private void GenerateBomb()
    {
        Instantiate(SerectGenreOfBomb(), new Vector2(Random.Range(m_minRange, m_maxRange), m_itemHeight), Quaternion.identity, transform);
    }
     private GameObject SerectGenreOfBomb()
    {
        int numberToSelect = ChooseGanre(RandomNumber());
        return m_BombPrefab[numberToSelect];
    }
    private int ChooseGanre(int numberToChoose)
    {
        int numberToChoose_ = numberToChoose;
        for (int i = 0; i < ChanceByGanre.Length; i++)
        {
            if(numberToChoose_ < ChanceByGanre[i])
            {
                return i;
            }
            else
            {
                numberToChoose_ -= ChanceByGanre[i];
            }
        }
        return ChanceByGanre.Length;
    }
private int RandomNumber()
    {  
        int randomNumber = Random.Range(0, m_totalChance);
        return randomNumber;
    }
    private void OnValidate()
    {
        if (ChanceByGanre == null || m_BombPrefab == null)
        { return; }
     //インスペクターにプレハブをひとつ登録すると確率の欄も自動で増えます。
        if(ChanceByGanre.Length != m_BombPrefab.Length)
        {
            System.Array.Resize(ref ChanceByGanre, m_BombPrefab.Length);
        }
    }
}
