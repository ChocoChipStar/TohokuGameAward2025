using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private Transform m_playerTransform;
    [SerializeField]
    private Rigidbody m_playerRigidbody;
    [Header("�R���g���[���[�ԍ�")]
    [SerializeField] private int m_playerNumber;
    public int PlayerNumber { get { return m_playerNumber; } }
    [Header("�v���C���[�̈ړ����x")]
    [SerializeField]
    private float     m_playerMoveSpeed;
    [Header("�v���C���[�̃W�����v��")]
    [SerializeField]
    private Vector3   m_playerJumpScale;
    [Header("�{���̉�]���x")]
    [SerializeField]
    private Vector3 m_bombRotationSpeed;
    [Header("�{���̔��ˊp�x")]
    [SerializeField]
    private float m_bombForceAngle;
    [Header("�{���̓������")]
    [SerializeField]
    private float m_bombThrowPower = 44;
    private Vector3 m_bombThrow;

    [SerializeField]
    private float m_bombCoolTime = 0.1f;

    private float m_stickRange = 0.7f;

    public bool IsPlayerGroundChecker { get; private set; }
    private bool m_isGetBomb = false;
    private bool m_isThrowBomb = false;
    private bool m_isPlayerLookLeft = true;
    public static int m_gamepadNomber;
    [Header("�{���̉������p�x")]
    [SerializeField]
    private float m_bombThrowSide = 30.0f;
    [Header("�{���̏㓊���p�x")]
    [SerializeField]
    private float m_bombThrowUp = 80.0f;
    [Header("�{���̉������p�x")]
    [SerializeField]
    private float m_bombThrowDown = -70.0f;

    private GameObject m_bombObject;
    private Rigidbody m_bombRigidbody;
    private Collider m_bombCollider;
    // Start is called before the first frame update
    void Start()
    {
        m_gamepadNomber = PlayerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return;
        m_playerTransform.rotation = Quaternion.identity;
        var padCurrent = Gamepad.all.Count;
        //�v���C���[�ʒu�擾
        Vector2 playerPositionVector2 = transform.position;

        for (int i = 0; i < padCurrent; i++)
        {
            if(i == PlayerNumber)
            {
                //���E�ړ�
                var leftStick = Gamepad.all[i].leftStick.ReadValue();
                playerPositionVector2.x += m_playerMoveSpeed * leftStick.x * Time.deltaTime;
                m_playerTransform.position = playerPositionVector2;
                //�n�ʂɐڐG������W�����v
                if (Gamepad.all[i].aButton.wasPressedThisFrame && IsPlayerGroundChecker == true)
                {
                    //Debug.Log(m_playerNomber);
                    m_playerRigidbody.AddForce(m_playerJumpScale, ForceMode.Impulse);
                }
                //�{����v���C���[�Ɏ�������
                if (m_isThrowBomb)
                {
                    Vector3 bombPosition = m_playerTransform.position;
                    bombPosition.y = m_playerTransform.position.y + m_playerTransform.localScale.y;
                    m_bombObject.transform.position = bombPosition;
                    if ((Gamepad.all[i].rightTrigger.wasPressedThisFrame))
                    {
                        ThrowBomb(leftStick);
                    }
                }
                //�v���C���[�����𔻒�
                if (leftStick.x >= 0.0f)
                {
                    m_isPlayerLookLeft = true;
                }
                else if (leftStick.x <= 0.0f)
                {
                    m_isPlayerLookLeft = false;
                }
                //�{���̊p�x����
                if (leftStick.x >= m_stickRange)
                {
                    m_bombForceAngle = m_bombThrowSide;
                }
                else if (leftStick.x <= -m_stickRange)
                {
                    m_bombForceAngle = 180.0f - m_bombThrowSide;
                }
                else if (leftStick.y <= -m_stickRange && !m_isPlayerLookLeft)
                {
                    m_bombForceAngle = 180.0f - m_bombThrowDown;
                }
                else if (leftStick.y >= m_stickRange && !m_isPlayerLookLeft)
                {
                    m_bombForceAngle = 180.0f - m_bombThrowUp;
                }
                else if (leftStick.y <= -m_stickRange && m_isPlayerLookLeft)
                {
                    m_bombForceAngle = m_bombThrowDown;
                }
                else if (leftStick.y >= m_stickRange && m_isPlayerLookLeft)
                {
                    m_bombForceAngle = m_bombThrowUp;
                }
            }
        }
    }

    private void ThrowBomb(Vector2 stickVector)
    {
        Invoke(nameof(BombCoolTimeEnd), m_bombCoolTime);
        m_bombRigidbody.useGravity = true;
        m_isThrowBomb = false;

        //�p�x����W�A���l�ɕϊ�
        
        float bombRad = m_bombForceAngle * Mathf.PI / 180f;
        m_bombThrow.x = Mathf.Cos(bombRad);
        m_bombThrow.y = Mathf.Sin(bombRad);
        m_bombRigidbody.AddTorque(m_bombRotationSpeed, ForceMode.VelocityChange);
        m_bombRigidbody.AddForce(m_bombThrow * m_bombThrowPower, ForceMode.Impulse);
    }
    private void BombCoolTimeEnd()
    {
        m_bombCollider.enabled = true;
        m_isGetBomb = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        //�X�e�[�W��ɂ���Ƃ�
        if(collision.gameObject.tag == "Stage")
            IsPlayerGroundChecker = true;
        //�{���I�u�W�F�擾
        if(collision.gameObject.tag == "Bomb" && !m_isGetBomb)
        {
            m_isGetBomb = true;
            m_isThrowBomb = true;
            collision.collider.enabled = false;
            collision.rigidbody.useGravity = false;
            m_bombObject = collision.gameObject;
            m_bombRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            m_bombCollider = collision.gameObject.GetComponent<Collider>();
        }
    }
    private void OnCollisionExit()
    {
        //�X�e�[�W��ɂ��Ȃ��Ƃ�
        IsPlayerGroundChecker = false;
    }
}
