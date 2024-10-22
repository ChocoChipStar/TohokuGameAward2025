using UnityEngine;

public class BombManager : MonoBehaviour
{
    [Header("�����܂ł̎���[s]")]
    [SerializeField]
    private float m_time = 3.0f;

    [Header("�����ɓ��������Ƃ��ɐ�����ԗ͂̋���")]
    [SerializeField]
    private float m_power = 1;

    [Header("�����̓�����͈�")]
    [SerializeField]
    private float m_size = 2;

    [Header("��]�̒��S�_")]
    [SerializeField]
    private float m_pivot = -1;

    [Header("������Prefab")][SerializeField] private Explosion m_explosionPrefab;

    [Header("���e�̃R���C�_�[")]
    [SerializeField]
    private SphereCollider m_collider;

    [Header("���e�̃��f��")]
    [SerializeField]
    private GameObject m_bombModel;

    [SerializeField]
    private BombController m_bombController;

    private PlayerMover m_playerMover;

    public bool isThrown = false;       //���e�����˂��ꂽ���ǂ�����ǐ�
    private bool isRowling = false;     //��]�����ǂ���

    private int m_playerNumber = -1;    //�v���C���[�ԍ��ۑ��p

    private void Update()
    {
        if (isRowling)
        {
            m_bombController.Rowling();
        }
    }

    public void FuseOn()
    {
        // ��莞�Ԍo�ߌ�ɔ���
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
        //�����j
        Explode();
    }

    private void Explode()
    {
        // �����𐶐�
        var explosion = Instantiate(m_explosionPrefab, m_collider.transform.position, Quaternion.identity);
        //�З͂̐ݒ�
        explosion.Explode(m_power, m_size);

        // ���g�͏�����
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�Ȃɂ��ɐG�ꂽ��
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
