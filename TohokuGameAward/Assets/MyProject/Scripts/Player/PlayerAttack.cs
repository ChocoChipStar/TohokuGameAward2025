using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Collider m_playerArmCollider;
    [SerializeField]
    private Transform m_playerArmTransform;
    [SerializeField]
    private Transform m_playerTransform;
    [SerializeField]
    private Rigidbody m_playerRigidbody;
    [SerializeField]
    public PlayerMover m_playerMoverScript;
    [SerializeField]
    private float m_playerAttackPower = 1;
    [SerializeField]
    private float m_playerAttackRange;
    [SerializeField]
    private float m_playerAttackTime;
    [SerializeField]
    private float m_playerStunTime;

    private bool m_isAttack = false;
    private bool m_isStun = false;
    //private bool m_isOnGround = false;

    Vector2 m_leftStick;
    Vector2 m_rightStik;
    Vector2 m_attackVector;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return;

        m_playerArmTransform.position = m_playerTransform.position;
     　 var padCurrent = Gamepad.all.Count;
        Vector2 playerPosition = m_playerTransform.position;
        m_playerArmTransform.rotation = Quaternion.identity;
        for (int i = 0; i < padCurrent; i++)
        {
            if(i == m_playerMoverScript.PlayerNumber)
            {
                 //コントローラー入力取得
                    m_leftStick = Gamepad.all[i].leftStick.ReadValue();
                    m_rightStik = Gamepad.all[i].rightStick.ReadValue();

                if (Gamepad.all[i].xButton.wasPressedThisFrame && !m_isAttack && !m_isStun)
                {
                    m_isAttack = true;
                    Invoke(nameof(StopPlayerAttack), m_playerAttackTime);
                }
                if (m_isAttack)
                {
                    //パンチの仮オブジェクト判定用
                    var normalizedLeftStick = m_leftStick.normalized;
                    m_playerArmTransform.position = playerPosition + normalizedLeftStick * m_playerAttackRange;
                }
            }   
        }
    }

    private void OnTriggerEnter(Collider hitPlayerCollider)
    {
        if(hitPlayerCollider.gameObject.tag == "Arm")
        {
            //攻撃した腕と攻撃されたキャラの位置からベクトルを正規化
            Vector3 normalizedPlayerArm = (m_playerArmTransform.position - hitPlayerCollider.transform.position).normalized;
            m_playerRigidbody.AddForce(normalizedPlayerArm.x * m_playerAttackPower, normalizedPlayerArm.y * m_playerAttackPower, 0, ForceMode.Impulse);
            m_isStun = true;
            Invoke(nameof(StopPlayerAttack), m_playerStunTime);
        }
    }
    private void StopPlayerAttack()
    {
        m_isAttack = false;
        m_isStun = false;
    }
}
