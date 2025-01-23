using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamGenerator : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private GameObject m_playerPrefab = null;

    [SerializeField]
    private GameObject m_cannonPrefab = null;

    [SerializeField]
    private CannonManager m_cannonManager = null;

    [SerializeField]
    private HumanoidData m_humanoidData = null;

    private List<int> m_randomIndex = new List<int>();

    public static List<int> AlphaTeamNumber { get; private set; } = new List<int> { };
    public static List<int> BravoTeamNumber { get; private set; } = new List<int> { };

    private void Awake()
    {
        NonOverlappingRandomValue();
        CreatePlayerBasedOnControllers();
    }

    private void NonOverlappingRandomValue()
    {
        for (int i = 0; i < 1000; i++)
        {
            if (m_randomIndex.Count >= InputData.PlayerMax)
            {
                return;
            }

            var randomValue = UnityEngine.Random.Range(0, InputData.PlayerMax);
            if (!m_randomIndex.Contains(randomValue))
            {
                m_randomIndex.Add(randomValue);
            }
        }
    }

    private void CreatePlayerBasedOnControllers()
    {
        m_playerManager.GetGamePadInstances();
        
        if (RoundManager.CurrentRound == (int)RoundManager.RoundState.Two)
        {
            SwitchingTeamMember();
        }

        for (int i = 0; i < m_playerManager.Instances.Length; i++)
        {
            if (RoundManager.CurrentRound == 0)
            {
                m_playerManager.Instances[i] = CreatingPlayer(i);
            }

            m_playerManager.SetMovement(i, false);

            var inputData = m_playerManager.Instances[i].GetComponent<InputData>();
            if (inputData != null)
            {
                inputData.SetNumber(i);
            }
        }
    }

    private GameObject CreatingPlayer(int index)
    {
        var instance = new GameObject();
        if (m_randomIndex[index] <= 1)
        {
            BravoTeamNumber.Add(index);
            instance = Instantiate(m_playerPrefab, m_humanoidData.Positions.StartPos[index], Quaternion.identity, this.transform);
        }
        else
        {
            AlphaTeamNumber.Add(index);
            instance = m_cannonManager.GenerateCannon(m_cannonPrefab);
        }

        instance.name = "Player" + (index + 1);
        return instance;
    }

    /// <summary>
    /// ラウンド2の際にプレイヤーと大砲側を入れ替える処理を行います
    /// </summary>
    private void SwitchingTeamMember()
    {
        var instance = new GameObject();
        for (int i = 0; i < BravoTeamNumber.Count; i++)
        {
            instance = m_cannonManager.GenerateCannon(m_cannonPrefab);
            instance.name = "Player" + (BravoTeamNumber[i] + 1);

            if (m_playerManager.Instances.Length <= i)
            {
                continue;
            }

            m_playerManager.Instances[i] = instance;
        }

        for (int i = 0; i < AlphaTeamNumber.Count; i++)
        {
            instance = Instantiate(m_playerPrefab, m_humanoidData.Positions.StartPos[i], Quaternion.identity, this.transform);
            instance.name = "Player" + (AlphaTeamNumber[i] + 1);

            if (m_playerManager.Instances.Length <= i)
            {
                continue;
            }
            m_playerManager.Instances[i + 2] = instance;
        }
    }
}