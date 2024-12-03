using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text m_gameSetText = null;

    [SerializeField]
    private string m_winText = null;

    [SerializeField]
    private Vector3 m_stageRange = Vector3.zero;

    [SerializeField]
    private PlayerManager m_playerManager = null;

    //ステージ範囲設定用変数
    private Bounds m_bounds = new Bounds(Vector3.zero, Vector3.zero);

    private Vector3 m_center = Vector3.zero;

    private void Start()
    {
        m_bounds = new Bounds(m_center, m_stageRange);
    }

    private void Update()
    {
        CheckPlayerOut();

        if (m_playerManager.GetOnlyOnePlayer())
        {
            GameSet();
        }
        //デバッグ用 スペースボタンでデストロイ
        DestroyPlayers();
    }

    private void CheckPlayerOut()
    {
        for (int i = 0; i < m_playerManager.PlayerCount.Length; i++)
        {
            if (m_playerManager.PlayerCount[i] != null && !m_bounds.Contains(m_playerManager.PlayerCount[i].transform.position))
            {
                Destroy(m_playerManager.PlayerCount[i]);
                m_playerManager.PlayerCount[i] = null;
            }
        }
    }

    private void GameSet()
    {
        if (m_gameSetText.gameObject.activeSelf)
        {
            return;
        }

        foreach (GameObject player in m_playerManager.PlayerCount)
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
