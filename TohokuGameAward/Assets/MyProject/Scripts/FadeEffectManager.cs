using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffectManager : MonoBehaviour
{
    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private RectTransform m_unMaskRectTrans = null;

    [SerializeField]
    private float m_fadeRate = 0.0f;

    private bool m_isFadeIn = false;
    private bool m_isFadeOut = false;

    private Vector3 m_varianceValue = Vector3.zero;

    private const float CircleMax = 25.0f;
    private static readonly Vector3 CircleScaleMax = new Vector3(25.0f, 25.0f, 25.0f);


    private void Start()
    {
        m_unMaskRectTrans.localScale = CircleScaleMax;
        m_varianceValue = -ConvertScaleValueToScaleRate(m_fadeRate);
    }

    private void FixedUpdate()
    {
        if(m_isFadeIn)
        {
            OnPlayFadeIn();
        }

        if(m_isFadeOut)
        {
            OnPlayFadeOut();
        }
    }

    private void FadingIn()
    {
        if (GreaterThanVector((Vector2)m_unMaskRectTrans.localScale,CircleMax))
        {
            InitializeFinishedFadeInMask();
            m_sceneChanger.TransitionScene(SceneChanger.SceneName.Tutorial);
            return;
        }

        m_unMaskRectTrans.localScale += m_varianceValue;
    }

    private void FadingOut()
    {
        if (LessThanVector((Vector2)m_unMaskRectTrans.localScale,0.0f))
        {
            InitializeFinishedFadeOutMask();
            m_sceneChanger.TransitionScene(SceneChanger.SceneName.Tutorial);
            return;
        }

        m_unMaskRectTrans.localScale += -m_varianceValue;
    }

    private void InitializeFinishedFadeInMask()
    {
        m_unMaskRectTrans.localScale = Vector3.zero;
        m_isFadeIn = false;
    }

    private void InitializeFinishedFadeOutMask()
    {
        m_unMaskRectTrans.localScale = Vector3.zero;
        m_isFadeOut = false;
    }

    private Vector3 ConvertScaleValueToScaleRate(float percentage = 1.0f)
    {
        return CircleScaleMax / 100.0f * percentage;
    }

    /// <summary>
    /// Vector2値のXとY両方が検証する値以下であるかを調べます
    /// </summary>
    /// <returns> true->検証値以下 false->検証値以下ではない </returns>
    private bool LessThanVector(Vector2 verifyVector, float verifyValue)
    {
        if(verifyVector.x <= verifyValue && verifyVector.y <= verifyValue)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Vector2値のXとY両方が検証する値以上であるかを調べます
    /// </summary>
    /// <returns> true->検証値以上 false->検証値以上ではない </returns>
    private bool GreaterThanVector(Vector2 verifyVector, float verifyValue)
    {
        if(verifyVector.x >= verifyValue && verifyVector.y >= verifyValue)
        {
            return true;
        }

        return false;
    }

    public void OnPlayFadeIn()
    {
        m_isFadeIn = true;
    }

    public void OnPlayFadeOut()
    {
        m_isFadeOut = true;
    }
}
