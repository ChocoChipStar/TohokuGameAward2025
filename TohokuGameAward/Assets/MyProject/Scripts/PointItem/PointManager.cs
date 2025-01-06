using System;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager m_playerManager = null;

    [SerializeField]
    private int[] m_score = null;

    void Start()
    {
        Array.Resize(ref m_score, m_playerManager.PlayerCount.Length); 
    }

    public int[] ScoreLength()
    {
        return m_score; 
    }

    public void AddScore(int playerIndex,int score)
    {
        m_score[playerIndex] += score;
    }
}
