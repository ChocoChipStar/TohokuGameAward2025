using Unity.VisualScripting;
using UnityEngine;

public class BlowMover : MonoBehaviour
{
    [SerializeField]
    private ExplosionData m_explosionData = null;

    [SerializeField]
    private Collider m_playerCollider = null;

    [SerializeField]
    private Rigidbody m_playerRigidbody = null;

    private PlayerMover m_playerMover = null;

    private RaycastHit hitRaycast;

    /// <summary> X軸減速処理の経過時間 </summary>
    private float m_decelerationElapsedTime = 0.0f;

    /// <summary> プレイヤー操作不能時間 </summary>
    private float m_cantInputElpasedTime = 0.0f;

    /// <summary> 反射速度を保存 </summary>
    private Vector3 m_reflectionVelocity = Vector3.zero;

    private Vector3 m_playerPosition = Vector3.zero;

    private bool m_isDecrease = false;

    private const float DecreaseTimeMax = 0.2f;
    private const float PlayerBlowSpeed = 10.0f;
    private const float PlayerMagnitudeMax = 1.0f;  //値は現在適当な数値
    private const float RateOfForceReduction = 0.9f;
    private const float RayDistance = 1.0f;
    private const float ReflectionDistanceMin = 0.4f;


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

        //一定速度以上でRayを出し、速度を記憶。
        if(m_playerRigidbody.velocity.magnitude > PlayerMagnitudeMax)
        {
            VelocityStorage();
        }
    }

    private void FixedUpdate()
    {
        if (m_playerRigidbody == null)
        {
            return;
        }

        if (m_decelerationElapsedTime < DecreaseTimeMax && m_isDecrease)
        {
            AfterDecreasePower();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(m_isDecrease)
        {
            ReflectionOperation(collision);
        }
    }

    private void VelocityStorage()
    {
        var velocity = m_playerRigidbody.velocity.normalized;
        Physics.Raycast(transform.position, velocity * 10.0f, out hitRaycast);
        Debug.DrawRay(transform.position, velocity * 10.0f);
        if (hitRaycast.collider != null)
        {
            var hitTrans = hitRaycast.collider.transform;
            var hitParentObj = hitTrans.parent.gameObject;

            //Debug.Log(hitParentObj.tag);
            if (hitParentObj.CompareTag(TagData.GetTag(TagData.Names.Wall))
            || hitParentObj.CompareTag(TagData.GetTag(TagData.Names.Ground)))
            {
                if (ReflectionDistanceMin < Vector3.Distance(this.transform.position, hitTrans.position))
                {
                    m_reflectionVelocity = m_playerRigidbody.velocity;
                    Debug.Log(m_reflectionVelocity);
                }
            }
        }
    }

    private void ReflectionOperation(Collision collision)
    {
        var inNormal = collision.contacts[0].normal;
        var forceRisult = Vector3.Reflect(m_reflectionVelocity, inNormal) * RateOfForceReduction;
        m_playerRigidbody.velocity = forceRisult;
    }

    /// <summary>
    /// 爆風で触れた物体が吹き飛ぶ方向を計算します
    /// </summary>
    private Vector3 GetExplosionDirection(Vector3 targetPos)
    {
        var direction = transform.position - targetPos;
        float xDirection = direction.x;
        float yDirection = Mathf.Abs(direction.x) + Mathf.Abs(direction.y);
        return new Vector3(xDirection, yDirection, 0.0f).normalized;
    }

    /// <summary>
    /// 爆弾が最大距離範囲内でどれだけ離れているかを調べ、爆風の威力を決めます
    /// </summary>
    private float CalculateExplosionPower(Vector3 BombPos, BombData bombDeta)
    {
        var bombToPlayerDistance = Vector3.Distance(this.transform.position, BombPos);
        return bombDeta.Params.ExplosionPower * Mathf.Clamp01(1 - (bombToPlayerDistance / bombDeta.Params.ExplosionRange));
    }

    /// <summary>
    /// 爆発による影響を受けた物体を吹き飛ばす処理を実行します。
    /// </summary>
    public void BlowOfTarget(Rigidbody rigidbody, Vector3 BombPos, Collider other, BombData bombDeta, PlayerMover playerMover)
    {
        
        var explosionDirectionPower = GetExplosionDirection(BombPos) * CalculateExplosionPower(BombPos, bombDeta);
        rigidbody.AddForce(explosionDirectionPower, ForceMode.Impulse);
        m_playerRigidbody = rigidbody;
        m_playerMover = playerMover;
        VelocityStorage();

        if (other.gameObject.CompareTag(TagData.GetTag(TagData.Names.Player)))
        {
            m_playerMover.GetExplosion(true);
        }

        m_cantInputElpasedTime = m_explosionData.Blow.CantInputTime;
        float forceTime = m_explosionData.Blow.DecelerationStartTime / CalculateExplosionPower(BombPos, bombDeta);

        Invoke(nameof(FirstDecreasePower), forceTime);
    }

    /// <summary>
    /// 最初の大きな減速を行います。
    /// </summary>
    private void FirstDecreasePower()
    {
        if (m_isDecrease || m_playerRigidbody == null)
        {
            return;
        }
        var playerVelocity = m_playerRigidbody.velocity;
        m_decelerationElapsedTime = 0.0f;

        var decelerationRateX = -playerVelocity.x * m_explosionData.Blow.DecelerationRate;
        var decelerationRateY = -playerVelocity.y * m_explosionData.Blow.DecelerationRate;
        m_playerRigidbody.AddForce(decelerationRateX, decelerationRateY, 0.0f, ForceMode.Impulse);

        m_isDecrease = true;
    }

    /// <summary>
    /// 徐々に大きな力で減速がかかります。
    /// </summary>
    private void AfterDecreasePower()
    {
        var targetVelocity = m_playerRigidbody.velocity;
        m_playerRigidbody.AddForce(-targetVelocity.x * m_decelerationElapsedTime, 0.0f, 0.0f, ForceMode.Impulse);
    }

    /// <summary>
    /// 現在プレイヤーが操作不能状態かを調べます
    /// </summary>a
    /// <returns> true->操作不能 false->操作可能 </returns>
    private bool InoperableChecker()
    {
        if (m_playerRigidbody == null)
        {
            return true;
        }

        var playerVelocity = m_playerRigidbody.velocity;
        if (playerVelocity.y <= 0.0f && m_cantInputElpasedTime <= 0.0f)
        {
            return false;
        }
        return true;
    }
}
