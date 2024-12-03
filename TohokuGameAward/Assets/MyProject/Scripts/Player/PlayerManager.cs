using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerPrefab = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    private GameObject[] m_playerCount = null;

    private bool m_isOnlyOnePlayer = false;

    public GameObject[] PlayerCount
    {
        get { return m_playerCount; }
    }

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
        m_playerCount = new GameObject[gamepads.Count];
        for (int i = 0; i < gamepads.Count; i++)
        {
            var instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[i], Quaternion.identity, this.transform);
            instance.name = "Player" + (i + 1);
            m_playerCount[i] = instance;
            
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
