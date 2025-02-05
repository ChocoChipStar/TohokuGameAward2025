using UnityEngine;

public class BombMover : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_rigidbody = null;

    [SerializeField]
    private GameObject m_explosionPrefab = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    private int m_shootPlayerNum = 0;

    private bool m_isAlreadyExistExplosion = false;

    private void Awake()
    {
        var direction = ConvertAngleToDirection(-m_cannonData.Params.ShootAngle).normalized;
        if(this.transform.position.x <= 0.0f)
        {
            direction.x = Mathf.Abs(direction.x);
        }
        m_rigidbody.AddForce(direction * m_cannonData.Params.ShootSpeed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(m_isAlreadyExistExplosion)
        {
            return;
        }

        CauseAnExplosion();
    }

    /// <summary>
    /// 引数の角度を方向に変換する処理を行います
    /// </summary>
    private Vector2 ConvertAngleToDirection(float angle)
    {
        var radian = (angle + 180.0f) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian),Mathf.Sin(radian));
    }

    /// <summary>
    /// 何かしらと衝突した際、爆発を発生させます
    /// </summary>
    public void CauseAnExplosion()
    {
        var instance = Instantiate(m_explosionPrefab, this.transform.position, Quaternion.identity);
        var explosionManager = instance.GetComponent<ExplosionManager>();
        explosionManager.SetShootPlayerNum(m_shootPlayerNum);

        m_isAlreadyExistExplosion = true;
        Destroy(this.gameObject); // 爆弾のオブジェクトを破壊する
    }

    /// <summary>
    /// 爆弾を発射したプレイヤーの番号を設定します
    /// </summary>
    public void SetShootPlayerNum(int shootPlayerNum)
    {
        m_shootPlayerNum = shootPlayerNum;
    }
}
