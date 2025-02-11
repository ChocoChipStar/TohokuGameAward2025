using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private Transform m_bombManager = null;

    public static readonly int CannonMax = 2;

    public Transform BombManager { get { return m_bombManager; } private set { m_bombManager = value; } }

    public void SetOperable(bool isEnabled)
    {
        for(int i = 0; i < TeamGenerator.MembersCount; i++)
        {
            var cannonMover = m_playerManager.CannonInstances[i].GetComponent<CannonMover>();
            cannonMover.SetOperable(isEnabled);

            var cannonBombShoot = m_playerManager.CannonInstances[i].GetComponent<CannonBombShoot>();
            cannonBombShoot.SetOperable(isEnabled);
        }
    }
}
