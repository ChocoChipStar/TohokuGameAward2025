using UnityEngine;

public class TitleAnimationStateWatcher : MonoBehaviour
{
    [SerializeField]
    private TitleManager m_titleManager = null;

    [SerializeField]
    private Animator m_animator = null;
   

    public void SetIsFinished()
    {
        m_titleManager.SetIsFinished();
    }
}

