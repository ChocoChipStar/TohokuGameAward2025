using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private CannonDictance m_dictanceManager = null;

    [SerializeField]
    private SoundEffectManager m_soundEffectManager = null;

    private int m_cannonCount = 0;
    public static readonly int CannonMax = 2;

    public GameObject GenerateCannon(GameObject cannon)
    {
        var cannonObject = Instantiate(cannon, Vector3.zero, Quaternion.identity, this.transform);
        InitializeCannon(cannonObject);
        return cannonObject;
    }

    private void InitializeCannon(GameObject cannon)
    {
        CannonMover[] cannonMover = new CannonMover[CannonMax];
        cannonMover[m_cannonCount] = cannon.GetComponent<CannonMover>();
        cannonMover[m_cannonCount].CannonInitialize(m_cannonCount);
        m_dictanceManager.GetCannonMover(cannonMover[m_cannonCount], m_cannonCount);
        m_cannonCount++;
    }

    public void PlaySoundEffect()
    {
        m_soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.Cannon);
    }
}
