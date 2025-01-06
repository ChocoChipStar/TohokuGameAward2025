using System.Collections.Generic;
using UnityEngine;

public class PointItemGenerator : MonoBehaviour
{
    [SerializeField]
    PointItemSelector m_pointItemSelector = null;

    [SerializeField]
    List<Vector3> m_rotatingItemPos = null;

    [SerializeField]
    float m_itemAngle = 0;

    [SerializeField]
    List<Vector3> m_staticItemPos = null;

    [SerializeField]
    int m_maxItems = 0;

    [SerializeField]
    float m_spawnInterval = 0;

    [SerializeField]
    float m_itemLifetime = 0;

    float m_timeCounter = 0;

    bool m_isItemActive = false;

    void Update()
    {
        if (m_timeCounter > m_spawnInterval && !m_isItemActive)
        {
            GeneratePointItem();
        }
        if (m_timeCounter > m_itemLifetime && m_isItemActive)
        {
            DestroyPointItem();
        }
        m_timeCounter += Time.deltaTime;
    }

    private void GeneratePointItem()
    {
        List<Vector3> rotatingItemPosCopy = new List<Vector3>(m_rotatingItemPos);
        List<Vector3> staticItemPosCopy = new List<Vector3>(m_staticItemPos);

        for (int i = 0; i < m_maxItems; i++)
        {
            GameObject randomItemPrefab = m_pointItemSelector.ChoosePointItem();
            int totalSpawnPos = rotatingItemPosCopy.Count + staticItemPosCopy.Count;
            int randomPos = UnityEngine.Random.Range(0, totalSpawnPos - 1);

            if(0 < rotatingItemPosCopy.Count && IsChosen(randomPos,rotatingItemPosCopy.Count))
            {
                GenerateRotatingItem(randomItemPrefab, rotatingItemPosCopy[randomPos]);
                rotatingItemPosCopy.RemoveAt(randomPos);
                m_isItemActive = true;
                continue;
            }

            randomPos -= rotatingItemPosCopy.Count;　//rotatingRingPosに該当しなければstaticRingPosで生成する。

            if (0 < staticItemPosCopy.Count && IsChosen(randomPos,staticItemPosCopy.Count))
            {
                GenerateStaticItem(randomItemPrefab, staticItemPosCopy[randomPos]);
                staticItemPosCopy.RemoveAt(randomPos);
                m_isItemActive = true;
                continue;
            }

            Debug.Log("存在しないポジションが選ばれています");
        }
    }

    private bool IsChosen(int randomPos,int count)
    {
        if(randomPos < count)
        {
            return true;
        }
        return false;
    }

    private void GenerateRotatingItem(GameObject ringPrefab,Vector3 pos)
    {
        Instantiate(ringPrefab, pos, Quaternion.Euler(0, m_itemAngle, 0), transform);
    }

    private void GenerateStaticItem(GameObject ringPrefab, Vector3 pos)
    {
        Instantiate(ringPrefab, pos, Quaternion.identity, transform);
    }

    private void DestroyPointItem()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject); 
        }

        m_timeCounter = 0;
        m_isItemActive = false;
    }
}
