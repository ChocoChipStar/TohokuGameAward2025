using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCrownGenerator : MonoBehaviour
{
    [SerializeField]
    private SpecialCrownData m_spCrownData = null;

    List<Vector3> m_generatePosList = new List<Vector3>();

    bool m_isGenerateSpacialCrown = false;

    private void GenerateCrown()
    {
        if(!m_isGenerateSpacialCrown)
        {
            return;
        }

        if(m_generatePosList.Count == 0 || m_generatePosList == null)
        {
            m_generatePosList = new List<Vector3>(m_spCrownData.Positions.GeneratePos);
        }

        for (int i = 0; i < m_spCrownData.Params.GenerateCountOneTime; i++)
        {
            var generateObj = m_spCrownData.Prefabs;
            var currentPosIndex = UnityEngine.Random.Range(0, m_generatePosList.Count);

            Instantiate(generateObj, m_generatePosList[currentPosIndex], Quaternion.identity, this.transform);
            m_generatePosList.RemoveAt(currentPosIndex);
        }
    }

    private void SpawnSpecialCrown()
    {
        if (m_isGenerateSpacialCrown && transform.childCount == 0)
        {
            GenerateCrown();
        }
    }

    private void SetGenerateSpacialCrown()
    {
        m_isGenerateSpacialCrown = true;
    }

    public IEnumerator GenerateStartSpecialCrown()
    {
        yield return new WaitForSeconds(m_spCrownData.Params.GenerateStartTime);//指定の時間がたったら生成開始

        SetGenerateSpacialCrown();
    }

    void Update()
    {
        SpawnSpecialCrown();
    }
}
//タイマーから残り時間を取得し、タイマーが残り時間以下になったらダイヤを出現させる。