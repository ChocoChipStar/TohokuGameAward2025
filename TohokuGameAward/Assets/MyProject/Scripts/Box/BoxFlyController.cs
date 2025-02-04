using UnityEngine;

public class BoxFlyController : MonoBehaviour
{
    [SerializeField]
    private float m_flyVelocity = 0f;

    [SerializeField]
    private bool m_isAffectedByExplosion = false;

    [SerializeField]
    private bool m_isFlying = false;

    [SerializeField]
    private Vector3 m_flyDirection = Vector3.zero;

    public bool IsFlying { get {  return m_isFlying; } }

    void Update()
    {
        if (m_isFlying)
        {
            BoxFlyAway();
        }
    }

    /// <summary>
    /// 箱が飛ばされる方向を計算します。
    /// </summary>
    public void CulculateFlyDirection(Vector3 explosionPos)//爆発側で呼び出す
    {
        if(m_isAffectedByExplosion)
        {
            return;
        }

        m_isAffectedByExplosion = true;

        Vector3 boxPos = this.transform.position;

        m_flyDirection = (boxPos - explosionPos).normalized;
    }

    /// <summary>
    /// 箱を飛ばすかどうかのフラグをオンにします。
    /// </summary>
    public void setBoxFly()
    {
        m_isFlying = true;
    }

    /// <summary>
    /// 箱を一定の方向に向かって飛ばします。
    /// </summary>
    void BoxFlyAway()
    {
        this.transform.position += m_flyDirection * m_flyVelocity;
    }
}
