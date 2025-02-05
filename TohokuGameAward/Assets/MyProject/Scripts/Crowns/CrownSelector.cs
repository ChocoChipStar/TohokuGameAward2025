using System.Collections.Generic;
using UnityEngine;

public class CrownSelector : MonoBehaviour
{
    [SerializeField]
    private CrownData m_crownData = null;

    private float m_totalRate = 0;

    private List<float> m_crownRateList = new List<float>();

    private void Start()
    {
        InitializeCrownRate();
    }

    private void InitializeCrownRate()
    {
        m_crownRateList.Add(m_crownData.Rate.SilverRate);
        m_crownRateList.Add(m_crownData.Rate.GoldRate);
        m_crownRateList.Add(m_crownData.Rate.DiamondRate);
        for(int i = 0; i < m_crownRateList.Count; i++)
        {
            m_totalRate += m_crownRateList[i];
        }
    }

    /// <summary>
    /// 今回生成する王冠を選出します
    /// </summary>
    /// <returns> 確定した王冠のオブジェクトを返します </returns>
    public GameObject SelectionCrown()
    {
        var randomValue = UnityEngine.Random.Range(0, m_totalRate);
        var cumulativeValue = 0.0f;
        for(int i = 0; i < (int)CrownData.CrownPattern.Max; i++)
        {
            cumulativeValue += m_crownRateList[i];
            if(randomValue < cumulativeValue)
            {
                return m_crownData.Prefabs[i];
            }
        }
        return m_crownData.Prefabs[0];
    }
}
