using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("”š”­‚Ü‚Å‚ÌŠÔ[s]")]
    [SerializeField]
    private float m_time = 3.0f;

    [Header("”š”­‚ÌˆĞ—Í")]
    [SerializeField]
    private float m_power = 1;

    [Header("”š•—‚ÌPrefab")][SerializeField] private Explosion m_explosionPrefab;

    private void Start()
    {
        // ˆê’èŠÔŒo‰ßŒã‚É”­‰Î
        Invoke(nameof(Explode), m_time);
    }

    private void Explode()
    {
        // ”š”­‚ğ¶¬
        var explosion = Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
        explosion.Explode(m_power);

        // ©g‚ÍÁ‚¦‚é
        Destroy(gameObject);
    }
}
