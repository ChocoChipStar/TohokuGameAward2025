using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField]
    private CannonMover[] m_playerRailMover = new CannonMover[2];

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private SoundEffectManager m_soundEffectManager = null;

    private int m_cannonCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (IsCannonCompare())
        {
            CannonDistanceChacker();
        }
    }

    //大砲同士がすり抜けないようにする
    private void CannonDistanceChacker()
    {
        for(int i = 0; i < m_cannonCount - 1; i++)
        {
            if (m_playerRailMover[i].CannonPosition + m_cannonData.Params.CannonDictance > m_playerRailMover[i + 1].CannonPosition)
            {
                var basisPosition = m_playerRailMover[i].CannonPosition;
                m_playerRailMover[i].FixCannonPosition(m_playerRailMover[i + 1].CannonPosition - (m_cannonData.Params.CannonDictance));
                m_playerRailMover[i + 1].FixCannonPosition(basisPosition + (m_cannonData.Params.CannonDictance));
            }
        }
    }

    //二台以上の大砲があるかを調べる
    private bool IsCannonCompare()
    {
        if (m_playerRailMover.Length >= m_cannonData.Params.CannonCount)
        {
            return true;
        }
        return false;
    }

    //大砲が作成される処理
    public GameObject GenerateCannon(GameObject cannon)
    {
        var cannonObject = Instantiate(cannon, Vector3.zero, Quaternion.identity, this.transform);
        
        m_playerRailMover[m_cannonCount] = cannonObject.GetComponent<CannonMover>();
        m_playerRailMover[m_cannonCount].CreateStart(m_cannonCount);
        m_cannonCount++;

        return cannonObject;
    }

    public void PlaySoundEffect()
    {
        m_soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.Cannon);
    }
}
