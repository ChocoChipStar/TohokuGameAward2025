using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private CannonDictanceManager m_dictanceManager = null;

    [SerializeField]
    private CannonMover[] m_CannonMover = new CannonMover[CannonMax];

    [SerializeField]
    private SoundEffectManager m_soundEffectManager = null;

    private int m_cannonCount = 0;
    private const int CannonMax = 2;

    public GameObject GenerateCannon(GameObject cannon)
    {
        var cannonObject = Instantiate(cannon, Vector3.zero, Quaternion.identity, this.transform);
        InitializeCannon(cannonObject);
        return cannonObject;
    }

    private void InitializeCannon(GameObject cannon)
    {
        m_CannonMover[m_cannonCount] = cannon.GetComponent<CannonMover>();
        m_CannonMover[m_cannonCount].CannonInitialize(m_cannonCount);
        m_dictanceManager.GetCannonMover(m_CannonMover[m_cannonCount], m_cannonCount);
        m_cannonCount++;
    }

    public void PlaySoundEffect()
    {
        m_soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.Cannon);
    }
}
