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
    private PlayerMover m_playerMover = null;
    private Rigidbody m_targetRigidbody = null;

    private RaycastHit hitInfo;

    /// <summary> X軸減速処理の経過時間 </summary>
    private float m_decelerationElapsedTime = 0.0f;

    /// <summary> プレイヤー操作不能時間 </summary>
    private float m_cantInputElpasedTime = 0.0f;

    private bool m_isDecrease = false;

    private const float RayDistance = 10.0f;
    private const float DecreaseTimeMax = 0.2f;

    private void Awake()
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
    }

    private void Update()
    {
        if (m_decelerationElapsedTime < DecreaseTimeMax)
        {
            m_decelerationElapsedTime += Time.deltaTime * (DecreaseTimeMax / m_explosionData.Blow.DecelerationTime);
        }

        // 吹き飛びの減速中じゃない場合
        if (!m_isDecrease)
        {
            return;
        }

        if (!InoperableChecker())
        {
            m_playerMover.GetExplosion(false);
            m_isDecrease = false;
        }

        // プレイヤー操作不能時間を測る
        if (m_cantInputElpasedTime > 0.0f)
        {
            m_cantInputElpasedTime += -Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (m_targetRigidbody == null)
        {
            return;
        }

        if (m_decelerationElapsedTime < DecreaseTimeMax && m_isDecrease)
        {
            AfterDecreasePower();
        }
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

        if (other.gameObject.transform.parent.CompareTag(TagData.GetTag(TagData.Names.Bomb)))
        {
            var bombBase = other.GetComponentInParent<BombBase>();
            bombBase.CauseAnExplosion();
            GenerateExplosion(other, rigidbody);
        }

        if (m_playerMover == null)
        {
            m_playerMover = other.GetComponentInParent<PlayerMover>();
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

        BlowOfTarget(rigidbody, other.transform.position, other);
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
        if (hitInfo.collider != null)
        {
            var hitTrans = hitInfo.collider.transform;
            var hitParentObj = hitTrans.parent.gameObject;
            if (hitParentObj.CompareTag(TagData.GetTag(TagData.Names.Ground))
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
    private Vector3 GetExplosionDirection(Vector3 targetPos)
    {
        var direction = targetPos - transform.position;
        float xDirection = direction.x;
        float yDirection = Mathf.Abs(direction.x) + Mathf.Abs(direction.y);
        return new Vector3(xDirection, yDirection, 0.0f).normalized;
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
    private void BlowOfTarget(Rigidbody rigidbody, Vector3 targetPos, Collider other)
    {
        var explosionDirectionPower = GetExplosionDirection(targetPos) * CalculateExplosionPower(targetPos);
        rigidbody.AddForce(explosionDirectionPower, ForceMode.Impulse);
        m_targetRigidbody = rigidbody;

        if (other.gameObject.CompareTag(TagData.GetTag(TagData.Names.Player)))
        {
            m_playerMover.GetExplosion(true);
        }

        m_cantInputElpasedTime = m_explosionData.Blow.CantInputTime;
        float forceTime = m_explosionData.Blow.DecelerationStartTime / CalculateExplosionPower(targetPos);

        Invoke(nameof(FirstDecreasePower), forceTime);
    }

    /// <summary>
    /// 最初の大きな減速を行います。
    /// </summary>
    private void FirstDecreasePower()
    {
        if (m_isDecrease || m_targetRigidbody == null)
        {
            return;
        }

        var playerVelocity = m_targetRigidbody.velocity;
        m_decelerationElapsedTime = 0.0f;

        var decelerationRateX = -playerVelocity.x * m_explosionData.Blow.DecelerationRate;
        var decelerationRateY = -playerVelocity.y * m_explosionData.Blow.DecelerationRate;
        m_targetRigidbody.AddForce(decelerationRateX, decelerationRateY, 0.0f, ForceMode.Impulse);

        m_isDecrease = true;
    }

    /// <summary>
    /// 徐々に大きな力で減速がかかります。
    /// </summary>
    private void AfterDecreasePower()
    {
        var targetVelocity = m_targetRigidbody.velocity;
        m_targetRigidbody.AddForce(-targetVelocity.x * m_decelerationElapsedTime, 0.0f, 0.0f, ForceMode.Impulse);
    }

    /// <summary>
    /// 現在プレイヤーが操作不能状態かを調べます
    /// </summary>
    /// <returns> true->操作不能 false->操作可能 </returns>
    private bool InoperableChecker()
    {
        if (m_targetRigidbody == null)
        {
            return true;
        }

        var playerVelocity = m_targetRigidbody.velocity;
        if (playerVelocity.y <= 0.0f && m_cantInputElpasedTime <= 0.0f)
        {
            return false;
        }
        return true;
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