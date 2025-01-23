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

    private HumanoidInvincible[] m_playerInvincible = new HumanoidInvincible[4];

    private float[] m_respawnCount = null;

    private bool[] m_isDead = new bool[4];

    public bool[] IsDead { get { return m_isDead; } }

    private void Start()
    {
        InitArray();
        GetPlayerInvisible();
    }

    private void Update()
    {
        RespawnAfterDelay();
    }

    /// <summary>
    /// 配列サイズの初期化
    /// </summary>
    private void InitArray()
    {
        Array.Resize(ref m_isDead, m_playerManager.Instances.Length);
        Array.Fill(m_isDead, false);

        Array.Resize(ref m_respawnCount, m_playerManager.Instances.Length);
        Array.Fill(m_respawnCount, 0.0f);
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
    private void RespawnAfterDelay()
    {
        for (int i = 0; i < m_playerManager.Instances.Length; i++)
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

    /// <summary>
    /// プレイヤーをリスポーン
    /// </summary>
    /// <param name="playerNum"></param>
    private void RespawnPlayer(int playerNum)
    {
        m_playerInvincible[playerNum].PlayerInvincibleTime();
        m_playerManager.Instances[playerNum].gameObject.transform.position = m_humanoidData.Positions.RespawnPos[playerNum];

        foreach (Transform child in m_playerManager.Instances[playerNum].transform)
        {
            child.GetComponent<Collider>().enabled = true;
        }

        Rigidbody rb = m_playerManager.Instances[playerNum].gameObject.GetComponentInParent<Rigidbody>();
        rb.isKinematic = false;
    }

    /// <summary>
    /// プレイヤーがデスしているかどうかのフラグを切り替えます。
    /// </summary>
    public void SwitchDeadFlug(int playerNum, bool isDead)
    {
        m_isDead[playerNum] = isDead;
    }
}
