using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField]
    private Animator m_textAnimator = null;

    public static readonly string[] AnimationName = new string[] { "Start", "Finish" };

    public enum AnimationType
    {
        Start,
        Finish
    }

    private bool m_isPlay = false;

    private AnimationType currentType = new AnimationType();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPlay(AnimationType.Start);
        }

        if (!m_isPlay)
        {
            return;
        }

        if (IsFinishedAnimation())
        {
            OnFinish();
        }
    }

    public void OnPlay(AnimationType type)
    {
        currentType = type;
        m_textAnimator.SetBool(AnimationName[(int)currentType], true);
        m_isPlay = true;
    }

    private void OnFinish()
    {
        foreach (var animationName in AnimationName)
        {
            m_textAnimator.SetBool(animationName, false);
        }
        m_isPlay = false;
    }

    private bool IsFinishedAnimation()
    {
        AnimatorStateInfo stateInfo = m_textAnimator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName(AnimationName[(int)currentType]))
        {
            return false;
        }

        if (stateInfo.normalizedTime >= 1.0f && !stateInfo.loop)
        {
            return true;
        }
        return false;
    }
}

