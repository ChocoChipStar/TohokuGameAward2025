using UnityEngine;

public abstract class BombBase : MonoBehaviour
{
    [SerializeField]
    private GameObject m_explosionPrefab = null;

    protected BombData m_bombData = null;

    private void OnCollisionEnter(Collision collision)
    {
        CauseAnExplosion();
    }

    /// <summary>
    /// 爆弾の爆発を引き起こす処理をします
    /// </summary>
    public void CauseAnExplosion()
    {
        var instanceObj = Instantiate(m_explosionPrefab, this.transform.position, Quaternion.identity);

        var blastManager = instanceObj.GetComponent<ExplosionManager>();
        blastManager.UpdateBombData(m_bombData); // 爆弾データ更新
        blastManager.OnPlayExplosionEffect();    // 爆風エフェクト再生

        Destroy(this.gameObject);                // 爆弾のオブジェクトを破壊する
    }
}
