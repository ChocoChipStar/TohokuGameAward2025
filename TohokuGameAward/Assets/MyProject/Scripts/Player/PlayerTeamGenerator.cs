using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamGenerator : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private GameObject m_humanoidPrefab = null;

    [SerializeField]
    private GameObject m_cannonPrefab = null;

    [SerializeField]
    private CannonManager m_cannonManager = null;

    [SerializeField]
    private HumanoidData m_humanoidData = null;

    /// <summary>
    /// プレイヤーの所持する乱数
    /// </summary>
    private List<int> m_randomTicket = new List<int>();

    public static List<int> AlphaTeamNumber { get; private set; } = new List<int> { };
    public static List<int> BravoTeamNumber { get; private set; } = new List<int> { };

    private void Awake()
    {
        NonOverlappingRandomValue();
        CreatePlayerBasedOnControllers();
    }

    private void NonOverlappingRandomValue()
    {
        //乱数をもとにプレイヤーに0～3の番号を割り当てます
        int trialCount = 1000;
        for (int i = 0; i < trialCount; i++)
        {
            if (m_randomTicket.Count >= InputData.PlayerMax)
            {
                return;
            }

            var randomValue = UnityEngine.Random.Range(0, InputData.PlayerMax);

            //重複していなければリストに追加されます
            if (!m_randomTicket.Contains(randomValue))
            {
                m_randomTicket.Add(randomValue);
            }
        }
    }

    private void CreatePlayerBasedOnControllers()
    {
        m_playerManager.GetGamePadInstances();
        m_playerManager.SetMovement(false);

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

            var inputData = m_playerManager.Instances[i].GetComponent<InputData>();
            if (inputData != null)
            {
                inputData.SetNumber(i);
            }
        }
    }

    /// <summary>
    /// 乱数で割り当てられた番号をもとにプレイヤー1からチームを割り当て、作成する処理を行います。
    /// </summary>
    /// <param name="playerNum">プレイヤーの番号（コントローラー接続順）</param>
    private GameObject CreatingPlayer(int playerNum)
    {
        var instance = new GameObject();

        //割り当てられた番号が1以下であればBチームに2以上であればAチームに割り当てられます。
        if (m_randomTicket[playerNum] <= 1)
        {
            BravoTeamNumber.Add(playerNum);
            instance = Instantiate(m_humanoidPrefab, m_humanoidData.Positions.StartPos[playerNum], Quaternion.identity, this.transform);
        }
        else
        {
            AlphaTeamNumber.Add(playerNum);
            instance = m_cannonManager.GenerateCannon(m_cannonPrefab);
        }

        instance.name = "Player" + (playerNum + 1);
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
            instance = Instantiate(m_humanoidPrefab, m_humanoidData.Positions.StartPos[i], Quaternion.identity, this.transform);
            instance.name = "Player" + (AlphaTeamNumber[i] + 1);

            if (m_playerManager.Instances.Length <= i)
            {
                continue;
            }
            m_playerManager.Instances[i + 2] = instance;
        }
    }
}