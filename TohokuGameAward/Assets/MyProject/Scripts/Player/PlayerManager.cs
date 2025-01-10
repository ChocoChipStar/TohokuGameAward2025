﻿using System;
using System.Runtime.CompilerServices;
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
    private float m_respawnTime = 0.0f;

    [SerializeField]
    private int m_cannonPlayerNumber = 0;

    private GameObject[] m_playerCount = null;
    private GameObject instance = null;

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

            //var instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[i], Quaternion.identity, this.transform);
            PlayerCountJudg(i, out instance);
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

    private void PlayerCountJudg(int i, out GameObject player)
    {
        if(m_cannonPlayerNumber <= i + 1)
        {
            m_cannonManager.GenerateCannon(m_CannonPrefab, out instance);
            player = instance;
        }
        else
        {
            var instance = Instantiate(m_playerPrefab, m_playerData.Positions.StartPos[i], Quaternion.identity, this.transform);
            player = instance;
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
