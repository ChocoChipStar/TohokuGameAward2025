using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_explosionPrefab = null;

    private void OnCollisionEnter(Collision collision)
    {
        CauseAnExplosion();
    }

    /// <summary>
    /// 何かしらと衝突した際、爆発を発生させます
    /// </summary>
    public void CauseAnExplosion()
    {
        Instantiate(m_explosionPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject); // 爆弾のオブジェクトを破壊する
    }
}
