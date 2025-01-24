using UnityEngine;

public class CannonDictanceManager : MonoBehaviour
{
    [SerializeField]
    private CannonManager m_cannonManager = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private CannonMover[] m_CannonMover = new CannonMover[CannonMax];

    private const int CannonMax = 2;

    private bool m_isHitCannon = false;

    public bool IsHitCannon { get { return m_isHitCannon; } }

    void Update()
    {
        if (CanCannonCompare())
        {
            DistanceChacker();
        }
    }

    /// <summary>
    /// 大砲同士がすり抜けないようにする
    /// </summary>
    private void DistanceChacker()
    {
        for (int i = 0; i < CannonMax - 1; i++)
        {
            if (m_CannonMover[i].CannonPosition + m_cannonData.Params.CannonDictance > m_CannonMover[i + 1].CannonPosition)
            {
                m_isHitCannon = true;
                var basisPosition = m_CannonMover[i].CannonPosition;
                m_CannonMover[i].FixCannonPosition(m_CannonMover[i + 1].CannonPosition - (m_cannonData.Params.CannonDictance));
                m_CannonMover[i + 1].FixCannonPosition(basisPosition + (m_cannonData.Params.CannonDictance));
            }
            else
            {
                m_isHitCannon = false;
            }
        }
    }

    /// <summary>
    /// 二台以上の大砲があるかを調べる
    /// </summary>
    private bool CanCannonCompare()
    {
        if (m_CannonMover.Length >= m_cannonData.Params.CannonCount)
        {
            return true;
        }
        return false;
    }

    public void GetCannonMover(CannonMover cannonMover, int number)
    {
        m_CannonMover[number] = cannonMover;
    }
}
