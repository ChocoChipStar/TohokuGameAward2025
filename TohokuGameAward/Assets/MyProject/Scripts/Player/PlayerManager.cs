using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerPrefab = null;

    [SerializeField]
    private PlayerParamsData m_paramsData = null;

    private List<GameObject> m_players = new List<GameObject>();

    private void Awake()
    {
        CreatePlayerBasedOnControllers();
    }

    private void CreatePlayerBasedOnControllers()
    {
        var gamepads = Gamepad.all;
        for(int i = 0; i < gamepads.Count; i++)
        {
            var playerInstance = Instantiate(m_playerPrefab, m_paramsData.StartPos[i], Quaternion.identity);
            m_players.Add(playerInstance);

            playerInstance.name = "Player" + (i + 1);

            var inputData = playerInstance.GetComponent<PlayerInputData>();
            if(inputData != null)
            {
                inputData.SetNumber(i);
            }    
        }
    }
}
