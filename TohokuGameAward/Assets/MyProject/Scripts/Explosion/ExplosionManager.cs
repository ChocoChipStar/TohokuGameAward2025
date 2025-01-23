using System.Collections;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField]
    private ParticleSystem m_explosionEffect = null;

    [SerializeField]
    private SphereCollider m_explosionCollider = null;

    [SerializeField]
    private ExplosionData m_explosionData = null;

    [SerializeField]
    private BombData m_bombData = null;

    private RaycastHit m_hitInfo;

    private const float RayDistance = 10.0f;

    private void Awake()
    {
        OnPlayExplosionEffect();
    }

    /// <summary>
    /// 爆弾の爆風エフェクトが持続しているフレーム中だけ当たり判定を有効化させる処理を行います
    /// </summary>
    private IEnumerator ActivateExplosionColliderDuration()
    {
        var elapseTimer = m_explosionData.Params.ColliderActivateDelayTime;
        while (elapseTimer > 0.0f)
        {
            yield return new WaitForFixedUpdate();
            elapseTimer += -Time.fixedDeltaTime; // 0.1秒待機（FixedDeltaTime）
        }

        m_explosionCollider.enabled = true;
        for (int i = 0; i < m_explosionData.Params.DurationFrameCount; i++)
        {
            yield return new WaitForFixedUpdate(); // 1秒間待機（FixedDeltaTime)
        }

        m_explosionCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var parentObj = other.gameObject.transform.parent.gameObject;
        if (TagManager.Instance.SearchedTagName(parentObj, TagManager.Type.Bomb))
        {
            InducedExplosion(parentObj, other.transform.position);
        }

        if (!TagManager.Instance.SearchedTagName(parentObj, TagManager.Type.Player))
        {
            return;
        }

        var invincible = parentObj.GetComponent<PlayerInvincible>();
        if (invincible.IsInvincible)
        {
            return;
        }

        if (IsPlayerInsideExplosion(other.transform.position))
        {
            var rigidbody = parentObj.GetComponent<Rigidbody>();
            var playerMover = parentObj.GetComponent<PlayerMover>();
            var blowMover = parentObj.GetComponent<BlowMover>();

            blowMover.BlowOfTarget(rigidbody, this.transform.position, other, m_bombData, playerMover);
        }
    }

    /// <summary>
    /// 爆弾の爆発に巻き込まれた爆弾を爆発させる誘爆処理を行います
    /// </summary>
    private void InducedExplosion(GameObject parentObj, Vector3 otherPos)
    {
        var bombBase = parentObj.GetComponent<BombManager>();
        bombBase.CauseAnExplosion();
    }

    /// <summary>
    /// プレイヤーが爆風の範囲内にいるかを調べます
    /// </summary>
    private bool IsPlayerInsideExplosion(Vector3 otherPos)
    {
        if (IsHittedStage(otherPos))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Rayがステージに衝突したかを検知します
    /// </summary>
    /// <returns> true->衝突を検知 false->検知無し </returns>
    private bool IsHittedStage(Vector3 otherPos)
    {
        var origin = this.transform.position;
        var direction = (otherPos - origin).normalized;
        direction.z = 0.0f;

        Physics.Raycast(origin, direction * RayDistance, out m_hitInfo);
        if(m_hitInfo.collider != null)
        {
            var hitObj = m_hitInfo.collider.gameObject;
            if (TagManager.Instance.SearchedTagName(hitObj, TagManager.Type.Ground, TagManager.Type.Wall))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 爆風エフェクトの初期化を行います
    /// </summary>
    private void InitializedExplosionEffect()
    {
        if (m_explosionEffect != null)
        {
            m_explosionEffect.Stop();
        }

        if (m_audioSource != null)
        {
            m_audioSource.Stop();
        }

        m_explosionCollider.enabled = false;
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 爆弾の爆風演出の処理を行います
    /// </summary>
    public void OnPlayExplosionEffect()
    {
        StartCoroutine(ActivateExplosionColliderDuration());
        Invoke(nameof(InitializedExplosionEffect), m_explosionData.Params.EffectEndTime);

        if (m_explosionEffect != null)
        {
            m_explosionEffect.Play(); // エフェクト演出再生
        }

        if (m_audioSource != null)
        {
            m_audioSource.Play(); // 効果音再生
        }
    }
}
