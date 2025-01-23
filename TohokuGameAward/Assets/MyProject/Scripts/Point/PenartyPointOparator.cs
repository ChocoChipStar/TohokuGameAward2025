using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenartyPointOparator : MonoBehaviour
{
    [SerializeField]
    private PointManager m_pointManager = null;

    [SerializeField]
    private PointData m_pointData = null;

    [SerializeField]
    private float[] m_deadPenartyInterVal = new float[InputData.PlayerMax];

    private bool[] m_isPenaltyPoint = new bool[InputData.PlayerMax];

    public bool[] IsPenaltyPoint { get { return m_isPenaltyPoint; } }

    public float[] DeadPenartyInterVal { get { return m_deadPenartyInterVal; } } 

    // Update is called once per frame
    void Update()
    {
        PenartyInterValTime();
        UpDatePointOnPlayerDeath();
    }

    void UpDatePointOnPlayerDeath()
    {
        for (int i = 0; i < InputData.PlayerMax; i++)
        {
            if (m_isPenaltyPoint[i] && m_deadPenartyInterVal[i] > 0)
            {
                AddCannonPoint(i);
                PenaltyScore(i);
                m_isPenaltyPoint[i] = false;
            }
        }
    }

    void PenartyInterValTime()
    {
        for (int i = 0; i < m_deadPenartyInterVal.Length; i++)
        {
            if (m_deadPenartyInterVal[i] > 0)
            {
                m_deadPenartyInterVal[i] -= Time.deltaTime;
            }
        }
    }

    public void AddCannonPoint(int playerIndex)
    {
        //PlayerIndexはペナルティを受けるプレイヤーのインデックスです。
        bool isAlphaTeam = PlayerManager.AlphaTeamNumber.Contains(playerIndex);
        bool isBravoTeam = PlayerManager.BravoTeamNumber.Contains(playerIndex);

        //ペナルティを受ける側の反対のチームにポイントを足します。
        if (isAlphaTeam)
        {
            int bravoTeamIndex = PlayerManager.BravoTeamNumber[0];
            m_pointManager.AddScore(bravoTeamIndex, m_pointData.Params.CannonPoint);
        }
        if (isBravoTeam)
        {
            int alphaTeamIndex = PlayerManager.AlphaTeamNumber[0];
            m_pointManager.AddScore(alphaTeamIndex, m_pointData.Params.CannonPoint);
        }
    }
    public void PenaltyScore(int playerIndex)
    {
     m_pointManager.DecreaseScore(playerIndex, m_pointData.Params.PenaltyPoint);
    }

    //以下の関数はGameManagerで呼び出しますが、コンフリクト防止のため次のブランチで

    public void SetPenaltyPoint(int index)
    {
        m_isPenaltyPoint[index] = true;
    }

    public void InitPenartyInterVal(int index)
    {
        m_deadPenartyInterVal[index] = m_pointData.Params.PenaltyInterval;
    }
}
