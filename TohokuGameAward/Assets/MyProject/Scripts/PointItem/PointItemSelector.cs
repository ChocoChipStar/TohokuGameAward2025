﻿using System;
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

        m_totalChanceOfPointItem = TotalPointItemChance();
    }

    /// <summary>
    /// リングのプレハブと出現確率を対応させた表を作ります。
    /// </summary>
    /// <returns></returns>
    private Tuple<GameObject, float>[] MakeRingPointItemData()
    {
        var makeTuple = m_pointData.Items.Prefab.Zip(m_pointData.Chances.ChanceOfItem, (prefab, chance) => Tuple.Create(prefab, chance));
        makeTuple.OrderByDescending(item => item.Item2);
        var maketupleArray = makeTuple.ToArray();
        return maketupleArray;
    }

    private float TotalPointItemChance()
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
