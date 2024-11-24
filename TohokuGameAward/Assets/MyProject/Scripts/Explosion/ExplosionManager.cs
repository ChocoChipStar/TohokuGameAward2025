using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_explosionEffect = null;

    [SerializeField]
    private ExplosionData m_explosionData = null;

    [SerializeField]
    private PlayerMover m_playerMover = null;

    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField]
    private SphereCollider m_explosionCollider = null;

    private BombData m_bombData = null;

    private Rigidbody m_targetRigidbody = null;

    private RaycastHit hitInfo;

    private float m_fallTime = 0.0f;                    //X軸減速処理の経過時間
    private float m_fallTimeLimit = 1.5f;               //X軸減速処理にかける時間
    private float m_decreaseForceTime = 3.2f;           //爆弾の威力に応じて速度減少が始まる時間の基準
    private float m_playerCantMoveTime = 0.0f;          //プレイヤーが操作できない時間

    private const float RayDistance = 10.0f;
    private const float DecreasePower = 0.5f;
    private const float FallTimeMax = 0.2f;           //X軸減速処理の最大値
    private const float PlayerCantMoveTimeMax = 1.0f;

    private bool m_isGetExplosion = false;              //爆風を受けているか

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

    private void Update()
    {
        if (m_isGetExplosion)       //爆風を受けた
        {
            if (GetInputPlayerCheck())
            {
                m_playerMover.GetExplosion(false);
                m_isGetExplosion = false;
            }
            if (m_playerCantMoveTime > 0.0f)
            {
                m_playerCantMoveTime -= Time.deltaTime;
            }
        }

        if (m_fallTime < FallTimeMax)
        {
            m_fallTime += Time.deltaTime * (FallTimeMax / m_fallTimeLimit);
        }
    }

    private void FixedUpdate()
    {
        if(m_fallTime < FallTimeMax && m_isGetExplosion)
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
        m_playerMover = other.GetComponentInParent<PlayerMover>();
        if (m_playerMover == null)
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
    private void BlowOfTarget(Rigidbody rigidbody, Vector3 targetPos)
    {
        var explosionDirectionPower = GetExplosionDirection(targetPos) * CalculateExplosionPower(targetPos);
        rigidbody.AddForce(explosionDirectionPower, ForceMode.Impulse);
        m_targetRigidbody = rigidbody;
        m_playerMover.GetExplosion(true);
        m_playerCantMoveTime = PlayerCantMoveTimeMax;
        float forceTime = m_decreaseForceTime / CalculateExplosionPower(targetPos);
        Invoke(nameof(FirstDecreasePower), forceTime);
    }

    /// <summary>
    /// 最初の大きな減速を行います。
    /// </summary>
    private void FirstDecreasePower()
    {
        if (!m_isGetExplosion)
        {
            var playerVelocity = m_targetRigidbody.velocity;
            m_fallTime = 0.0f;
            m_targetRigidbody.AddForce(-playerVelocity.x * DecreasePower, -playerVelocity.y * DecreasePower, 0.0f, ForceMode.Impulse);
        }
        m_isGetExplosion = true;
    }

    /// <summary>
    /// 徐々に大きな力で減速がかかります。
    /// </summary>
    private void AfterDecreasePower()
    {
        var targetVelocity = m_targetRigidbody.velocity;
        m_targetRigidbody.AddForce(-targetVelocity.x * m_fallTime, 0.0f, 0.0f, ForceMode.Impulse);
    }

    /// <summary>
    /// プレイヤーが操作可能になる条件
    /// </summary>
    /// <returns> true->一定時間経過+落下した </returns>
    private bool GetInputPlayerCheck()      
    {
        var playerVelocity = m_targetRigidbody.velocity;
        if (playerVelocity.y < 0.0f && m_playerCantMoveTime <= 0.0f)
        {
            return true;
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

    /// <summary>
    /// 起爆させる爆弾の情報を更新します
    /// </summary>
    public void UpdateBombData(BombData bombData)
    {
        m_bombData = bombData;
    }
}
