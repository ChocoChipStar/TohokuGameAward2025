using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerPrefab = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    private void Awake()
    {
       CreatePlayerBasedOnControllers();
    }

    private void CreatePlayerBasedOnControllers()
    {
        var gamepads = Gamepad.all;
        for (int i = 0; i < gamepads.Count; i++)
        {
            var instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[i], Quaternion.identity);
            instance.name = "Player" + (i + 1);

            var inputData = instance.GetComponent<PlayerInputData>();
            if (inputData != null)
            {
                inputData.SetNumber(i);
            }
        }
    }
}
