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

    [Header("������Prefab")][SerializeField] private Explosion m_explosionPrefab;

    [Header("���e�̃R���C�_�[")]
    [SerializeField]
    private Collider m_collider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //�J�E���g�_�E���X�^�[�g
            FuseOn();
        }
    }

    private void FuseOn()
    {
        // ��莞�Ԍo�ߌ�ɔ���
        Invoke(nameof(Explode), m_time);
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
}
