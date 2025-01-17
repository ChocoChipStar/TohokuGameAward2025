using System;
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
    private GameObject m_CannonPrefab = null;

    [SerializeField]
    private CannonManager m_cannonManager = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    [SerializeField]
    private GameManager m_gameManager = null;

    private PlayerInvincible[] m_playerInvincible = new PlayerInvincible[4];

    [SerializeField]
    private float m_respawnTime = 0.0f;

    [SerializeField]
    private int m_cannonPlayerNumber = 0;

    [SerializeField]
    private GameObject[] m_playerCount = null;
    private GameObject instance = null;

    //private bool m_isOnlyOnePlayer = false;

    private bool[] m_isDead = new bool[4];
    private bool[] m_isCannon = new bool[4];
    private bool[] m_isshot = new bool[4];

    [SerializeField]
    private float[] m_shotInterval = new float[4];

    private float[] m_respawnCount = null;

    public bool[] IsDead {  get { return m_isDead; } }
    public bool[] IsCannon { get {  return m_isCannon; } }

    public bool[] IsShot { get { return m_isshot; } set { m_isshot = value; } }

    public float[] ShotInterval { get { return m_shotInterval; } set { m_shotInterval = value; } }

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
        ShotIntervalTime();
        //CountPlayers();
        RespawnAfterDelay();
    }

    private void CreatePlayerBasedOnControllers()
    {
        var gamepads = Gamepad.all;
        m_playerCount = new GameObject[gamepads.Count];//gamepads.Count
        for (int i = 0; i < gamepads.Count; i++)//gamepads.Count
        {
            //var instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[i], Quaternion.identity, this.transform);
            if (Timer.Round % 2 == 0)
            {
                PlayerCountJudg(i, out instance);
            }
            if (Timer.Round % 2 == 1)
            {
                SwapCannonAndPlayer(i, out instance);
            }
            instance.name = "Player" + (i + 1);
            m_playerCount[i] = instance;

            m_playerInvincible[i] = GetComponentInChildren<PlayerInvincible>();
            var inputData = instance.GetComponent<InputData>();

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

    private void PlayerCountJudg(int i, out GameObject player)
    {
        if(m_cannonPlayerNumber <= i + 1)
        {
            m_isCannon[i] = true;
            m_cannonManager.GenerateCannon(m_CannonPrefab, out instance);
            player = instance;
        }
        else
        {
            m_isCannon[i] = false;
            var instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[i], Quaternion.identity, this.transform);
            player = instance;
        }
    }

    private void SwapCannonAndPlayer(int i, out GameObject player)
    {
        if (m_cannonPlayerNumber <= i + 1)
        {
            m_isCannon[i] = false;
            var instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[i], Quaternion.identity, this.transform);
            player = instance;
        }
        else
        {
            m_isCannon[i] = true;
            m_cannonManager.GenerateCannon(m_CannonPrefab, out instance);
            player = instance;
        }
    }

    private void RespawnPlayer(int playerNum)
    {
        m_playerInvincible[playerNum].PlayerInvincibleTime();
        m_playerCount[playerNum].gameObject.transform.position = m_playerData.Positions.RespawnPos[playerNum];

        foreach (Transform child in m_playerCount[playerNum].transform)
        {
            child.GetComponent<Collider>().enabled = true;
        }

        Rigidbody rb = m_playerCount[playerNum].gameObject.GetComponentInParent<Rigidbody>();
        rb.isKinematic = false;
    }

    private void ShotIntervalTime()
    {
        for(int i = 0; i < m_playerCount.Length; i++)
        {
            if(m_isshot[i])
            {
                m_shotInterval[i] -= Time.deltaTime;
            }
            if(m_shotInterval[i] < 0)
            {
                m_isshot[i] = false;
            }
        }
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
        Rigidbody rb = m_playerCount[playerNum].gameObject.GetComponentInParent<Rigidbody>();
        rb.isKinematic = true;

        foreach (Transform child in m_playerCount[playerNum].transform)
        {
            child.GetComponent<Collider>().enabled = false;
        }
    }

    public int GetPlayerIndex(GameObject player)
    {

        for (int i = 0; i < m_playerCount.Length; i++)
        {
            if (m_playerCount[i] == player)
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
