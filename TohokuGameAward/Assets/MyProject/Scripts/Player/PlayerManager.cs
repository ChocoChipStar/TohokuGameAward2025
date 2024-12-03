using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerPrefab = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    private bool m_isOnlyOnePlayer = false;

    private void Awake()
    {
        CreatePlayerBasedOnControllers();
    }

    private void Update()
    {
        CountPlayers();
    }

    private void CreatePlayerBasedOnControllers()
    {
        var gamepads = Gamepad.all;
        for (int i = 0; i < gamepads.Count; i++)
        {
            var instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[i], Quaternion.identity, this.transform);
            instance.name = "Player" + (i + 1);

            var inputData = instance.GetComponent<PlayerInputData>();
            if (inputData != null)
            {
                inputData.SetNumber(i);
            }
        }
    }

    private int CountPlayers()
    {
        int count = this.transform.childCount;

        if (count <= 1)
        {
            m_isOnlyOnePlayer = true;
        }

        return count;
    }

    public bool GetOnlyOnePlayer()
    {
        return m_isOnlyOnePlayer;
    }
}
