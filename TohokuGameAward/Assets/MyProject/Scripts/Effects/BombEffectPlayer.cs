﻿using UnityEngine;

public class BombEffectPlayer : MonoBehaviour
{
    [SerializeField]
    private ExplosionData m_explosionData = null;

    [SerializeField]
    private BombColliderScaleChange m_bombScaleChanger = null;

    private float m_keepScaleTime = 0.0f;

    private static readonly float DestroyScale = 0.05f;

    private bool m_isGettingBigger = false;

    private bool m_isVanish = false;
    private bool m_isDestroy = false;

    public bool IsVanish { get { return m_isVanish; } private set { m_isVanish = value; } }
    public bool IsDestroy { get { return m_isDestroy; } private set{ m_isDestroy = value; } }

    private void Awake()
    {
        m_bombScaleChanger = GetComponentInParent<BombColliderScaleChange>();
        var scale = m_explosionData.Effect.ScaleMin;
        this.transform.localScale = new Vector3(scale, scale, scale);
        m_isGettingBigger = true;
        SoundEffectManager.Instance.OnPlayOneShot(SoundEffectManager.SoundEffectName.Explosion);
    }

    private void Update()
    {
        if (IsVanish)
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

    private void OnTriggerEnter(Collider other)
    {
        if(m_bombScaleChanger == null)
        {
            return;
        }
        m_bombScaleChanger.HitPlayer(other);
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
           IsVanish = true;
        }
    }

    private void VanishBomb()
    {
        var scale = transform.localScale.x;
        scale -= (m_explosionData.Effect.ScaleMax / m_explosionData.Effect.EffectEndTime) * Time.deltaTime;
        this.transform.localScale = new Vector3( scale, scale, scale);
        if(scale < DestroyScale)
        {
            IsDestroy = true;
            Destroy(this.gameObject);
        }
    }
}
