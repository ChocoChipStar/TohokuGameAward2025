using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private bool m_onPlayFadeIn = false;

    [SerializeField]
    private bool m_onPlayFadeOut = false;

    [SerializeField]
    private ScreenFade m_screenFade = null;

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
        Confirm,
        Main,
        Result
    }

    private void Awake()
    {
        if(!m_onPlayFadeIn)
        {
            m_screenFade.InitializeFadeIn();
            return;
        }

        StartCoroutine(m_screenFade.StartFadeIn());
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
#endif

    private void FixedUpdate()
    {
        if (!m_screenFade.IsFinishFadeOut)
        {
            return;
        }
        m_onPlayFadeOut = false;

        if (m_isSpecified)
        {
            LoadSpecifiedScene(m_specifiedName);
            return;
        }

        LoadNextScene();
    }

    /// <summary>
    /// 引数で指定したシーンに遷移する処理を行います
    /// </summary>
    /// <param name="sceneNum"> シーンを設定 </param>
    public void LoadSpecifiedScene(SceneName sceneNum)
    {
        if(m_onPlayFadeOut)
        {
            m_isSpecified = true;
            m_specifiedName = sceneNum;

            StartCoroutine(m_screenFade.StartFadeOut());
            return;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesName[(int)sceneNum]);
    }

    /// <summary>
    /// 現在シーンから次のシーンへ遷移する処理を行います
    /// </summary>
    public void LoadNextScene()
    {
        if(m_onPlayFadeOut)
        {
            StartCoroutine(m_screenFade.StartFadeOut());
            return;
        }

        var currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        var isIndexMax = currentIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1;
        if(isIndexMax)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(currentIndex + 1);
    }
}
