using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Rigidbody playerRigidbody;

    [SerializeField]
    private int playerNomber;

    [SerializeField]
    private float playerMoveSpeed;

    [SerializeField]
    private Vector3 playerJumpScale;

    private bool playerGroundChecker = false;

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

        var padCurrent = Gamepad.all.Count;
        Vector2 playerPositionVector2 = transform.position;
        playerTransform.rotation = Quaternion.identity;
        for (int i = 0; i < padCurrent; i++)
        {
            if(i == playerNomber)
            {
                //地面に接触したらジャンプ
                if (Gamepad.all[i].aButton.wasPressedThisFrame && playerGroundChecker == true)
                {
                    Debug.Log(playerNomber);
                    playerRigidbody.AddForce(playerJumpScale, ForceMode.Impulse);
                }

                //左右移動
                var leftStick = Gamepad.all[i].leftStick.ReadValue();
                playerPositionVector2.x += playerMoveSpeed * leftStick.x * Time.deltaTime;
                playerTransform.position = playerPositionVector2;
            }
        }
    }
    
    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Stage")
        playerGroundChecker = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        playerGroundChecker = false;
    }
}
