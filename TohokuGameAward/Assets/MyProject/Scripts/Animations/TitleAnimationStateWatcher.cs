using UnityEngine;

public class TitleAnimationStateWatcher : StateMachineBehaviour
{
    TitleManager m_titleManager = null;
    // アニメーションが終了したときに呼ばれる
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_titleManager = animator.gameObject.GetComponent<TitleManager>();
        m_titleManager.SetIsFinished();
    }
}

