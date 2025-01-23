using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private GameObject[] m_instances = null;
   
    public GameObject[] Instances { get { return m_instances; } }

    private void Awake()
    {
        GetGamePadInstances();
    }

    /// <summary>
    /// instancesを取得します
    /// </summary>
    public void GetGamePadInstances()
    {
        var gamepads = Gamepad.all;
        m_instances = new GameObject[gamepads.Count];
    }

    /// <summary>
    /// プレイヤー動作の有無を設定します
    /// </summary>
    /// <param name="index"> プレイヤー番号 </param>
    /// <param name="isActive"> true -> 操作可能 false -> 操作不可 </param>
    public void SetMovement(int index, bool isActive)
    {
        var playerMover = m_instances[index].GetComponent<HumanoidMover>();
        if (playerMover != null)
        {
            playerMover.enabled = isActive;
            return;
        }

        var cannonMover = m_instances[index].GetComponent<CannonMover>();
        if (cannonMover != null)
        {
            cannonMover.enabled = isActive;
            return;
        }
    }

    /// <summary>
    /// プレイヤーのinstancesの番号取得
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerIndex(GameObject player)
    {
        for (int i = 0; i < m_instances.Length; i++)
        {
            if (m_instances[i] == player)
            {
                return i;
            }
        }
        return -1;
    }
}
