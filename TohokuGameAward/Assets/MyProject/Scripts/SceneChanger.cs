using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
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
        CharacterSelect,
        Main,
        Result
    }

    public void TransitionScene(SceneName sceneNum)
    {
        SceneManager.LoadScene(ScenesName[(int)sceneNum]);
    }
}
