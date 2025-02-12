﻿using System.Collections.Generic;
using UnityEngine;

public class TeamGenerator : MonoBehaviour
{
    public static TeamGenerator Instance = null;

    private bool m_isAssigned = false;

    private List<int> m_randomTicket = new List<int>();

    /// <summary>
    /// 乱数番号札の試行回数
    /// </summary>
    private const int RandomTicketValueTrialCount = 1000;

    private static readonly int EvenNumber = 2;
    public static List<int> AlphaTeamNumber { get; private set; } = new List<int> { };
    public static List<int> BravoTeamNumber { get; private set; } = new List<int> { };

    public static readonly int MembersCount = 2;

    public static readonly string[] TeamName = new string[] { "Alpha", "Bravo" };

    public enum TeamType
    {
        Alpha,
        Bravo
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (RoundManager.CurrentRound == (int)RoundManager.RoundState.One)
        {
            TeamAssignment();
        }
    }

    /// <summary>
    /// 重複しないランダムな番号をプレイヤー分（４つ）生成します
    /// </summary>
    private List<int> CreateNonOverlappingRandomValue()
    {
        //乱数をもとにプレイヤーに0～3の番号を割り当てます
        int counter = 0;
        List<int> tickets = new List<int>();
        while (true)
        {
            if (tickets.Count >= PlayerManager.PlayerMax)
            {
                break;
            }

            if(counter >= RandomTicketValueTrialCount)
            {
                break;
            }

            var randomValue = Random.Range(0, PlayerManager.PlayerMax);
            //重複していなければリストに追加されます
            if (!tickets.Contains(randomValue))
            {
                tickets.Add(randomValue);
            }
        }

        return tickets;
    }

    /// <summary>
    /// プレイヤーのチーム割り振り処理を行います
    /// </summary>
    public void TeamAssignment()
    {
        if(m_isAssigned)
        {
            return;
        }

        // 乱数番号札の作成
        //m_randomTicket = CreateNonOverlappingRandomValue();
        for (int i = 0; i < PlayerManager.PlayerMax; i++)
        {
            //プレイヤーナンバーが偶数ならアルファ
            if(0 == i % EvenNumber)
            {
                AlphaTeamNumber.Add(i);
            }
            else
            {
                BravoTeamNumber.Add(i);
            }

            //// 割り当てられた番号が1以下であればBチームに2以上であればAチームに割り当てられます。
            //if (m_randomTicket[i] < MembersCount)
            //{
            //    AlphaTeamNumber.Add(i);
            //}
            //else
            //{
            //    BravoTeamNumber.Add(i);
            //}
        }
        m_isAssigned = true;
    }

    public string GetCurrentHumanoidTeamName()
    {
        if (RoundManager.CurrentRound == (int)RoundManager.RoundState.One)
        {
            return TeamGenerator.TeamName[(int)TeamGenerator.TeamType.Bravo];
        }
        return TeamGenerator.TeamName[(int)TeamGenerator.TeamType.Alpha];
    }

    public string GetCurrentCannonTeamName()
    {
        if (RoundManager.CurrentRound == (int)RoundManager.RoundState.One)
        {
            return TeamGenerator.TeamName[(int)TeamGenerator.TeamType.Alpha];
        }
        return TeamGenerator.TeamName[(int)TeamGenerator.TeamType.Bravo];
    }
}