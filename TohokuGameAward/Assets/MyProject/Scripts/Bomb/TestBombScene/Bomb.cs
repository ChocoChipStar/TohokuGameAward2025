using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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

    //���̃J�E���g�_�E��
    [SerializeField]
    private Text m_text;

    //true = ���e�����˂��ꂽ
    public bool isThrown = false;       
    
    //true = player�ɓ����������A�����\
    private bool isPlayerDirectExplode = false;

    //true = ��]��
    private bool isRowling = false;


    //�f�o�b�N�p
    private bool isTimerStart = false;
    private float m_currentTime;
    private float m_timer = 0;

    //���e�f�[�^�ۑ��p
    private float m_time  = 0;
    private float m_power = 0;
    private float m_size  = 0;
    private float m_pivot = 0;
    private int   m_count = 0;
    private GameObject m_bombPrefab;

    private void Start()
    {
        // BombManager���炱�̔��e�̃W�������ɑΉ�����BombData���擾
        // �V���O���g�����g�p����BombManager�ɃA�N�Z�X
        currentBombData = BombManager.Instance.GetBombDataByGenre(bombGenre);

        ApplyBombData(currentBombData);

        m_currentTime = m_time;       // �J�E���g�_�E���J�n���Ԃ�ݒ�
        m_text.text = m_currentTime.ToString();
    }

    // �擾����BombData�̒l���g���āA���e�̐ݒ�𔽉f���郁�\�b�h
    private void ApplyBombData(BombData data)
    {
        m_time  = currentBombData.time;
        m_power = currentBombData.power;
        m_size  = currentBombData.size;
        m_pivot = currentBombData.pivot;
        if(m_count == 0)
        {
            m_count = currentBombData.count;
        }
        if(currentBombData.bomb != null)
        {
            m_bombPrefab = currentBombData.bomb;
        }
    }

    private void Update()
    {
        m_text.rectTransform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.7f, 0));

        if(isTimerStart)
        {
            if (m_currentTime > 0) // �J�E���g��0���傫���ꍇ�ɂ̂ݎ��s
            {
                m_timer += Time.deltaTime; // �o�ߎ��Ԃ����Z

                if (m_timer >= 1f) // 1�b�o�߂�����
                {
                    m_currentTime--;      // �J�E���g��1���炷
                    m_text.text = m_currentTime.ToString(); // Text�ɕ\��
                    m_timer = 0f;         // �^�C�}�[�����Z�b�g
                }
            }
            else
            {
                OnCountdownEnd(); // �J�E���g�_�E�����I�������Ƃ��̏���
            }
        }

        if (isRowling)
        {
            m_bombController.Rowling();
        }
    }

    private void OnCountdownEnd()
    {
        m_text.text = "0";
    }

    public void FuseOn()
    {
        // ��莞�Ԍo�ߌ�ɔ���
        Invoke(nameof(Explode), m_time);
        isTimerStart = true;
    }

    public void ThrowBomb()
    {
        if (!isThrown)
        {
            m_collider.center = new Vector3(0, m_pivot, 0);
            m_bombModel.transform.localPosition = new Vector3(0, m_pivot, 0);
            m_bombController.Throw();
            isPlayerDirectExplode = true;
            isThrown = true;
            isRowling = true;

            if (m_count > 1)
            {
                m_count--;
                var mini = Instantiate(m_bombPrefab, transform.position, Quaternion.identity);
                Bomb miniBomb = mini.GetComponent<Bomb>();
                miniBomb.m_count = m_count;
            }
            
            m_count = 1;
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

        m_count = 0;

        // ���g�͏�����
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�Ȃɂ��ɐG�ꂽ��
        isRowling = false;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isPlayerDirectExplode = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (isPlayerDirectExplode)
            {
                Explosion();
            }
            else if(!isThrown)
            {
                FuseOn();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isThrown)
            {
                isPlayerDirectExplode = true;
            }
        }
    }
}
