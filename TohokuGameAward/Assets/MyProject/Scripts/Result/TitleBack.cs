using UnityEngine;
using System.Collections;

public class TitleBack : MonoBehaviour
{
    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private float m_changeDelay = 0f;

    private void ToNextScene()
    {
        StartCoroutine(SetNextScene());
    }
    private IEnumerator SetNextScene()
    {
        yield return new WaitForSeconds(m_changeDelay);
        m_sceneChanger.LoadSpecifiedScene(SceneChanger.SceneName.Title);
    }
}
