using System.Collections;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_explosionEffect = null;

    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField]
    private SphereCollider m_explosionCollider = null;

    [SerializeField, Header("爆風の判定が実際に発生するまでのディレイ")]
    private float m_colliderActivateDelayTime = 0.1f;

    [SerializeField, Header("爆風の持続フレーム数")]
    private int m_durationFrameCount = 1;

    [SerializeField, Header("エフェクト含めすべての再生が終了するまでの時間")]
    private float m_stopSeconds = 2f;

    private BombData m_bombData = null;

    private RaycastHit hitInfo;

    private const float RayDistance = 10.0f;

    private void Awake()
    {
        if(m_explosionEffect != null)
        {
            m_explosionEffect.Stop();
        }
        if(m_audioSource != null)
        {
            m_audioSource.Stop();
        }

        m_explosionCollider.enabled = false;
    }

    /// <summary>
    /// 爆弾の爆風エフェクトが持続しているフレーム中だけ当たり判定を有効化させる処理を行います
    /// </summary>
    private IEnumerator ActivateExplosionColliderDuration()
    {
        var elapseTimer = m_colliderActivateDelayTime;
        while (elapseTimer > 0.0f)
        {
            yield return new WaitForFixedUpdate();
            elapseTimer += -Time.fixedDeltaTime; // 0.1秒待機（FixedDeltaTime）
        }

        m_explosionCollider.enabled = true;
        for (int i = 0; i < m_durationFrameCount; i++)
        {
            yield return new WaitForFixedUpdate(); // 1秒間待機（FixedDeltaTime)
        }

        m_explosionCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var rigidbody = other.GetComponentInParent<Rigidbody>();
        if (rigidbody == null)
        {
            return;
        }

        GenerateExplosion(other, rigidbody);
    }

    /// <summary>
    /// 爆風を発生させる処理を行います
    /// </summary>
    private void GenerateExplosion(Collider other, Rigidbody rigidbody)
    {
        var origin = Vector3.zero;
        var direction = Vector3.zero;
        GetRayOriginAndDirection(this.transform.position, other.transform.position, out origin, out direction);

        if (IsHittedWall(origin, direction))
        {
            return;
        }

        var targetPos = other.transform.position;
        BlowOffTarget(rigidbody, direction, targetPos);
    }

    /// <summary>
    /// 投射するRayのOriginとDirectionを取得する処理を行います
    /// </summary>
    private void GetRayOriginAndDirection(Vector3 targetA, Vector3 targetB, out Vector3 origin, out Vector3 direction)
    {
        origin = targetA;
        direction = (targetB - targetA).normalized;
        direction.z = 0.0f;
    }

    /// <summary>
    /// Rayが壁に衝突したかを検知します
    /// </summary>
    /// <returns> true->衝突を検知 false->検知無し </returns>
    private bool IsHittedWall(Vector3 origin, Vector3 direction)
    {
        Physics.Raycast(origin, direction * RayDistance, out hitInfo);
        if(hitInfo.collider != null)
        {
            return hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Wall");
        }

        return false;
    }

    /// <summary>
    /// 爆弾が最大距離範囲内でどれだけ離れているかを調べ、爆風の威力を決めます
    /// </summary>
    private float CalculateExplosionPower(Vector3 targetPos)
    {
        var bombToPlayerDistance = Vector3.Distance(this.transform.position, targetPos);
        return m_bombData.Params.ExplosionPower * Mathf.Clamp01(1 - (bombToPlayerDistance / m_bombData.Params.ExplosionRange));
    }

    /// <summary>
    /// 爆風範囲内に入ったプレイヤーを吹き飛ばす処理を行います
    /// </summary>
    private void BlowOffTarget(Rigidbody rigidbody, Vector3 direction, Vector3 targetPos)
    {
        var blownPower = direction * CalculateExplosionPower(targetPos);
        rigidbody.AddForce(blownPower, ForceMode.VelocityChange);
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
        Invoke(nameof(InitializedExplosionEffect), m_stopSeconds);

        if (m_explosionEffect != null)
        {
            m_explosionEffect.Play(); // エフェクト演出再生
        }

        if (m_audioSource != null)
        {
            m_audioSource.Play(); // 効果音再生
        }
    }

    /// <summary>
    /// 起爆させる爆弾の情報を更新します
    /// </summary>
    public void UpdateBombData(BombData bombData)
    {
        m_bombData = bombData;
    }
}
