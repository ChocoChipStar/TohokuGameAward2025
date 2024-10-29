using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_bombRigidbody;

    [Header("�������")]
    [SerializeField]
    public float m_throwForce = 1f;
    [Header("�΂ߓ����̊p�x")]
    [SerializeField]
    public float m_throwAngle = 45f;
    [Header("��]���x")]
    [SerializeField]
    public float m_rotationSpeed = 10f;


    public void Throw()
    {
        m_bombRigidbody.velocity = Vector3.zero;
        m_bombRigidbody.rotation = Quaternion.identity;
        // �΂ߏ�ɓ��˂��邽�߂̕����x�N�g�����쐬
        float radianAngle = m_throwAngle * Mathf.Deg2Rad; // �p�x�����W�A���ɕϊ�
        Vector3 throwDirection = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0); // �΂ߕ���

        // ���e���΂ߏ�ɓ�����
        m_bombRigidbody.AddForce(throwDirection * m_throwForce, ForceMode.Impulse);
    }

    public void Rowling()
    {
        // Rigidbody�̑��x���擾
        Vector3 velocity = m_bombRigidbody.velocity;

        // �ړ����x�Ɋ�Â��ĉ�]���v�Z
        float speed = velocity.magnitude;

        // z���̉�]�𑬓x�ɉ����Đݒ�
        float rotationAmount = speed * m_rotationSpeed * Time.deltaTime;

        // ���݂̉�]�ɉ��Z
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z - rotationAmount);
    }
}

