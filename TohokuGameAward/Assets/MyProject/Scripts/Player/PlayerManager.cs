using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerPrefab = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    [SerializeField]
    private float m_respawnTime = 0.0f;

    [SerializeField]
    private int[] m_playerScore = null;

    private GameObject[] m_playerCount = null;

    //private bool m_isOnlyOnePlayer = false;

    private bool[] m_isDead = null;

    private float[] m_respawnCount = null;

    public GameObject[] PlayerCount
    {
        get { return m_playerCount; }
    }

    private void Awake()
    {
        CreatePlayerBasedOnControllers();
        InitArray();
    }

    private void Update()
    {
        //CountPlayers();
        RespawnAfterDelay();
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

    private void InitArray()
    {
        Array.Resize(ref m_isDead, PlayerCount.Length);
        Array.Fill(m_isDead, false);

        Array.Resize(ref m_respawnCount, PlayerCount.Length);
        Array.Fill(m_respawnCount, 0.0f);
    }

    private void RespawnAfterDelay()
    {
        for (int i = 0; i < m_playerCount.Length; i++)
        {
            if (m_isDead[i] == false)
            {
                continue;
            }

            m_respawnCount[i] += Time.deltaTime;

            if (m_respawnCount[i] > m_respawnTime)
            {
                SwitchDeadFlug(i,false);
                RespawnPlayer(i);
                m_respawnCount[i] = 0.0f;
            }
        }
    }

    private void RespawnPlayer(int playerNum)
    {
        int number = playerNum;

        var instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[playerNum], Quaternion.identity, this.transform);
        instance.name = "Player" + (number + 1);
        m_playerCount[number] = instance;

    }

    /// <summary>
    /// プレイヤーがデスしているかどうかのフラグを切り替えます。
    /// </summary>
    public void SwitchDeadFlug(int playerNum,bool isDead)
    {
        m_isDead[playerNum] = isDead;
    }

    public void ScoreControl(int totalScore, GameObject gameObject)
    {
        for (int i = 0; i < m_playerCount.Length + 1; i++)
        {
            Debug.Log(totalScore);

            m_playerScore[0] += totalScore;
            
           
        }
    }

    //private int CountPlayers()
    //{
    //    int count = this.transform.childCount;

    //    if (count <= 1)
    //    {
    //        m_isOnlyOnePlayer = true;
    //    }

    //    return count;
    //}

    //public bool GetOnlyOnePlayer()
    //{
    //    return m_isOnlyOnePlayer;
    //}
}
