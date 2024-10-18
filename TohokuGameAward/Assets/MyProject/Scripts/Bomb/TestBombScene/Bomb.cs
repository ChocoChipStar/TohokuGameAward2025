using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("爆発までの時間[s]")]
    [SerializeField]
    private float m_time = 3.0f;

    [Header("爆発の威力")]
    [SerializeField]
    private float m_power = 1;

    [Header("爆風のPrefab")][SerializeField] private Explosion m_explosionPrefab;

    private void Start()
    {
        // 一定時間経過後に発火
        Invoke(nameof(Explode), m_time);
    }

    private void Explode()
    {
        // 爆発を生成
        var explosion = Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
        explosion.Explode(m_power);

        // 自身は消える
        Destroy(gameObject);
    }
}
