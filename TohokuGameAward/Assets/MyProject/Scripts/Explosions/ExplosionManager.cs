using System.Collections;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
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
        OnPlayExplision();
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

        // ヒューマノイド以外なら返す
        if (!TagManager.Instance.SearchedTagName(parentObj, TagManager.Type.Humanoid))
        {
            return; 
        }

        // 無敵時間中なら返す
        var invincible = parentObj.GetComponent<HumanoidInvincible>();
        if (invincible.IsInvincible)
        {
            return; 
        }

        var humanoidMover = parentObj.GetComponent<HumanoidMover>();
        var humanoidBlow = parentObj.GetComponent<HumanoidBlow>();

        humanoidBlow.InitializeStartBlow(this.transform.position, humanoidMover);
    }

    /// <summary>
    /// 爆弾の爆発に巻き込まれた爆弾を爆発させる誘爆処理を行います
    /// </summary>
    private void InducedExplosion(GameObject parentObj, Vector3 otherPos)
    {
        var bombMover = parentObj.GetComponent<BombMover>();
        bombMover.CauseAnExplosion();
    }

    /// <summary>
    /// 爆弾の爆風演出の初期化を行います
    /// </summary>
    private void InitializedExplosion()
    {
        m_explosionEffect.Stop();

        m_explosionCollider.enabled = false;
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 爆弾の爆風演出の処理を行います
    /// </summary>
    public void OnPlayExplision()
    {
        StartCoroutine(ActivateExplosionColliderDuration());
        Invoke(nameof(InitializedExplosion), m_explosionData.Params.EffectEndTime);

        m_explosionEffect.Play();
        SoundEffectManager.Instance.OnPlayOneShot(SoundEffectManager.SoundEffectName.Explosion);
    }
}
