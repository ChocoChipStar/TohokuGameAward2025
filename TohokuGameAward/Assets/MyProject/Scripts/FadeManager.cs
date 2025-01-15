using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_unMaskRectTrans = null;

    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private float m_interval = 0.0f;

    [SerializeField]
    private float m_fadeSpeed = 0.0f;

    private bool m_isFadeIn = false;
    private bool m_isFadeOut = false;

    private Vector3 m_varianceValue = Vector3.zero;

    private const float CircleMax = 25.0f;
    private static readonly Vector3 CircleScaleMax = new Vector3(25.0f, 25.0f, 25.0f);

    public bool IsFinishFadeIn { get; private set; } = false;
    public bool IsFinishFadeOut { get; private set; } = false;

    private void Start()
    {
        m_varianceValue = ConvertScaleValueToScaleRate(m_fadeSpeed);
    }

    private void FixedUpdate()
    {
        if(m_isFadeIn)
        {
            FadingIn();
        }

        if(m_isFadeOut)
        {
            FadingOut();
        }
    }

    private void FadingIn()
    {
        if (GreaterThanVector((Vector2)m_unMaskRectTrans.localScale,CircleMax))
        {
            InitializeFadeIn();
            return;
        }

        m_unMaskRectTrans.localScale += m_varianceValue;
    }

    /// <summary>
    /// フェードアウトアニメーションの処理を行います
    /// </summary>
    private void FadingOut()
    {
        if (LessThanVector((Vector2)m_unMaskRectTrans.localScale,0.0f))
        {
            InitializeFadeOut();
            return;
        }

        m_unMaskRectTrans.localScale += -m_varianceValue;
    }

    private void InitializeFadeIn()
    {
        m_unMaskRectTrans.localScale = CircleScaleMax;
        m_isFadeIn = false;
        IsFinishFadeIn = true;
    }

    private void InitializeFadeOut()
    {
        m_unMaskRectTrans.localScale = Vector3.zero;
        m_isFadeOut = false;
        IsFinishFadeOut = true;
    }

    /// <summary>
    /// フェードアニメーション速度を百分率に変換します
    /// </summary>
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

    public IEnumerator StartFadeIn()
    {
        yield return new WaitForSeconds(m_interval);

        m_unMaskRectTrans.localScale = Vector3.zero;
        m_isFadeIn = true;
    }

    public IEnumerator StartFadeOut()
    {
        yield return new WaitForSeconds(m_interval);

        m_unMaskRectTrans.localScale = CircleScaleMax;
        m_isFadeOut = true;
    }
}
