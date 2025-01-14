using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    ResultText m_resText = null;

    [SerializeField]
    float m_sceneChangeDelay = 0;

    // Update is called once per frame
    void Update()
    {
        
        if (m_resText.IsResultEnded)
        {
            m_sceneChangeDelay -= Time.deltaTime;
        }

        if(m_sceneChangeDelay < 0)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
