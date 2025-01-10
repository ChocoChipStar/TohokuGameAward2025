using UnityEngine;

public class CannonBombGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_bombPrehab = null;

    [SerializeField]
    private PlayerInputData m_inputData = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private Transform m_shootTransform = null;


    // Update is called once per frame
    void Update()
    {
        if (CanShootBomb())
        {
            ShootBomb();
        }
    }
    
    //大砲が爆弾を投げる処理
    private void ShootBomb()
    {
        var bomb = Instantiate(m_bombPrehab, m_shootTransform.position, Quaternion.identity);
        var bombRigidbody = bomb.GetComponent<Rigidbody>();
        
        if(bombRigidbody == null)
           return;

        bombRigidbody.AddForce(ShootVector() * m_cannonData.Params.ShootSpeed, ForceMode.Impulse);
    }

    //投げる角度を計算
    private Vector3 ShootVector()
    {
        var vector = m_shootTransform.position - transform.position;
        vector.Normalize();
        return vector;
    }

    //爆弾を発射できるか確認
    //※クールタイムなどの処理を追加予定
    private bool CanShootBomb()
    {
        if(m_inputData.WasPressedButton(PlayerInputData.ActionsName.Jump, m_inputData.SelfNumber))
        {
            return true;
        }
        return false;
    }
}
