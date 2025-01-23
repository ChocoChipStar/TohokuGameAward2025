using System;
using UnityEngine;

public class HumanoidRespawn : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private HumanoidData m_humanoidData = null;

    [SerializeField]
    private float m_respawnTime = 0.0f;

    private HumanoidInvincible[] m_playerInvincible = new HumanoidInvincible[InputData.PlayerMax];

    private float[] m_respawnCount = new float[InputData.PlayerMax];

    private bool[] m_isDead = new bool[InputData.PlayerMax];

    public bool[] IsDead { get { return m_isDead; } }

    private void Start()
    {
        GetPlayerInvisible();
    }

    private void Update()
    {
        CanRespawn();
    }

    private void GetPlayerInvisible()
    {
        for(int i = 0; i < InputData.PlayerMax; i++)
        {
            m_playerInvincible[i] = m_playerManager.Instances[i].GetComponentInChildren<HumanoidInvincible>();
        }
    }

    /// <summary>
    /// プレイヤーがリスポーン可能かを調べる
    /// </summary>
    private void CanRespawn()
    {
        for (int i = 0; i < InputData.PlayerMax; i++)
        {
            if (m_isDead[i])
            {
                m_respawnCount[i] += Time.deltaTime;
            }

            if (m_respawnCount[i] > m_respawnTime)
            {
                SwitchDeadFlug(i, false);
                RespawnPlayer(i);
                m_respawnCount[i] = 0.0f;
            }
        }
    }

    private void RespawnPlayer(int playerNum)
    {
        m_playerInvincible[playerNum].StartInvincible();
        m_playerManager.Instances[playerNum].transform.position = m_humanoidData.Positions.RespawnPos[playerNum];
        RespawnParamsInitialize(playerNum);
    }

    private void RespawnParamsInitialize(int playerNum)
    {
        foreach (Transform child in m_playerManager.Instances[playerNum].transform)
        {
            child.GetComponent<Collider>().enabled = true;
        }

        Rigidbody rigidbody = m_playerManager.Instances[playerNum].GetComponentInParent<Rigidbody>();
        rigidbody.isKinematic = false;
    }

    /// <summary>
    /// プレイヤーがデスしているかどうかのフラグを切り替えます。
    /// </summary>
    public void SwitchDeadFlug(int playerNum, bool isDead)
    {
        m_isDead[playerNum] = isDead;
    }
}
