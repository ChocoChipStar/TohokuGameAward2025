using UnityEngine;

public class BombManager : MonoBehaviour
{
    [Header("爆発までの時間[s]")]
    [SerializeField]
    private float m_time = 3.0f;

    [Header("爆風に当たったときに吹っ飛ぶ力の強さ")]
    [SerializeField]
    private float m_power = 1;

    [Header("爆発の当たる範囲")]
    [SerializeField]
    private float m_size = 2;

    [Header("回転の中心点")]
    [SerializeField]
    private float m_pivot = -1;

    [Header("爆風のPrefab")][SerializeField] private Explosion m_explosionPrefab;

    [Header("爆弾のコライダー")]
    [SerializeField]
    private SphereCollider m_collider;

    [Header("爆弾のモデル")]
    [SerializeField]
    private GameObject m_bombModel;

    [SerializeField]
    private BombController m_bombController;

    private PlayerMover m_playerMover;

    public bool isThrown = false;       //爆弾が投射されたかどうかを追跡
    private bool isRowling = false;     //回転中かどうか

    private int m_playerNumber = -1;    //プレイヤー番号保存用

    private void Update()
    {
        if (isRowling)
        {
            m_bombController.Rowling();
        }
    }

    public void FuseOn()
    {
        // 一定時間経過後に発火
        Invoke(nameof(Explode), m_time);
    }

    public void ThrowBomb()
    {
        if (!isThrown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            m_collider.center = new Vector3(0, m_pivot, 0);
            m_bombModel.transform.localPosition = new Vector3(0, m_pivot, 0);
            m_bombController.Throw();
            isThrown = true;
            isRowling = true;
        }
    }

    private void Explosion()
    {
        //即爆破
        Explode();
    }

    private void Explode()
    {
        // 爆発を生成
        var explosion = Instantiate(m_explosionPrefab, m_collider.transform.position, Quaternion.identity);
        //威力の設定
        explosion.Explode(m_power, m_size);

        // 自身は消える
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //なにかに触れたら
        isRowling = false;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            m_collider.center = new Vector3(0, 0, 0);
            m_bombModel.transform.localPosition = new Vector3(0, 0, 0);
            isThrown = false;
            m_playerNumber = -1;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            m_playerMover = other.gameObject.GetComponent<PlayerMover>();
            //if (m_playerNumber == -1 || m_playerNumber == m_playerMover.playerNomber)
            //{
            //    m_playerNumber = m_playerMover.playerNomber;
            //}
            //else
            //{
            //    Explosion();
            //}
        }
    }
}
