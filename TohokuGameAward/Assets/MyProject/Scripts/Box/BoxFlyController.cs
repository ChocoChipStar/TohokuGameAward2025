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
    /// ������΂����������v�Z���܂��B
    /// </summary>
    public void CulculateFlyDirection(Vector3 explosionPos)//�������ŌĂяo��
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
    /// �����΂����ǂ����̃t���O���I���ɂ��܂��B
    /// </summary>
    public void setBoxFly()
    {
        m_isFlying = true;
    }

    /// <summary>
    /// �������̕����Ɍ������Ĕ�΂��܂��B
    /// </summary>
    void BoxFlyAway()
    {
        this.transform.position += m_flyDirection * m_flyVelocity;
    }
}
