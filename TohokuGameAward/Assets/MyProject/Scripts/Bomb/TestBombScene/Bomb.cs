using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_rigidbody = null;

    [SerializeField]
    private GameObject m_explosionPrefab = null;

    [SerializeField]
    protected SphereCollider m_bombCollider = null;

    private int targetNumber = 0;

    private bool m_isDetected = false;
    private bool m_isHoldingItem = false;
    private bool m_isThrow = false;
    private bool m_isGrounded = false;

    protected virtual void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (m_isDetected)
        {
            InitializeDetectedItem();
        }

        if (m_isHoldingItem)
        {
            HoldingItem();
        }
    }

    protected virtual void InitializeDetectedItem()
    {
        m_isDetected = false;
        m_isHoldingItem = true;

        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;

        m_bombCollider.enabled = false;
    }

    /// <summary>
    /// アイテムを持たせる処理を実行します
    /// </summary>
    protected virtual void HoldingItem()
    {
        if (m_isThrow)
        {
            m_isHoldingItem = false;
            return;
        }
    }

    /// <summary>
    /// 爆弾の爆発を引き起こす処理をします
    /// </summary>
    protected virtual void CauseAnExplosion()
    {
        var instanceObj = Instantiate(m_explosionPrefab, this.transform.position, Quaternion.identity);
        
        var explosion = instanceObj.GetComponent<Explosion>();
        // explosion.SetBlastPower();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var isHitPlayer = collision.gameObject.CompareTag(TagData.NameList[(int)TagData.Number.Player]);
        if (isHitPlayer && m_isThrow && !m_isGrounded)
        {

        }

        var isHitStage = collision.gameObject.CompareTag(TagData.NameList[(int)TagData.Number.Stage]);
        if (isHitStage && m_isThrow)
        {
            m_isThrow = false;
            m_isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagData.NameList[(int)TagData.Number.Player]))
        {
            m_isDetected = true;

            var inputData = other.gameObject.GetComponent<PlayerInputData>();
            targetNumber = inputData.SelfNumber;
        }
    }
}
