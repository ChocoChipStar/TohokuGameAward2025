using UnityEngine;
using UnityEngine.InputSystem;

public class CannonBombShoot : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_shootInitialPosition = Vector3.zero;

    [SerializeField]
    private Vector3 m_shootVelocity = Vector3.zero;

    [SerializeField]
    private Transform m_shootTransform = null;

    [SerializeField]
    private GameObject m_bombrefab = null;

    [SerializeField,Header("爆弾発射最低距離")]
    private float m_shootPowerMin = 2.0f;

    [SerializeField, Header("爆弾発射最高距離")]
    private float m_shootPowerMax = 2.0f;

    [SerializeField, Header("爆弾の飛距離増加速度")]
    private float m_shootChargeSpeed = 0.0f;

    [SerializeField, Header("爆弾発射角度")]
    private float m_shootQuaternion = 0.0f;

    [SerializeField,Header("発射の強さ")]
    private float m_shootPower = 0.0f;

    bool m_isShoot = false;

    public Vector3 ShootVelocity { get { return m_shootVelocity; } private set { m_shootVelocity = value; } }
    public Vector3 ShootInitialPosition { get { return m_shootInitialPosition; } private set { m_shootInitialPosition = value; } }
  
    public bool IsShoot { get { return m_isShoot; } private set { m_isShoot = value; } }

    private void Start()
    {
        m_shootTransform.Rotate(0.0f,0.0f, m_shootQuaternion);
        m_shootTransform.position = ShootVelocity.normalized;
    }

    private void Update()
    {
        //今はここで入力判定を行っています。
        if (Gamepad.current.bButton.isPressed)
        {
            ShootPowerCharge();
        }

        if (Gamepad.current.bButton.wasReleasedThisFrame)
        {
            ShootBombOperation();
        }
    }

    /// <summary>
    /// ボタンが押されている間、飛距離を伸ばす処理を行います。
    /// </summary>
    private void ShootPowerCharge()
    {
        if (m_shootPowerMax > m_shootPower)
        {
            m_shootPower += m_shootChargeSpeed * Time.deltaTime;
        }
        ShootInitialPosition = m_shootTransform.transform.position;
        ShootVelocity = m_shootTransform.up * m_shootPower;
        IsShoot = true;
    }
    /// <summary>
    ///ボムを投げる処理を行います。
    /// </summary>
    private void ShootBombOperation()
    {
        var rigidbody = GenerateBomb().GetComponent<Rigidbody>();
        rigidbody.AddForce(ShootVelocity * rigidbody.mass, ForceMode.Impulse);
        m_shootPower = m_shootPowerMin;
        IsShoot = false;
    }

    /// <summary>
    /// ボムを生成し、bombのGameObjectを返します。。
    /// </summary>
    private GameObject GenerateBomb()
    {
        var bomb = Instantiate(m_bombrefab, m_shootTransform.position, Quaternion.identity);
        return bomb;
    }
}
