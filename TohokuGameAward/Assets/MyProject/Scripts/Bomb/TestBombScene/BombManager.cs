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

    [Header("爆風のPrefab")][SerializeField] private Explosion m_explosionPrefab;

    [Header("爆弾のコライダー")]
    [SerializeField]
    private Collider m_collider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //カウントダウンスタート
            FuseOn();
        }
    }

    private void FuseOn()
    {
        // 一定時間経過後に発火
        Invoke(nameof(Explode), m_time);
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
}
