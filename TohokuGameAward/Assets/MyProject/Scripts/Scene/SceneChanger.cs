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

    private SceneName m_specifiedName = 0;
    private bool m_isSpecified = false;

    private static readonly string[] ScenesName = new string[]
    {
        "TitleScene",
        "TutorialScene",
        "ConfirmScene",
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

    private void Awake()
    {
        if(!m_onPlayFadeIn)
        {
            m_fadeManager.InitializeFadeIn();
            return;
        }

        StartCoroutine(m_fadeManager.StartFadeIn());
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
#endif

    private void FixedUpdate()
    {
        if (!m_fadeManager.IsFinishFadeOut)
        {
            return;
        }
        m_onPlayFadeOut = false;

        if (m_isSpecified)
        {
            TransitionSpecifiedScene(m_specifiedName);
            return;
        }

        TransitionNextScene();
    }

    /// <summary>
    /// 引数で指定したシーンに遷移する処理を行います
    /// </summary>
    /// <param name="sceneNum"> シーンを設定 </param>
    public void TransitionSpecifiedScene(SceneName sceneNum)
    {
        if(m_onPlayFadeOut)
        {
            m_isSpecified = true;
            m_specifiedName = sceneNum;

            StartCoroutine(m_fadeManager.StartFadeOut());
            return;
        }

        SceneManager.LoadScene(ScenesName[(int)sceneNum]);
    }

    /// <summary>
    /// 現在シーンから次のシーンへ遷移する処理を行います
    /// </summary>
    public void TransitionNextScene()
    {
        if(m_onPlayFadeOut)
        {
            StartCoroutine(m_fadeManager.StartFadeOut());
            return;
        }

        var currentIndex = SceneManager.GetActiveScene().buildIndex;
        var isIndexMax = currentIndex >= SceneManager.sceneCountInBuildSettings - 1;
        if(isIndexMax)
        {
            SceneManager.LoadScene(0);
            return;
        }

        SceneManager.LoadScene(currentIndex + 1);
    }
}
