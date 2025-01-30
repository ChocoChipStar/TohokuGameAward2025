using UnityEngine;

public class BombScaleChange : MonoBehaviour
{
    [SerializeField]
    private BombData m_bombData = null;

    [SerializeField,Header("通常サイズ")]
    private float m_scaleMin = 0.0f;

    [SerializeField,Header("最大サイズ")]
    private float m_scaleMax = 0.0f;

    [SerializeField,Header("最大サイズまで大きくなる時間")]
    private float m_scaleChangeTime = 0.0f;

    [SerializeField, Header("最大サイズを維持する時間")]
    private float m_scaleMaxTime = 0.0f;

    private const float m_scaleConstant = 0.1f;

    private bool m_isShoot = false;
    private bool m_isScaleChange = false;

    private void Start()
    {
        this.transform.localScale = new Vector3(m_scaleMin, m_scaleMin, m_scaleConstant);
    }

    private void Update()
    {
        if (m_isShoot)
        {
            ScaleChange();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        var parent = collider.gameObject.transform.parent.gameObject;

        var rigidbody = parent.GetComponentInParent<Rigidbody>();
        if (rigidbody == null)
        {
            return;
        }

        var playerMover = parent.GetComponentInParent<PlayerMover>();
        if (playerMover == null)
        {
            return;
        }

        m_isShoot = true;
        m_isScaleChange = true;

        var blowMover = parent.GetComponentInParent<BlowMover>();
        blowMover.BlowOfTarget(rigidbody, transform.position, collider, m_bombData, playerMover);
    }

    /// <summary>
    /// 何かにぶつかると徐々に大きくなり消える。
    /// </summary>
    private void ScaleChange()
    {
        if (m_isScaleChange)
        {
            var scale = transform.localScale.x;
            scale += (m_scaleMax / m_scaleChangeTime) * Time.deltaTime;
            this.transform.localScale = new Vector3(scale, scale, m_scaleConstant);

            if(scale >= m_scaleMax)
            {
                m_isScaleChange = false;
            }
        }
        else
        {
           Destroy(this.gameObject, m_scaleMaxTime);
        }
    }
}
