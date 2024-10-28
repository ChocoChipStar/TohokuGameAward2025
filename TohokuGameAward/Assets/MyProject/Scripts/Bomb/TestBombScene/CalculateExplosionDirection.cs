using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("爆風の判定が実際に発生するまでのディレイ")]
    [SerializeField]
    private float m_startDelaySeconds = 0.1f;

    [Header("爆風の持続フレーム数")][SerializeField] private int m_durationFrameCount = 1;

    [Header("エフェクト含めすべての再生が終了するまでの時間")]
    [SerializeField]
    private float m_stopSeconds = 2f;

    [SerializeField] private ParticleSystem m_effect;

    [SerializeField] private AudioSource m_sfx;

    [SerializeField] private SphereCollider m_collider;

    private float m_explosionPower;

    //吹っ飛び判定用Ray
    private Vector3 m_rayOrigin     = new Vector3(0, 0, 0);
    private Vector3 m_rayDirection  = new Vector3(0, 0, 0);
    private float m_rayDistance = 10;

    private void Awake()
    {
        if(m_effect != null)
        {
            m_effect.Stop();
        }
        if(m_sfx != null)
        {
            m_sfx.Stop();
        }

        m_collider.enabled = false;
    }

    /// <summary>
    /// 爆破する
    /// </summary>
    public void Explode(float power, float size)
    {
        //爆発の威力の設定
        m_explosionPower = power;
        m_collider.radius = size;

        // 当たり判定管理のコルーチン
        StartCoroutine(ExplodeCoroutine());
        // 爆発エフェクト含めてもろもろを消すコルーチン
        StartCoroutine(StopCoroutine());
        // エフェクトと効果音再生
        if(m_effect != null)
        {
            m_effect.Play();
        }
        if(m_sfx != null)
        {
            m_sfx.Play();
        }
    }

    private IEnumerator ExplodeCoroutine()
    {
        // 指定秒数が経過するまでFixedUpdate上で待つ
        var delayCount = Mathf.Max(0, m_startDelaySeconds);
        while (delayCount > 0)
        {
            yield return new WaitForFixedUpdate();
            delayCount -= Time.fixedDeltaTime;
        }

        // 時間経過したらコライダを有効化して爆発の当たり判定が出る
        m_collider.enabled = true;

        // 一定フレーム数有効化
        for (var i = 0; i < m_durationFrameCount; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        // 当たり判定無効化
        m_collider.enabled = false;
    }

    private IEnumerator StopCoroutine()
    {
        // 時間経過後に消す
        yield return new WaitForSeconds(m_stopSeconds);
        if (m_effect != null)
        {
            m_effect.Stop();
        }
        if (m_sfx != null)
        {
            m_sfx.Stop();
        }
        m_collider.enabled = false;

        Destroy(gameObject);
    }

    /// <summary>
    /// 爆風にヒットしたときに相手をふっとばす処理
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // 衝突対象がRigidbodyの配下であるかを調べる
        var rigidBody = other.GetComponentInParent<Rigidbody>();

        // Rigidbodyがついてないなら吹っ飛ばない。終わり
        if (rigidBody == null) return;

        //layerMaskの設定
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Player");

        //rayの開始position
        m_rayOrigin = transform.position;

        //rayの目標position
        Vector3 temp = other.transform.position - transform.position;
        m_rayDirection = temp.normalized;

        // Rayを生成
        Ray _ray = new Ray(m_rayOrigin, m_rayDirection);
        RaycastHit _hit;

        // もしRayを投射してlayerMaskにあるコライダーに衝突したら
        if (Physics.Raycast(_ray.origin, _ray.direction * m_rayDistance, out _hit))
        {
            if (_hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                return;
            }
        }

        // 対象との距離を計算
        float distance = Vector3.Distance(transform.position, other.transform.position);

        // 距離に応じて威力を減衰させる (例えば距離が増すごとに威力が減る)
        float maxDistance = 4; // 爆風の有効範囲
        float distanceFactor = Mathf.Clamp01(1 - (distance / maxDistance)); // 距離による減衰率を計算

        // 爆風によって爆発中央から吹き飛ぶ方向のベクトルを作る
        var direction = (other.transform.position - transform.position).normalized;

        direction.z = 0;

        // 吹っ飛ばす力の大きさを距離に応じて減衰させる
        float adjustedPower = m_explosionPower * distanceFactor;

        // 吹っ飛ばす
        // ForceModeを変えると挙動が変わる（今回は質量無視）
        rigidBody.AddForce(direction * adjustedPower, ForceMode.VelocityChange);
    }
}
