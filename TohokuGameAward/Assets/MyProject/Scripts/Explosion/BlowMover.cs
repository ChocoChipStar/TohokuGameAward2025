using Unity.VisualScripting;
using UnityEngine;

public class BlowMover : MonoBehaviour
{
    [SerializeField]
    private ExplosionData m_explosionData = null;

    [SerializeField]
    private Rigidbody m_playerRigidbody = null;

    private BombData m_bombData = null;
    private PlayerMover m_playerMover = null;

    private RaycastHit hitRaycast;

    /// <summary> X軸減速処理の経過時間 </summary>
    private float m_decelerationElapsedTime = 0.0f;

    /// <summary> プレイヤー操作不能時間 </summary>
    private float m_cantInputElpasedTime = 0.0f;

    /// <summary> 反射速度を保存 </summary>
    private Vector3 m_reflectionVelocity = Vector3.zero;

    private Vector3 m_playerPosition = Vector3.zero;

    private bool m_isBlow = false;

    private const float DecreaseTimeMax = 0.15f;
    private const float BasisInputElpasedTime = 60.0f;  //タイマー基準値
    private const float PlayerBlowSpeed = 10.0f;        
    private const float PlayerMagnitudeLimit = 1.2f;      //値は目安
    private const float RateOfForceReduction = 0.8f;    //反射時の減少率
    private const float ReflectionDistanceMin = 0.8f;   //Rayの判定距離の最低値
    private const float BlowCheckerVelocityMin = 2.0f;  //この値よりVelocityが小さくなったら吹き飛びが終わる
    public bool IsBlow { get { return m_isBlow;} }

    private void Update()
    {
        // 吹き飛びの減速中じゃない場合
        if (!m_isBlow)
        {
            return;
        }

        if (m_decelerationElapsedTime < DecreaseTimeMax)
        {
            m_decelerationElapsedTime += Time.deltaTime * (DecreaseTimeMax / m_explosionData.Blow.DecelerationTime);
        }

        if (!PlayerBlowChecker())
        {
            m_playerMover.GetExplosion(false);
            m_isBlow = false;
        }

        // プレイヤー操作不能時間を測る
        if (m_cantInputElpasedTime > 0.0f)
        {
            m_cantInputElpasedTime += -Time.deltaTime * (BasisInputElpasedTime / m_bombData.Params.ExplosionPower);
        }

        //吹き飛び中にRayがでる。
        BlowOutProcess();
    }

    private void FixedUpdate()
    {
        if (m_playerRigidbody == null)
        {
            return;
        }

        if (m_decelerationElapsedTime < DecreaseTimeMax && m_isBlow)
        {
            AfterDecreasePower();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ステージに当たると跳ね返る
        if (m_isBlow)
        {
            ReflectionOperation(collision);
        }
    }

    private void BlowOutProcess()
    {
        //Rayを飛ばして速度を記録
        var velocity = m_playerRigidbody.velocity.normalized;
        Physics.Raycast(transform.position + velocity, velocity, out hitRaycast);

        if (hitRaycast.collider != null &&
            ReflectionDistanceMin < Vector3.Distance(this.transform.position, hitRaycast.point))
        {
            var hitTrans = hitRaycast.collider.transform;
            var hitParentObj = hitTrans.gameObject;
            if (TagManager.Instance.SearchedTagName(hitParentObj, TagManager.Type.Wall, TagManager.Type.Ground))
            {
                 m_reflectionVelocity = m_playerRigidbody.velocity;
            }
        }
    }

    private void ReflectionOperation(Collision collision)
    {
        //反射する
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
        if (rigidbody == null || m_isBlow)
        {
            return;
        }
        var explosionDirectionPower = GetExplosionDirection(BombPos) * CalculateExplosionPower(BombPos, bombDeta);
        rigidbody.AddForce(explosionDirectionPower, ForceMode.Impulse);

        m_isBlow = true;
        m_playerRigidbody = rigidbody;
        m_playerMover = playerMover;
        m_bombData = bombDeta;
        m_reflectionVelocity = explosionDirectionPower;

        var parentObj = other.transform.parent.gameObject;
        if (TagManager.Instance.SearchedTagName(parentObj,TagManager.Type.Player))
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
        if (m_playerRigidbody == null)
        {
            return;
        }

        var playerVelocity = m_playerRigidbody.velocity;
        m_decelerationElapsedTime = 0.0f;
        var decelerationRateX = -playerVelocity.x * m_explosionData.Blow.DecelerationRate;
        var decelerationRateY = -playerVelocity.y * m_explosionData.Blow.DecelerationRate;
        m_playerRigidbody.AddForce(decelerationRateX, decelerationRateY, 0.0f, ForceMode.Impulse);
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
    /// 現在プレイヤーが吹き飛び状態かを調べます
    /// </summary>a
    /// <returns> true->操作不能 false->操作可能 </returns>
    private bool PlayerBlowChecker()
    {
        if (m_playerRigidbody == null)
        {
            return true;
        }

        var playerVelocity = m_playerRigidbody.velocity;
        if (playerVelocity.x <= BlowCheckerVelocityMin && m_cantInputElpasedTime <= 0.0f)
        {
            return false;
        }
        return true;
    }
}