using System.Collections;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_explosionEffect = null;

    [SerializeField]
    private ExplosionData m_explosionData = null;

    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField]
    private SphereCollider m_explosionCollider = null;

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

        if (IsHittedStage(origin, direction))
        {
            return;
        }

        BlowOfTarget(rigidbody, other.transform.position);
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
    /// Rayがステージに衝突したかを検知します
    /// </summary>
    /// <returns> true->衝突を検知 false->検知無し </returns>
    private bool IsHittedStage(Vector3 origin, Vector3 direction)
    {
        Physics.Raycast(origin, direction * RayDistance, out hitInfo);
        if(hitInfo.collider != null)
        {
            var hitTrans = hitInfo.collider.transform;
            var hitParentObj = hitTrans.parent.gameObject;
            if(hitParentObj.CompareTag(TagData.GetTag(TagData.Names.Ground)) 
            || hitParentObj.CompareTag(TagData.GetTag(TagData.Names.Wall)))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 爆風で触れた物体が吹き飛ぶ方向を計算します
    /// </summary>
    private Vector3 GetExplosionDirection(Rigidbody rigidbody)
    {
        return new Vector3(1.0f, 0.75f, 0.0f).normalized; // 現在は適当な数値を代入中
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
    /// 爆発による影響を受けた物体を吹き飛ばす処理を実行します。
    /// </summary>
    private void BlowOfTarget(Rigidbody rigidbody, Vector3 targetPos)
    {
        // 吹き飛びの処理をここに記述↓
        // 爆発の食らう位置で方向が変わるようであれば GetExplosionDirection関数を変更
        // 爆発を食らう位置での威力減衰が異なる計算式になる場合は　CalculateExplosionPower関数を変更
        // 爆発で実際に吹き飛ばす処理の実行命令を出す場合は　BlowOfTarget関数を変更する

        var explosionDirectionPower = GetExplosionDirection(rigidbody) * CalculateExplosionPower(targetPos);
        rigidbody.AddForce(explosionDirectionPower, ForceMode.Impulse);
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

    /// <summary>
    /// 起爆させる爆弾の情報を更新します
    /// </summary>
    public void UpdateBombData(BombData bombData)
    {
        m_bombData = bombData;
    }
}
