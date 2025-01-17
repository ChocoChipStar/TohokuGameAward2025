using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerPrefab = null;

    [SerializeField]
    private GameObject m_cannonPrefab = null;

    [SerializeField]
    private CannonManager m_cannonManager = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    [SerializeField]
    private RoundManager m_roundManager = null;

    private PlayerInvincible[] m_playerInvincible = new PlayerInvincible[4];

    [SerializeField]
    private float m_respawnTime = 0.0f;

    private GameObject[] m_instances = null;

    private List<int> m_randomIndex = new List<int>();

    private bool[] m_isDead = new bool[4];
    private bool[] m_isCannon = new bool[4];

    private float[] m_respawnCount = null;

    public static List<int> AlphaTeamNumber { get; private set; } = new List<int> { };
    public static List<int> BravoTeamNumber { get; private set; } = new List<int> { };


    public bool[] IsDead {  get { return m_isDead; } }
    public bool[] IsCannon { get {  return m_isCannon; } }

    public GameObject[] Instances { get { return m_instances; } }

    private void Awake()
    {
        NonOverlappingRandomValue();
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
        m_instances = new GameObject[gamepads.Count];
        if(RoundManager.CurrentRound == (int)RoundManager.RoundState.Two)
        {
            SwitchingTeamMember();
        }

        for (int i = 0; i < gamepads.Count; i++)
        {
            if (RoundManager.CurrentRound == 0)
            {
                m_instances[i] = CreatingPlayer(i);
            }

            SetMovement(i,false);

            m_playerInvincible[i] = m_instances[i].GetComponentInChildren<PlayerInvincible>();

            var inputData = m_instances[i].GetComponent<InputData>();
            if (inputData != null)
            {
                inputData.SetNumber(i);
            }
        }
    }

    /// <summary>
    /// プレイヤー動作の有無を設定します
    /// </summary>
    /// <param name="index"> プレイヤー番号 </param>
    /// <param name="isActive"> true -> 操作可能 false -> 操作不可 </param>
    public void SetMovement(int index, bool isActive)
    {
        var playerMover = Instances[index].GetComponent<PlayerMover>();
        if (playerMover != null)
        {
            playerMover.enabled = isActive;
            return;
        }

        var cannonMover = Instances[index].GetComponent<CannonMover>();
        if (cannonMover != null)
        {
            cannonMover.enabled = isActive;
            return;
        }
    }

    /// <summary>
    /// 配列サイズの初期化
    /// </summary>
    private void InitArray()
    {
        Array.Resize(ref m_isDead, Instances.Length);
        Array.Fill(m_isDead, false);

        Array.Resize(ref m_respawnCount, Instances.Length);
        Array.Fill(m_respawnCount, 0.0f);
    }

    private void RespawnAfterDelay()
    {
        for (int i = 0; i < m_instances.Length; i++)
        {
            if (m_isDead[i])
            {
               m_respawnCount[i] += Time.deltaTime; 
            }

            if (m_respawnCount[i] > m_respawnTime )
            {
               SwitchDeadFlug(i,false);
                RespawnPlayer(i);
                m_respawnCount[i] = 0.0f;
            }
        }
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

    private GameObject CreatingPlayer(int index)
    {
        var instance = new GameObject();
        if(index <= 1)
        {
            instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[index], Quaternion.identity, this.transform);
            AlphaTeamNumber.Add(m_randomIndex[index]);
        }
        else
        {
            instance = m_cannonManager.GenerateCannon(m_cannonPrefab);
            BravoTeamNumber.Add(m_randomIndex[index]);
        }

        instance.name = "Player" + (m_randomIndex[index] + 1);
        return instance;
    }

    /// <summary>
    /// ラウンド2の際にプレイヤーと大砲側を入れ替える処理を行います
    /// </summary>
    private void SwitchingTeamMember()
    {
        var instance = new GameObject();
        for (int i = 0; i < AlphaTeamNumber.Count; i++)
        {
            instance = m_cannonManager.GenerateCannon(m_cannonPrefab);
            instance.name = "Player" + (AlphaTeamNumber[i] + 1);

            if(m_instances.Length <= i)
            {
                continue;
            }

            m_instances[i] = instance;
        }

        for (int i = 0; i < BravoTeamNumber.Count; i++)
        {
            instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[i], Quaternion.identity, this.transform);
            instance.name = "Player" + (BravoTeamNumber[i] + 1);

            if (m_instances.Length <= i)
            {
                continue;
            }
            m_instances[i + 2] = instance;
        }
    }

    private void RespawnPlayer(int playerNum)
    {
        m_playerInvincible[playerNum].PlayerInvincibleTime();
        m_instances[playerNum].gameObject.transform.position = m_playerData.Positions.RespawnPos[playerNum];

        foreach (Transform child in m_instances[playerNum].transform)
        {
            child.GetComponent<Collider>().enabled = true;
        }

        Rigidbody rb = m_instances[playerNum].gameObject.GetComponentInParent<Rigidbody>();
        rb.isKinematic = false;
    }

    /// <summary>
    /// プレイヤーがデスしているかどうかのフラグを切り替えます。
    /// </summary>
    public void SwitchDeadFlug(int playerNum,bool isDead)
    {
      m_isDead[playerNum] = isDead;
    }

    public void DisablePhysics(int playerNum)
    {
        Rigidbody rb = m_instances[playerNum].gameObject.GetComponentInParent<Rigidbody>();
        rb.isKinematic = true;

        foreach (Transform child in m_instances[playerNum].transform)
        {
            child.GetComponent<Collider>().enabled = false;
        }
    }

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
