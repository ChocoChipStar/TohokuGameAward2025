using UnityEngine;
using static InputData;

public class CannonBombShoot : MonoBehaviour
{
    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private SelfData m_selfData = null;

    [SerializeField]
    private Transform m_shootTransform = null;

    [SerializeField]
    private GameObject m_bombrefab = null;

    [SerializeField,Header("発射の強さ")]
    private float m_shootPower = 0.0f;

    [SerializeField]
    private float m_bombStockTime = 0.0f;
    [SerializeField]
    private float m_cannonCookingOffTime = 0.0f;

    bool m_isShoot = false;

    private Vector3 m_shootInitialPosition = Vector3.zero;
    private Vector3 m_shootVelocity = Vector3.zero;

    public Vector3 ShootVelocity { get { return m_shootVelocity; } private set { m_shootVelocity = value; } }
    public Vector3 ShootInitialPosition { get { return m_shootInitialPosition; } private set { m_shootInitialPosition = value; } }
  
    public bool IsShoot { get { return m_isShoot; } private set { m_isShoot = value; } }

    private void Start()
    {
        m_shootTransform.Rotate(0.0f,0.0f, m_cannonData.Params.ShootAngle);
        ShootInitialPosition = m_shootTransform.transform.position;
    }

    private void Update()
    {
        if (m_cannonCookingOffTime < 1.0f)
        {
            m_cannonCookingOffTime += Time.deltaTime;
            return;
        }

        if (m_inputData.WasPressedActionInput(ActionsType.Attack, m_selfData.Number)
         && CanShootBomb())
        {
            ShootPowerCharge();
        }

        if (!m_inputData.WasPressedActionInput(ActionsType.Attack, m_selfData.Number) && IsShoot)
        {
            ShootBombOperation();
        }

        if(m_bombStockTime < m_cannonData.Params.MagazineMax)
        {
            m_bombStockTime += Time.deltaTime / m_cannonData.Params.ReloadTime;
        }
    }

    /// <summary>
    /// ボタンが押されている間、飛距離を伸ばす処理を行います。
    /// </summary>
    private void ShootPowerCharge()
    {
        if (m_cannonData.Params.ShootSpeedMax > m_shootPower)
        {
            m_shootPower += m_cannonData.Params.ShootChargeSpeed * Time.deltaTime;
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
        m_shootPower = m_cannonData.Params.ShootSpeedMin;
        IsShoot = false;
        m_cannonCookingOffTime = 0.0f;
        m_bombStockTime--;
    }

    /// <summary>
    /// ボムを生成し、bombのGameObjectを返します。
    /// </summary>
    private GameObject GenerateBomb()
    {
        var bomb = Instantiate(m_bombrefab, m_shootTransform.position, Quaternion.identity);
        return bomb;
    }

    private bool CanShootBomb()
    {
        if(m_bombStockTime >= 1.0f)
        {
            return true;
        }
        return false;
    }
}
