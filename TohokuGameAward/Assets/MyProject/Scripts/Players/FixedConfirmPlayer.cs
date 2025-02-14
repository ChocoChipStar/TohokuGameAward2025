using UnityEngine;

public class FixedConfirmPlayer : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private Vector2[] fixedConfirmPos = new Vector2[PlayerManager.PlayerMax];

    private void Start()
    {
        for(int i = 0;  i < PlayerManager.PlayerMax; i++)
        {
            m_playerManager.Instances[i].transform.position = fixedConfirmPos[i];
        }
    }
}
