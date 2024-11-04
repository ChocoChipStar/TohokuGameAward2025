using UnityEngine;
using UnityEngine.UI;
using static BombData;

public class Bomb : MonoBehaviour
{
    [SerializeField, Header("爆風のPrefab")]
    private Explosion m_explosionPrefab;

    [SerializeField, Header("爆弾のコライダー")]
    private SphereCollider m_collider;

    [SerializeField, Header("爆弾のモデル")]
    private GameObject m_bombModel;

    [SerializeField]
    private BombController m_bombController;

    [SerializeField]
    private Text m_countDownText;

    private BombData m_bombData;
    private BombManager m_bombManager;

    private bool m_isIgnited = false;

    //デバック用
    private int m_currentExplosionTime;
    private bool isThrowFlagLastFrameActiveChecker = false;
    private bool isIgnitedFlagLastFrameActiveChecker = false;
    private float m_elapsedTime = 0;

    //爆弾データ保存用
    private float m_time  = 0;
    private float m_power = 0;
    private float m_size  = 0;
    private float m_pivot = 0;
    private int   m_count = 0;
    private GameObject m_bombPrefab;

    public BombGenre bombGenre;

    public static bool IsPlayerThrown { get; private set; } = false;

    private void Start()
    {
        // BombManagerからこの爆弾のジャンルに対応するBombDataを取得
        // シングルトンを使用してBombManagerにアクセス
        m_bombData = BombManager.Instance.GetBombDataByGenre(bombGenre);

        ApplyBombData(m_bombData);

        m_currentExplosionTime = (int)m_time;       // カウントダウン開始時間を設定
        m_countDownText.text = m_currentExplosionTime.ToString();
    }

    // 取得したBombDataの値を使って、爆弾の設定を反映するメソッド
    private void ApplyBombData(BombData data)
    {
        m_time  = m_bombData.ExplosionTime;
        m_power = m_bombData.BlastPower;
        m_size  = m_bombData.BlastRange;
        m_pivot = m_bombData.BombPivot;
        if(m_count == 0)
        {
            m_count = m_bombData.BombCount;
        }
        if(m_bombData.OtherBombObj != null)
        {
            m_bombPrefab = m_bombData.OtherBombObj;
        }
    }

    private void Update()
    {
        m_countDownText.rectTransform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.7f, 0));

        if(PlayerThrow.IsThrow && !isThrowFlagLastFrameActiveChecker)
        {
            ThrowBomb();
            isThrowFlagLastFrameActiveChecker = PlayerThrow.IsThrow;
        }

        if(PlayerPickUp.IsHoldingItem && !isIgnitedFlagLastFrameActiveChecker)
        {
            IgnitedTheBomb();
            isIgnitedFlagLastFrameActiveChecker = PlayerPickUp.IsHoldingItem;
        }

        if (!m_isIgnited)
        {
            return;
        }

        if (m_currentExplosionTime == 0)
        {
            // 計測時間を超えたら初期化する
            m_countDownText.text = "0";
            m_isIgnited = false;
        }

        m_elapsedTime += Time.deltaTime;
        if (m_elapsedTime >= 1.0f)
        {
            m_currentExplosionTime--;
            m_countDownText.text = m_currentExplosionTime.ToString();
            m_elapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// 爆弾を着火させる処理を行います
    /// </summary>
    public void IgnitedTheBomb()
    {
        // 一定時間経過後に発火
        if(m_bombData.BombType != BombData.BombGenre.Mini && !m_isIgnited)
        {
            Invoke(nameof(BombExplosion), m_time);
            m_isIgnited = true;
        }
    }

    public void ThrowBomb()
    {
        m_collider.center = new Vector3(0, m_pivot, 0);
        IsPlayerThrown = true;

        if (m_bombData.BombType == BombData.BombGenre.Mini)
        {
            m_count--;
            var mini = Instantiate(m_bombPrefab, transform.position, Quaternion.identity);
            Bomb miniBomb = mini.GetComponent<Bomb>();
            miniBomb.m_count = m_count;
        }
        m_count = 1;
    }

    private void BombExplosion()
    {
        var instanceObj = Instantiate(m_explosionPrefab, m_collider.transform.position, Quaternion.identity);
        instanceObj.SetBlastPower(m_power, m_size); // 爆弾の爆発威力を反映させる

        m_count = 0;
        isThrowFlagLastFrameActiveChecker = false;
        isIgnitedFlagLastFrameActiveChecker = false;
        IsPlayerThrown = false;
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 爆弾が何かと衝突したらPivot位置リセットする
        m_collider.center = new Vector3(0, 0, 0);

        if (IsPlayerThrown && !PlayerThrow.IsThrow && m_bombData.BombType == BombData.BombGenre.Mini)
        {
            BombExplosion(); // ミニボムの場合は即起爆させる
        }

        if (collision.gameObject.CompareTag(TagData.NameList[(int)TagData.Number.Stage]))
        {
            IsPlayerThrown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagData.NameList[(int)TagData.Number.Player]))
        {
            if (IsPlayerThrown && !PlayerThrow.IsThrow)
            {
                BombExplosion();
            }
        }
    }
}
