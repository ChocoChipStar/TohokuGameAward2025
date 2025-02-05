using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownGenerator : MonoBehaviour
{
    [SerializeField]
    private CrownSelector m_crownSelector = null;

    [SerializeField]
    private CrownData m_crownData = null;

    /// <summary>
    /// ステージ上のあらゆる位置に王冠を複数個生成します
    /// </summary>
    private void GenerateCrown()
    {
        var generatePosList = new List<Vector3>(m_crownData.Positions.GeneratePos);
        if(generatePosList.Count == 0)
        {
            throw new ArgumentNullException("王冠を生成する座標が設定されていません");
        }

        for (int i = 0; i < m_crownData.Params.GenerateCountOneTime; i++)
        {
            var generateObj = m_crownSelector.SelectionCrown();
            var currentPosIndex = UnityEngine.Random.Range(0, generatePosList.Count);

            Instantiate(generateObj, generatePosList[currentPosIndex], Quaternion.identity, this.transform);
            generatePosList.RemoveAt(currentPosIndex);
        }
    }

    /// <summary>
    /// ステージ上に存在する全ての王冠を削除します
    /// </summary>
    private void DestroyCrowns()
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    public IEnumerator GenerateStart()
    {
        while (true)
        {
            GenerateCrown();

            // 消滅する時間が来るまで待機
            yield return new WaitForSeconds(m_crownData.Params.LifeTime);

            DestroyCrowns();

            // 生成スパン時間が来るまで待機
            yield return new WaitForSeconds(m_crownData.Params.GenerateSpan);
        }
    }
}
