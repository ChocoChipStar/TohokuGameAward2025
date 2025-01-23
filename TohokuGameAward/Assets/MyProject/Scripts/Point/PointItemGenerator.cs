using System.Collections.Generic;
using UnityEngine;

public class PointItemGenerator : MonoBehaviour
{
    [SerializeField]
    PointItemSelector m_pointItemSelector = null;

    [SerializeField]
    PointData m_pointData = null;  

    float m_timeCounter = 0;

    bool m_isItemActive = false;

    void Update()
    {
        if (m_timeCounter > m_pointData.Params.SpawnInterval && !m_isItemActive)
        {
            GeneratePointItem();
        }
        if (m_timeCounter > m_pointData.Params.ItemLifeTime && m_isItemActive)
        {
            DestroyPointItem();
        }
        m_timeCounter += Time.deltaTime;
    }

    private void GeneratePointItem()
    {
        List<Vector3> rotatingItemPosCopy = new List<Vector3>(m_pointData.Positions.RotatingItemPos);
        List<Vector3> staticItemPosCopy = new List<Vector3>(m_pointData.Positions.StaticItemPos);

        for (int i = 0; i < m_pointData.Params.MaxItem; i++)
        {
            GameObject randomItemPrefab = m_pointItemSelector.ChoosePointItem(); //ポイントアイテムの種類を選びます

            int totalSpawnPos = staticItemPosCopy.Count;
            int randomPos = UnityEngine.Random.Range(0, totalSpawnPos - 1);//全ての出現位置からランダムに1つ選びます。

            if (0 < staticItemPosCopy.Count)
            {
                GenerateStaticItem(randomItemPrefab, staticItemPosCopy[randomPos]);
                staticItemPosCopy.RemoveAt(randomPos);
                m_isItemActive = true;
            }
            else
            {
                Debug.Log("存在しないポジションが選ばれています");
            }
        }
    }

    private void GenerateStaticItem(GameObject ringPrefab, Vector3 pos)
    {
        Instantiate(ringPrefab, pos, Quaternion.identity, transform);
    }

    private void DestroyPointItem()
    {
        foreach (Transform child in this.transform)
        {
            if (TagManager.Instance.SearchedTagName(child.gameObject,TagManager.Type.Crown))
            {
                Destroy(child.gameObject);
            }
        }

        m_timeCounter = 0;
        m_isItemActive = false;
    }
}
