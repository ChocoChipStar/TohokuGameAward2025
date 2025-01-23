using System.Collections.Generic;
using UnityEngine;

public class PointItemGenerator : MonoBehaviour
{
    [SerializeField]
    private PointItemSelector m_pointItemSelector = null;

    [SerializeField]
    private PointData m_pointData = null;  

    private float m_timeCounter = 0;

    private bool m_isItemActive = false;

    private void Update()
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
        List<Vector3> staticItemPos = new List<Vector3>(m_pointData.Positions.StaticItemPos);

        for (int i = 0; i < m_pointData.Params.MaxItem; i++)
        {
            GameObject randomItemPrefab = m_pointItemSelector.ChoosePointItem(); //ポイントアイテムの種類を選びます

            int totalSpawnPos = staticItemPos.Count;
            int randomPos = Random.Range(0, totalSpawnPos - 1);//全ての出現位置からランダムに1つ選びます。

            if (0 < staticItemPos.Count)
            {
                GenerateStaticItem(randomItemPrefab, staticItemPos[randomPos]);
                staticItemPos.RemoveAt(randomPos);
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
