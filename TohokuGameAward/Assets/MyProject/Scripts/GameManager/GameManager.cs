using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_Players = null;

    [SerializeField]
    Text m_gameSetText = null;

    [SerializeField]
    private string m_winText = null;

    [SerializeField]
    private Vector3 m_stageRange = Vector3.zero;

    private int m_currentPlayerCount = 0;

    

    //ステージ範囲設定用
    private Bounds m_bounds = new Bounds(Vector3.zero,Vector3.zero);
    private Vector3 m_center = Vector3.zero;

    private void Start()
    {
       m_bounds = new Bounds(m_center, m_stageRange);
    }
    private void Update()
    {
        m_currentPlayerCount = CountPlayers();
        CheckPlayerOut();

        if (m_currentPlayerCount <= 1)
        {
            GameSet();
        }

        //デバッグ用 スペースボタンでデストロイ
        DestroyPlayers();
    }
    private void CheckPlayerOut()
        {
            for (int i = 0; i < m_Players.Length; i++)
            {
                if(m_Players[i] != null && !m_bounds.Contains(m_Players[i].transform.position))
                {
                    Destroy(m_Players[i]);
                    m_Players[i] = null;

                }
            }
        }
    private int CountPlayers()
    {
        int count = 0;
        for(int i = 0;i < m_Players.Length; i++)
        {
            if (m_Players[i] != null)
            {
                count++;
            }
        }
        return count;
    }
    private void GameSet()
    {
        if (m_gameSetText.gameObject.activeSelf)
        { return; }

        foreach (GameObject player in m_Players)
        {
            if (player != null)
            {
                string winnerName = player.name;
                m_gameSetText.text = winnerName + m_winText;
                m_gameSetText.gameObject.SetActive(true);
                return;
            }
        }
    }
  private void DestroyPlayers()
    {
        string tagName = "Player";
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(GameObject.FindWithTag(tagName));
        }
    }
}

/*
③完了したGameManagerをPrefab化し、PrefabsフォルダにGameCircleフォルダを作成しそこ*/