using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("�����̔��肪���ۂɔ�������܂ł̃f�B���C")]
    [SerializeField]
    private float m_startDelaySeconds = 0.1f;

    [Header("�����̎����t���[����")][SerializeField] private int m_durationFrameCount = 1;

    [Header("�G�t�F�N�g�܂߂��ׂĂ̍Đ����I������܂ł̎���")]
    [SerializeField]
    private float m_stopSeconds = 2f;

    [SerializeField] private ParticleSystem m_effect;

    [SerializeField] private AudioSource m_sfx;

    [SerializeField] private SphereCollider m_collider;

    private float m_explosionPower;

    //������є���pRay
    private Vector3 m_rayOrigin     = new Vector3(0, 0, 0);
    private Vector3 m_rayDirection  = new Vector3(0, 0, 0);
    private float m_rayDistance = 10;

    private void Awake()
    {
        if(m_effect != null)
        {
            m_effect.Stop();
        }
        if(m_sfx != null)
        {
            m_sfx.Stop();
        }

        m_collider.enabled = false;
    }

    /// <summary>
    /// ���j����
    /// </summary>
    public void Explode(float power, float size)
    {
        //�����̈З͂̐ݒ�
        m_explosionPower = power;
        m_collider.radius = size;

        // �����蔻��Ǘ��̃R���[�`��
        StartCoroutine(ExplodeCoroutine());
        // �����G�t�F�N�g�܂߂Ă������������R���[�`��
        StartCoroutine(StopCoroutine());
        // �G�t�F�N�g�ƌ��ʉ��Đ�
        if(m_effect != null)
        {
            m_effect.Play();
        }
        if(m_sfx != null)
        {
            m_sfx.Play();
        }
    }

    private IEnumerator ExplodeCoroutine()
    {
        // �w��b�����o�߂���܂�FixedUpdate��ő҂�
        var delayCount = Mathf.Max(0, m_startDelaySeconds);
        while (delayCount > 0)
        {
            yield return new WaitForFixedUpdate();
            delayCount -= Time.fixedDeltaTime;
        }

        // ���Ԍo�߂�����R���C�_��L�������Ĕ����̓����蔻�肪�o��
        m_collider.enabled = true;

        // ���t���[�����L����
        for (var i = 0; i < m_durationFrameCount; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        // �����蔻�薳����
        m_collider.enabled = false;
    }

    private IEnumerator StopCoroutine()
    {
        // ���Ԍo�ߌ�ɏ���
        yield return new WaitForSeconds(m_stopSeconds);
        if (m_effect != null)
        {
            m_effect.Stop();
        }
        if (m_sfx != null)
        {
            m_sfx.Stop();
        }
        m_collider.enabled = false;

        Destroy(gameObject);
    }

    /// <summary>
    /// �����Ƀq�b�g�����Ƃ��ɑ�����ӂ��Ƃ΂�����
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // �ՓˑΏۂ�Rigidbody�̔z���ł��邩�𒲂ׂ�
        var rigidBody = other.GetComponentInParent<Rigidbody>();

        // Rigidbody�����ĂȂ��Ȃ琁����΂Ȃ��B�I���
        if (rigidBody == null) return;

        //layerMask�̐ݒ�
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Player");

        //ray�̊J�nposition
        m_rayOrigin = transform.position;

        //ray�̖ڕWposition
        Vector3 temp = other.transform.position - transform.position;
        m_rayDirection = temp.normalized;

        // Ray�𐶐�
        Ray _ray = new Ray(m_rayOrigin, m_rayDirection);
        RaycastHit _hit;

        // ����Ray�𓊎˂���layerMask�ɂ���R���C�_�[�ɏՓ˂�����
        if (Physics.Raycast(_ray.origin, _ray.direction * m_rayDistance, out _hit))
        {
            if (_hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                return;
            }
        }

        // �ΏۂƂ̋������v�Z
        float distance = Vector3.Distance(transform.position, other.transform.position);

        // �����ɉ����ĈЗ͂����������� (�Ⴆ�΋������������ƂɈЗ͂�����)
        float maxDistance = 4; // �����̗L���͈�
        float distanceFactor = Mathf.Clamp01(1 - (distance / maxDistance)); // �����ɂ�錸�������v�Z

        // �����ɂ���Ĕ����������琁����ԕ����̃x�N�g�������
        var direction = (other.transform.position - transform.position).normalized;

        direction.z = 0;

        // ������΂��͂̑傫���������ɉ����Č���������
        float adjustedPower = m_explosionPower * distanceFactor;

        // ������΂�
        // ForceMode��ς���Ƌ������ς��i����͎��ʖ����j
        rigidBody.AddForce(direction * adjustedPower, ForceMode.VelocityChange);
    }
}
