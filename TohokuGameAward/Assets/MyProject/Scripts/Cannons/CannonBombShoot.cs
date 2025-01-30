using UnityEngine;

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

    private float m_shootPower = 10.0f;

    bool m_isShoot = false;


    public Vector3 ShootVelocity { get { return m_shootVelocity; } private set { m_shootVelocity = value; } }
    public Vector3 ShootInitialPosition { get { return m_shootInitialPosition; } private set { m_shootInitialPosition = value; } }


    private void Update()
    {
        m_shootInitialPosition = m_shootTransform.transform.position;
        m_shootVelocity = m_shootTransform.transform.up * m_shootPower;

        if (Input.GetKey(KeyCode.Space) && !m_isShoot)
        {
            //m_isShoot = true;
            //var bomb = Instantiate(m_bombrefab, m_shootTransform.position, Quaternion.identity);
            //var rigidbody = bomb.GetComponent<Rigidbody>();
            //rigidbody.AddForce(ShootVelocity * rigidbody.mass, ForceMode.Impulse);
        }
    }
}
