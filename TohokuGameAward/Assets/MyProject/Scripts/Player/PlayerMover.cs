using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private Transform m_playerTransform;
    [SerializeField]
    private Rigidbody m_playerRigidbody;
    [Header("コントローラー番号")]
    [SerializeField] private int m_playerNumber;
    public int PlayerNumber { get { return m_playerNumber; } }
    [Header("プレイヤーの移動速度")]
    [SerializeField]
    private float     m_playerMoveSpeed;
    [Header("プレイヤーのジャンプ力")]
    [SerializeField]
    private Vector3   m_playerJumpScale;
    [Header("ボムの回転速度")]
    [SerializeField]
    private Vector3 m_bombRotationSpeed;
    [Header("ボムの発射角度")]
    [SerializeField]
    private float m_bombForceAngle;
    [Header("ボムの投げる力")]
    [SerializeField]
    private float m_bombThrowPower = 44;
    private Vector3 m_bombThrow;

    [SerializeField]
    private float m_bombCoolTime = 0.1f;

    private float m_stickRange = 0.7f;

    private bool m_isPlayerGroundChecker = false;
    private bool m_isGetBomb = false;
    private bool m_isThrowBomb = false;
    private bool m_isPlayerLookLeft = true;
    public static int m_gamepadNomber;
    [Header("ボムの横投げ角度")]
    [SerializeField]
    private float m_bombThrowSide = 30.0f;
    [Header("ボムの上投げ角度")]
    [SerializeField]
    private float m_bombThrowUp = 80.0f;
    [Header("ボムの下投げ角度")]
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
        //プレイヤー位置取得
        Vector2 playerPositionVector2 = transform.position;

        for (int i = 0; i < padCurrent; i++)
        {
            if(i == PlayerNumber)
            {
                //左右移動
                var leftStick = Gamepad.all[i].leftStick.ReadValue();
                playerPositionVector2.x += m_playerMoveSpeed * leftStick.x * Time.deltaTime;
                m_playerTransform.position = playerPositionVector2;
                //地面に接触したらジャンプ
                if (Gamepad.all[i].aButton.wasPressedThisFrame && m_isPlayerGroundChecker == true)
                {
                    //Debug.Log(m_playerNomber);
                    m_playerRigidbody.AddForce(m_playerJumpScale, ForceMode.Impulse);
                }
                //ボムをプレイヤーに持たせる
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
                //プレイヤー向きを判定
                if (leftStick.x >= 0.0f)
                {
                    m_isPlayerLookLeft = true;
                }
                else if (leftStick.x <= 0.0f)
                {
                    m_isPlayerLookLeft = false;
                }
                //ボムの角度決定
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

        //角度をラジアン値に変換
        
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
        //ステージ上にいるとき
        if(collision.gameObject.tag == "Stage")
            m_isPlayerGroundChecker = true;
        //ボムオブジェ取得
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
        //ステージ上にいないとき
        m_isPlayerGroundChecker = false;
    }
}
