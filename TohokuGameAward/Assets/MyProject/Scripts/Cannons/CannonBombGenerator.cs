using UnityEngine;
using UnityEngine.UI;

public class CannonBombGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_bombPrehab = null;

    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private Slider m_cannonSlider = null;

    [SerializeField]
    private Transform m_shootTransform = null;

    [SerializeField]
    private CannonManager m_cannonManager = null;

    [SerializeField]
    private float m_bombStock = 0.0f;

    [SerializeField]
    private float m_bombCoolTime = 0.0f;

    private void Awake()
    {
        m_cannonSlider.maxValue = m_cannonData.Params.CannonBombStock;
        m_bombStock = m_cannonData.Params.CannonBombStock;
        m_cannonManager = GetComponentInParent<CannonManager>();
    }

    void Update()
    {
        if (CanShootBomb())
        {
            ShootBomb();
        }

        if(m_bombStock < m_cannonData.Params.CannonBombStock)
        {
            BombReroad();
        }
        else
        {
            m_bombStock = m_cannonData.Params.CannonBombStock;
        }

        if(m_bombCoolTime <= 1.0f)
        {
            m_bombCoolTime += Time.deltaTime / m_cannonData.Params.BombCoolTime;
        }

        //スライダー反映
        m_cannonSlider.value = m_bombStock;
    }

    private void ShootBomb()
    {
        var bomb = Instantiate(m_bombPrehab, m_shootTransform.position, Quaternion.identity);
        var bombRigidbody = bomb.GetComponent<Rigidbody>();
        
        if(bombRigidbody == null)
        {
            return;
        }

        m_cannonManager.PlaySoundEffect();
        bombRigidbody.AddForce(ShootVector() * m_cannonData.Params.ShootSpeed, ForceMode.Impulse);
        m_bombStock--;
        m_bombCoolTime--;
    }

    /// <summary>
    /// 投げる角度を計算
    /// </summary>
    /// <returns></returns>
    private Vector3 ShootVector()
    {
        var vector = m_shootTransform.position - transform.position;
        vector.Normalize();
        return vector;
    }

    private void BombReroad()
    {
        m_bombStock += Time.deltaTime / m_cannonData.Params.BombReroadTime;
    }

    /// <summary>
    /// 爆弾を発射できるか確認
    /// </summary>
    private bool CanShootBomb()
    {
        if(!m_inputData.WasPressedActionButton(InputData.ActionsName.Shoot, m_inputData.SelfNumber)) 
        {
            return false;
        }

        if(m_bombStock > 1.0f && m_bombCoolTime > 1.0f)
        {
            return true ;
        }
        return false;
    }
}
