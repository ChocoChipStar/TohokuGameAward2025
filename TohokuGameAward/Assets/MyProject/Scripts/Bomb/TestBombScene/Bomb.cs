using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static BombData;

public class Bomb : MonoBehaviour
{
    // この爆弾のジャンルをインスペクターで選択
    public BombGenre bombGenre;

    // 爆弾のデータ
    private BombData currentBombData;

    // Bombへの参照（シーン内のBombオブジェクトを参照）
    private BombManager bombManager;

    [Header("爆風のPrefab")][SerializeField] private Explosion m_explosionPrefab;

    [Header("爆弾のコライダー")]
    [SerializeField]
    private SphereCollider m_collider;

    [Header("爆弾のモデル")]
    [SerializeField]
    private GameObject m_bombModel;

    [SerializeField]
    private BombController m_bombController;

    //仮のカウントダウン
    [SerializeField]
    private Text m_text;

    //true = 爆弾が投射された
    public bool isThrown = false;       
    
    //true = playerに当たった時、爆発可能
    private bool isPlayerDirectExplode = false;

    //true = 回転中
    private bool isRowling = false;


    //デバック用
    private bool isTimerStart = false;
    private float m_currentTime;
    private float m_timer = 0;

    //爆弾データ保存用
    private float m_time  = 0;
    private float m_power = 0;
    private float m_size  = 0;
    private float m_pivot = 0;
    private int   m_count = 0;
    private GameObject m_bombPrefab;

    private void Start()
    {
        // BombManagerからこの爆弾のジャンルに対応するBombDataを取得
        // シングルトンを使用してBombManagerにアクセス
        currentBombData = BombManager.Instance.GetBombDataByGenre(bombGenre);

        ApplyBombData(currentBombData);

        m_currentTime = m_time;       // カウントダウン開始時間を設定
        m_text.text = m_currentTime.ToString();
    }

    // 取得したBombDataの値を使って、爆弾の設定を反映するメソッド
    private void ApplyBombData(BombData data)
    {
        m_time  = currentBombData.time;
        m_power = currentBombData.power;
        m_size  = currentBombData.size;
        m_pivot = currentBombData.pivot;
        if(m_count == 0)
        {
            m_count = currentBombData.count;
        }
        if(currentBombData.bomb != null)
        {
            m_bombPrefab = currentBombData.bomb;
        }
    }

    private void Update()
    {
        m_text.rectTransform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.7f, 0));

        if(isTimerStart)
        {
            if (m_currentTime > 0) // カウントが0より大きい場合にのみ実行
            {
                m_timer += Time.deltaTime; // 経過時間を加算

                if (m_timer >= 1f) // 1秒経過したら
                {
                    m_currentTime--;      // カウントを1減らす
                    m_text.text = m_currentTime.ToString(); // Textに表示
                    m_timer = 0f;         // タイマーをリセット
                }
            }
            else
            {
                OnCountdownEnd(); // カウントダウンが終了したときの処理
            }
        }

        if (isRowling)
        {
            m_bombController.Rowling();
        }
    }

    private void OnCountdownEnd()
    {
        m_text.text = "0";
    }

    public void FuseOn()
    {
        // 一定時間経過後に発火
        Invoke(nameof(Explode), m_time);
        isTimerStart = true;
    }

    public void ThrowBomb()
    {
        if (!isThrown)
        {
            m_collider.center = new Vector3(0, m_pivot, 0);
            m_bombModel.transform.localPosition = new Vector3(0, m_pivot, 0);
            m_bombController.Throw();
            isPlayerDirectExplode = true;
            isThrown = true;
            isRowling = true;

            if (m_count > 1)
            {
                m_count--;
                var mini = Instantiate(m_bombPrefab, transform.position, Quaternion.identity);
                Bomb miniBomb = mini.GetComponent<Bomb>();
                miniBomb.m_count = m_count;
            }
            
            m_count = 1;
        }
    }

    private void Explosion()
    {
        //即爆破
        Explode();
    }

    private void Explode()
    {
        // 爆発を生成
        var explosion = Instantiate(m_explosionPrefab, m_collider.transform.position, Quaternion.identity);
        //威力の設定
        explosion.Explode(m_power, m_size);

        m_count = 0;

        // 自身は消える
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //なにかに触れたら
        isRowling = false;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isPlayerDirectExplode = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (isPlayerDirectExplode)
            {
                Explosion();
            }
            else if(!isThrown)
            {
                FuseOn();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isThrown)
            {
                isPlayerDirectExplode = true;
            }
        }
    }
}
