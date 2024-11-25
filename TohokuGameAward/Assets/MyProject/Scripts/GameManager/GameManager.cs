using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_gameSetText = null;

    int m_count = 0;
    string m_tagName = "Player";

    void Update()
    {
        m_count = CountPlayers();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyPlayers();
        }

        if (m_count == 0)
        {
            GameSet();
        }
    }
    private int CountPlayers()
    {
        int count = 0;
        count = GameObject.FindGameObjectsWithTag(m_tagName).Length;
        return count;
    }

    private void DestroyPlayers()
    {
        Destroy(GameObject.FindWithTag(m_tagName));
    }

    private void GameSet()
    {
        m_gameSetText.SetActive(true);
    }
}

