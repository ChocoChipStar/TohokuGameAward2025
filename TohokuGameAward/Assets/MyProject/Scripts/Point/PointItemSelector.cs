using System;
using System.Linq;
using UnityEngine;

public class PointItemSelector : MonoBehaviour
{
    [SerializeField]
    PointData m_pointData = null;

    private Tuple<GameObject, float>[] m_pointItemData = null;

    private float m_totalChanceOfPointItem = 0;

    void Start()
    {
        m_pointItemData = MakeRingPointItemData();

        m_totalChanceOfPointItem = TotalChanceOfPointItem();
    }

    /// <summary>
    /// リングのプレハブと出現確率を対応させた表を作ります。
    /// 両方を対応させつつ並び替えたいため、Tapleを使用しています。
    /// </summary>
    /// <returns></returns>
    private Tuple<GameObject, float>[] MakeRingPointItemData()
    {
        //m_pointData.Items.Prefab の各要素（prefab）と、m_pointData.Chances.ChanceOfItem の各要素（chance）を
        //1対1で組み合わせてTapleと呼ばれる表を作成しています。
        var makeTuple = m_pointData.Items.Prefab.Zip(m_pointData.Chances.ChanceOfItem, (prefab, chance) 
            　　　　　=> Tuple.Create(prefab, chance));

        //Tapleの機能を使用し、Item2（chance）の値を基準に降順に並び替えています。
        var sortedTuple = makeTuple.OrderByDescending(item => item.Item2);

        //並び替えたTapleを配列に変換しています。
        var maketupleArray = sortedTuple.ToArray(); 

        return maketupleArray;
    }

    private float TotalChanceOfPointItem()
    {
        float total = 0;

        for (int i = 0; i < m_pointData.Chances.ChanceOfItem.Length; i++)
        {
            total += m_pointData.Chances.ChanceOfItem[i];
        }

        return total;
    }

    public GameObject ChoosePointItem()
    {
        //m_pointItemDataには、確率順に並んだプレハブのデータが入っています。
        //m_pointItemData[i].Item1にはPointDataのm_pointItemPrefab[i]と同じ値が入っています。
        //m_pointItemData[i].Item2にはPointDataのm_chanceOfPointItem[i]と同じ値が入っています。

        float numberToChoose = UnityEngine.Random.Range(0, m_totalChanceOfPointItem);

        for (int i = 0; i < m_pointItemData.Length; i++)
        {
            if (numberToChoose < m_pointItemData[i].Item2)
            {
                return m_pointItemData[i].Item1;
            }
            else
            {
                numberToChoose -= m_pointItemData[i].Item2;
            }
        }
        return m_pointItemData[^1].Item1;
    }
}
