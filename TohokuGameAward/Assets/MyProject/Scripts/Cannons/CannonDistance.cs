using UnityEngine;

public class CannonDistance : MonoBehaviour
{
    [SerializeField]
    private CannonManager m_cannonManager = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private CannonMover[] m_cannonMover = new CannonMover[CannonManager.CannonMax];

    private bool m_isHitCannon = false;

    public bool IsHitCannon { get { return m_isHitCannon; } }

    void Update()
    {
        if (CanCannonCompare())
        {
            DistanceChecker();
        }
    }

    /// <summary>
    /// 大砲同士がすり抜けないようにする
    /// </summary>
    private void DistanceChecker()
    {
        for (int i = 0; i < CannonManager.CannonMax - 1; i++)
        {
            if (m_cannonMover[i].CannonPosition + m_cannonData.Params.CannonDictance > m_cannonMover[i + 1].CannonPosition)
            {
                m_isHitCannon = true;
                var basisPosition = m_cannonMover[i].CannonPosition;
                m_cannonMover[i].FixCannonPosition(m_cannonMover[i + 1].CannonPosition - (m_cannonData.Params.CannonDictance));
                m_cannonMover[i + 1].FixCannonPosition(basisPosition + (m_cannonData.Params.CannonDictance));
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
        if (m_cannonMover.Length >= m_cannonData.Params.CannonCount)
        {
            return true;
        }
        return false;
    }

    public void GetCannonMover(CannonMover cannonMover, int number)
    {
        m_cannonMover[number] = cannonMover;
    }
}
