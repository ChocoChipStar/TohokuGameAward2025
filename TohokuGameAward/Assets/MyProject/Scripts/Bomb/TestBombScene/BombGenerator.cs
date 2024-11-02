using UnityEngine;
using UnityEngine.SceneManagement;

public class BombGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_bomb;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Instantiate(m_bomb, transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
