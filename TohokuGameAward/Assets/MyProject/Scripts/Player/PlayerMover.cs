using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private Transform m_playerTransform;
    [SerializeField]
    private Rigidbody m_playerRigidbody;
    [SerializeField]
    public int m_playerNomber;

    [SerializeField]
    private float     m_playerMoveSpeed;
    [SerializeField]
    private Vector3   m_playerJumpScale;

    private bool m_playerGroundChecker = false;

    public static int m_gamepadNomber;


    // Start is called before the first frame update
    void Start()
    {
        m_gamepadNomber = m_playerNomber;
    }

    // Update is called once per frame
    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return;

        var padCurrent = Gamepad.all.Count;
        Vector2 playerPositionVector2 = transform.position;
        m_playerTransform.rotation = Quaternion.identity;

        for (int i = 0; i < padCurrent; i++)
        {
            if(i == m_playerNomber)
            {
                //地面に接触したらジャンプ
                if (Gamepad.all[i].aButton.wasPressedThisFrame && m_playerGroundChecker == true)
                {
                    //Debug.Log(m_playerNomber);
                    m_playerRigidbody.AddForce(m_playerJumpScale, ForceMode.Impulse);
                }
                //左右移動
                var leftStick = Gamepad.all[i].leftStick.ReadValue();
                playerPositionVector2.x += m_playerMoveSpeed * leftStick.x * Time.deltaTime;
                m_playerTransform.position = playerPositionVector2;
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Stage")
            m_playerGroundChecker = true;
    }
    private void OnCollisionExit()
    {
        m_playerGroundChecker = false;
    }
}
