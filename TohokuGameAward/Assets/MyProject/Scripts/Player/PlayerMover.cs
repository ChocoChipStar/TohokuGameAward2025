using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_rigidbody = null;

    [SerializeField]
    private PlayerInputData m_inputData = null;

    [SerializeField]
    private PlayerParamsData m_paramsData = null;

    private bool m_isGrounded = false;

    private void Update()
    {
        if(CanMove())
        {
            MoveOperation();
        }

        if(CanJump())
        {
            JumpOperation();
        }
    }

    private void MoveOperation()
    {
        var stickValue = m_inputData.GetLeftStickValue(PlayerManager.SelfNumber);
        this.transform.position += new Vector3(m_paramsData.MoveSpeed * stickValue.x * Time.deltaTime, 0.0f, 0.0f);
    }

    private bool CanMove()
    {
        if(m_inputData.GetLeftStickValue(PlayerManager.SelfNumber).x > PlayerInputData.MovementDeadZoneRange)
        {
            return true;
        }

        if(m_inputData.GetLeftStickValue(PlayerManager.SelfNumber).x < -PlayerInputData.MovementDeadZoneRange)
        {
            return true;
        }

        return false;
    }

    private void JumpOperation()
    {
        m_rigidbody.AddForce(new Vector3(0.0f, m_paramsData.JumpPower, 0.0f), ForceMode.Impulse);
    }

    private bool CanJump()
    {
        if(m_inputData.WasPressedButton(PlayerInputData.InputButton.Jump,PlayerManager.SelfNumber) && m_isGrounded)
        {
            return true;
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagData.NameList[(int)TagData.Number.Stage]))
        {
            m_isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagData.NameList[(int)TagData.Number.Stage]))
        {
            m_isGrounded = false;
        }
    }

    //[SerializeField]
    //private Transform m_playerTransform;
    //[SerializeField]
    //private Rigidbody m_playerRigidbody;
    //[Header("コントローラー番号")]
    //[SerializeField] private int m_playerNumber;
    //public int PlayerNumber { get { return m_playerNumber; } }
    //[Header("プレイヤー移動速度")]
    //[SerializeField]
    //private float     m_playerMoveSpeed;
    //[Header("プレイヤージャンプ力")]
    //[SerializeField]
    //private Vector3   m_playerJumpScale;
    //[Header("ボムの回転力")]
    //[SerializeField]
    //private float m_bombRotationSpeed;
    //[Header("投げ角度")]
    //[SerializeField]
    //private float m_bombForceAngle;
    //[Header("投げる力")]
    //[SerializeField]
    //private float m_bombThrowPower = 44;
    ////private Vector3 m_bombThrow;

    //private float m_stickRange = 0.7f;

    //public bool IsPlayerGroundChecker { get; private set; }
    //public bool m_isGetBomb = false;
    ////private bool m_isThrowBomb = false;
    //private bool m_isPlayerLookLeft = true;
    //public static int m_gamepadNomber;
    //[Header("横投げ")]
    //[SerializeField]
    //private float m_bombThrowSide = 30.0f;
    //[Header("上投げ")]
    //[SerializeField]
    //private float m_bombThrowUp = 80.0f;
    //[Header("下投げ")]
    //[SerializeField]
    //private float m_bombThrowDown = -70.0f;
    //[SerializeField]
    //private GameObject m_bombObject;
    //[SerializeField]
    //private Rigidbody m_BombRigidbody;
    //[SerializeField]
    //private Bomb m_bomb;
    //[SerializeField]
    //private Collider m_bombCollider;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    m_gamepadNomber = PlayerNumber;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    var gamepad = Gamepad.current;
    //    if (gamepad == null)
    //        return;
    //    m_playerTransform.rotation = Quaternion.identity;
    //    var padCurrent = Gamepad.all.Count;
    //    //プレイヤー位置取得
    //    Vector2 playerPositionVector2 = transform.position;

    //    for (int i = 0; i < padCurrent; i++)
    //    {
    //        if(i == PlayerNumber)
    //        {
    //            //コントローラー入力
    //            var leftStick = Gamepad.all[i].leftStick.ReadValue();
    //            playerPositionVector2.x += m_playerMoveSpeed * leftStick.x * Time.deltaTime;
    //            m_playerTransform.position = playerPositionVector2;
    //            //ジャンプ判定
    //            if (Gamepad.all[i].aButton.wasPressedThisFrame && IsPlayerGroundChecker == true)
    //            {
    //                m_playerRigidbody.AddForce(m_playerJumpScale, ForceMode.Impulse);
    //            }
    //            //ボムを持っている
    //            if (m_bombObject != null)
    //            {
    //                //もしボムを持っていたらプレイヤーの上に持ってくる
    //                Vector3 bombPosition = m_playerTransform.position;

    //                if (!GetPlayerDirection(leftStick))
    //                {
    //                    bombPosition.x = m_playerTransform.position.x - m_playerTransform.localScale.x;
    //                }
    //                else
    //                {
    //                    bombPosition.x = m_playerTransform.position.x + m_playerTransform.localScale.x;
    //                }

    //                m_bombObject.transform.position = bombPosition;

    //                //投げる
    //                if ((Gamepad.all[i].rightTrigger.wasPressedThisFrame))
    //                {
    //                    m_BombRigidbody.useGravity = true;//ボム重力
    //                    m_bombCollider.enabled = true;    //ボムコライダー

    //                    ThrowBomb(leftStick);
    //                    RowlingBomb();
    //                    m_bombObject = null;
    //                }
    //            }
    //        }
    //    }
    //}

    //private void ThrowBomb(Vector2 leftStick)
    //{
    //    //プレイヤー向き
    //    m_isPlayerLookLeft = GetPlayerDirection(leftStick);

    //    //投げ角度決定
    //    Vector2 downThrowBombposition = m_playerTransform.position;
    //    if (leftStick.x >= m_stickRange)
    //    {
    //        m_bombForceAngle = m_bombThrowSide;//横投げ
    //    }
    //    else if (leftStick.x <= -m_stickRange)
    //    {
    //        m_bombForceAngle = 180.0f - m_bombThrowSide;//横投げ
    //    }
    //    else if (leftStick.y >= m_stickRange && !m_isPlayerLookLeft)
    //    {
    //        m_bombForceAngle = 180.0f - m_bombThrowUp;//右上投げ
    //    }
    //    else if (leftStick.y >= m_stickRange && m_isPlayerLookLeft)
    //    {
    //        m_bombForceAngle = m_bombThrowUp;//左上投げ
    //    }
    //    else if (leftStick.y <= -m_stickRange && !m_isPlayerLookLeft)
    //    {
    //        //右下投げ
    //        m_bombForceAngle = 180.0f - m_bombThrowDown;
    //        downThrowBombposition.x += -m_playerTransform.localScale.x;
    //        m_bombObject.transform.position = downThrowBombposition;
    //    }
    //    else if (leftStick.y <= -m_stickRange && m_isPlayerLookLeft)
    //    {
    //        //左下投げ
    //        m_bombForceAngle = m_bombThrowDown;
    //        downThrowBombposition.x += m_playerTransform.localScale.x;
    //        m_bombObject.transform.position = downThrowBombposition;
    //    }
    //    m_bombCollider.enabled = true;
    //    // m_bombObject.transform.position = downThrowBombposition;
    //    //角度受け渡し
    //    m_BombRigidbody.velocity = Vector3.zero;
    //    m_BombRigidbody.rotation = Quaternion.identity;

    //    // 斜め上に投射するための方向ベクトルを作成
    //    float radianAngle = m_bombForceAngle * Mathf.Deg2Rad; // 角度をラジアンに変換
    //    Vector3 throwDirection = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0); // 斜め方向

    //    // 爆弾を斜め上に投げる
    //    m_BombRigidbody.AddForce(throwDirection * m_bombThrowPower, ForceMode.Impulse);
    //    m_bombObject.GetComponent<Bomb>().isPlayerDirectExplode = true;
    //}

    //private bool GetPlayerDirection(Vector2 leftStickValue)
    //{
    //    if (leftStickValue.x > 0.0f)
    //    {
    //        return  true;
    //    }
    //    else if (leftStickValue.x < 0.0f)
    //    {
    //        return false;
    //    }

    //    return true;
    //}

    //private void RowlingBomb()
    //{
    //    // Rigidbodyの速度を取得
    //    Vector3 velocity = m_BombRigidbody.velocity;

    //    // 移動速度に基づいて回転を計算
    //    float speed = velocity.magnitude;

    //    // z軸の回転を速度に応じて設定
    //    float rotationAmount = speed * m_bombRotationSpeed * Time.deltaTime;

    //    // 現在の回転に加算
    //    transform.localRotation =
    //    Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y,
    //                     transform.localRotation.eulerAngles.z - rotationAmount);
    //}
    //private void OnCollisionStay(Collision collision)
    //{
    //    //ステージ上にいるとき
    //    if(collision.gameObject.tag == "Stage")
    //        IsPlayerGroundChecker = true;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag(TagData.NameList[(int)TagData.TagsNumber.Item]) && m_bombObject == null)
    //    {
    //        var m_bomb = other.gameObject.GetComponent<Bomb>();

    //        m_bombCollider = other.gameObject.GetComponent<Collider>();
    //        m_BombRigidbody = other.gameObject.GetComponent<Rigidbody>();

    //        m_isGetBomb = true;                 //ボム所持フラグ
    //        m_bombCollider.enabled = false;     //ボムコライダーオフ
    //        m_BombRigidbody.useGravity = false; //ボムの重力オフ
    //        m_BombRigidbody.velocity = Vector3.zero;
    //        m_BombRigidbody.angularVelocity = Vector3.zero;
    //        m_BombRigidbody.rotation = Quaternion.identity;

    //        if (!m_bomb.isThrown)
    //        {
    //            m_bombObject = other.gameObject;
    //        }
    //    }
    //}

    //private void OnCollisionExit()
    //{
    //    //地面を離れた
    //    IsPlayerGroundChecker = false;
    //}
}
