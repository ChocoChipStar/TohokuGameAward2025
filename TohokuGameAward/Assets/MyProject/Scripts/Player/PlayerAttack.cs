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
    private bool m_isOnGround = false;

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
        if(!m_isOnGround && m_playerMoverScript.IsPlayerGroundChecker)
            m_isOnGround =true;

        for (int i = 0; i < padCurrent; i++)
        {
            if(i == m_playerMoverScript.PlayerNumber)
            {
                m_leftStick = Gamepad.all[i].leftStick.ReadValue();
                m_rightStik = Gamepad.all[i].rightStick.ReadValue();
                //�R���g���[���[��L�X�e�B�b�N
                if (Gamepad.all[i].xButton.wasPressedThisFrame && !m_isStun && !m_isAttack)
                {
                    //�p���`����
                    if (Mathf.Abs(m_leftStick.x) > Mathf.Abs(m_leftStick.y))
                    {
                        m_attackVector.x = m_leftStick.normalized.x;
                    }
                    else if (Mathf.Abs(m_leftStick.x) < Mathf.Abs(m_leftStick.y))
                    {
                        m_attackVector.y = m_leftStick.normalized.y;
                    }
                    m_isAttack = true;
                    
                    //�󒆂ɂ���ꍇ�U���ɐ��������Ȃ�
                    if (m_isOnGround)
                    {
                        m_playerRigidbody.AddForce(m_attackVector * m_playerAttackPower, ForceMode.Impulse);
                        m_isOnGround = false;
                    }
                    Invoke(nameof(StopPlayerAttack), m_playerAttackTime);
                }
                if (m_isAttack)
                {
                    m_playerArmTransform.position = playerPosition + m_attackVector * m_playerAttackRange;
                }

                if(!m_isStun && !m_isAttack && m_playerMoverScript.IsPlayerGroundChecker)
                {
                    //�X�^���ł͂Ȃ��A�U�����ł͂Ȃ��A�n�ʂɂ���ꍇ
                    m_playerRigidbody.AddForce(Vector3.zero);
                }
                
            }   
        }
    }
    private void OnTriggerEnter(Collider hitPlayerCollider)
    {
        if(hitPlayerCollider.gameObject.tag == "Arm")
        {
            //�U�������r�ƍU�����ꂽ�L�����̈ʒu����x�N�g���𐳋K��
            Vector3 normalizedPlayerArm = (m_playerArmTransform.position - hitPlayerCollider.transform.position).normalized;
            m_playerRigidbody.AddForce(normalizedPlayerArm.x * m_playerAttackPower, normalizedPlayerArm.y * m_playerAttackPower, 0, ForceMode.Impulse);
            m_isStun = true;
            Invoke(nameof(StopPlayerAttack), m_playerStunTime);
        }
    }
    private void StopPlayerAttack()
    {
        m_attackVector = Vector2.zero;
        m_isAttack = false;
        m_isStun = false;
    }
}
