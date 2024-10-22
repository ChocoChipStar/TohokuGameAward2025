using UnityEngine;
using static BombData;

public class Bomb : MonoBehaviour
{
    // ���̔��e�̃W���������C���X�y�N�^�[�őI��
    public BombGenre bombGenre;

    // ���e�̃f�[�^
    private BombData currentBombData;

    // Bomb�ւ̎Q�Ɓi�V�[������Bomb�I�u�W�F�N�g���Q�Ɓj
    private BombManager bombManager;

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

    //���e�f�[�^�ۑ��p
    private float m_time;
    private float m_power;
    private float m_size;
    private float m_pivot;

    private void Start()
    {
        // BombManager���炱�̔��e�̃W�������ɑΉ�����BombData���擾
        // �V���O���g�����g�p����BombManager�ɃA�N�Z�X
        currentBombData = BombManager.Instance.GetBombDataByGenre(bombGenre);

        ApplyBombData(currentBombData);
    }

    // �擾����BombData�̒l���g���āA���e�̐ݒ�𔽉f���郁�\�b�h
    private void ApplyBombData(BombData data)
    {
        m_time = currentBombData.time;
        m_power = currentBombData.power;
        m_size = currentBombData.size;
        m_pivot = currentBombData.pivot;
    }

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
