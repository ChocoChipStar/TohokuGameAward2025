using UnityEngine;

public class BombEffectPlayer : MonoBehaviour
{
    [SerializeField]
    private ExplosionData m_explosionData = null;

    private float m_keepScaleTime = 0.0f;

    private static readonly float DestroyScale = 0.05f;

    private bool m_isGettingBigger = false;

    private bool m_isVanish = false;

    private void Awake()
    {
        var scale = m_explosionData.Effect.ScaleMin;
        this.transform.localScale = new Vector3(scale, scale, scale);
        m_isGettingBigger = true;
    }

    private void Update()
    {
        if (m_isVanish)
        {
            VanishBomb();
            return;
        }

        if (m_isGettingBigger)
        {
            ChangeScaleBigger();
        }
        else
        {
            KeepScale();
        }
    }

    private void ChangeScaleBigger()
    {
        var scale = transform.localScale.x;
        scale += (m_explosionData.Effect.ScaleMax / m_explosionData.Effect.ScaleChangeTime) * Time.deltaTime;
        this.transform.localScale = new Vector3(scale, scale, scale);
        
        if (scale >= m_explosionData.Effect.ScaleMax)
        {
            m_isGettingBigger = false;
        }
    }

    private void KeepScale()
    {
        m_keepScaleTime += Time.deltaTime;
        if(m_keepScaleTime > m_explosionData.Effect.ScaleMaxTime)
        {
           m_isVanish = true;
        }
    }

    private void VanishBomb()
    {
        var scale = transform.localScale.x;
        scale -= (m_explosionData.Effect.ScaleMax / m_explosionData.Effect.EffectEndTime) * Time.deltaTime;
        this.transform.localScale = new Vector3( scale, scale, scale);
        if(scale < DestroyScale)
        {
            Destroy(this.gameObject);
        }
    }
}
