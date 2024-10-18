using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("�����܂ł̎���[s]")]
    [SerializeField]
    private float m_time = 3.0f;

    [Header("�����̈З�")]
    [SerializeField]
    private float m_power = 1;

    [Header("������Prefab")][SerializeField] private Explosion m_explosionPrefab;

    private void Start()
    {
        // ��莞�Ԍo�ߌ�ɔ���
        Invoke(nameof(Explode), m_time);
    }

    private void Explode()
    {
        // �����𐶐�
        var explosion = Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
        explosion.Explode(m_power);

        // ���g�͏�����
        Destroy(gameObject);
    }
}
