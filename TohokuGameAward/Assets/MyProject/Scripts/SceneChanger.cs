using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private bool m_onPlayFadeIn = false;

    [SerializeField]
    private bool m_onPlayFadeOut = false;

    [SerializeField]
    private FadeManager m_fadeManager = null;

    private static readonly string[] ScenesName = new string[]
    {
        "TitleScene", 
        "TutorialScene", 
        "CharacterSelectScene", 
        "MainScene", 
        "ResultScene"
    };

    public enum SceneName
    {
        Title,
        Tutorial,
        Main,
        Result
    }

#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
#endif

    private void FixedUpdate()
    {
        if(m_fadeManager.IsFadeIn)
        {
            
        }

        if(m_fadeManager.IsFadeOut)
        {

        }
    }

    public void TransitionScene(SceneName sceneNum)
    {
        if(m_onPlayFadeOut)
        {
            m_fadeManager.StartFadeOut();
            return;
        }

        SceneManager.LoadScene(ScenesName[(int)sceneNum]);
    }

    public void TransitionNextScene()
    {
        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }
}
