using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffectManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_unMaskRectTrans = null;

    [SerializeField]
    private float m_fadeRate = 0.0f;

    private bool m_isFadeIn = false;
    private bool m_isFadeOut = false;

    private Vector3 m_subtractValue = Vector3.zero;

    private static readonly Vector3 CircleScaleMax = new Vector3(25.0f, 25.0f, 25.0f);

    private void Start()
    {
        m_unMaskRectTrans.localScale = CircleScaleMax;
        m_subtractValue = -ConvertScaleValueToScaleRate(m_fadeRate);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_isFadeOut = true;
        }
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

    public void OnPlayFadeIn()
    {

    }

    public void OnPlayFadeOut()
    {
        if (m_unMaskRectTrans.localScale.x <= 0.0f)
        {
            m_unMaskRectTrans.localScale = Vector3.zero;
            m_isFadeOut = false;
            return;
        }

        m_unMaskRectTrans.localScale += m_subtractValue;
    }

    private Vector3 ConvertScaleValueToScaleRate(float percentage = 1.0f)
    {
        return CircleScaleMax / 100.0f * percentage;
    }
}
