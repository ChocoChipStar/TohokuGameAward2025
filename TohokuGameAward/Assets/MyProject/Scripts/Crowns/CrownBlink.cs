using System.Collections;
using UnityEngine;

public class CrownBlink : MonoBehaviour
{
    [SerializeField]
    CrownData m_crownData = null;

    [SerializeField]
    float m_blinkSpeed = 0.0f;

    [SerializeField]
    float m_blinkStartTime = 0.0f;//何秒前

    float m_waitToBlink = 0.0f;

    bool m_isblink = false;

    MeshRenderer m_meshRenderer = null;

    void Start()
    {
        m_waitToBlink = m_crownData.Params.LifeTime - m_blinkStartTime;
        m_meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(SetBlink());  
    }

    void Update()
    {
        if (m_isblink)
        {
            StartCoroutine(BlinkCrown());
            m_isblink = false;
        }
    }

    private IEnumerator SetBlink()
    {
        if(m_blinkStartTime >= m_crownData.Params.LifeTime)
        {
            m_blinkStartTime = m_crownData.Params.LifeTime;
        }

        yield return new WaitForSeconds(m_waitToBlink);

        m_isblink = true;
    }

    private IEnumerator BlinkCrown()
    {
        while (true)
        {
            if (m_meshRenderer.enabled == true)
            {
                m_meshRenderer.enabled = false;
            }
            else
            {
                m_meshRenderer.enabled = true;
            }

            yield return new WaitForSeconds(m_blinkSpeed);
        }
    }
}
