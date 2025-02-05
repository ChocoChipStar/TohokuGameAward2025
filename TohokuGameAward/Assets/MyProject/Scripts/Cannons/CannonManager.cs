using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    public static readonly int CannonMax = 2;

    public void SetOperable(bool isEnabled)
    {
        for(int i = 0; i < TeamGenerator.MembersCount; i++)
        {
            var cannonMover = m_playerManager.CannonInstances[i].GetComponent<CannonMover>();
            cannonMover.SetOperable(isEnabled);

            var cannonAttack = m_playerManager.CannonInstances[i].GetComponent<CannonAttack>();
            cannonAttack.SetOperable(isEnabled);
        }
    }
}
