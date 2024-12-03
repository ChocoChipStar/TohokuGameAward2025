using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public abstract class BombBase : MonoBehaviour
{
    [SerializeField]
    private GameObject m_explosionPrefab = null;

    [SerializeField]
    protected Rigidbody m_bombbody = null;

    [SerializeField]
    protected SphereCollider m_bombCollider = null;

    [SerializeField]
    private Renderer m_bombRenderer = null;

    [SerializeField] 
    private Material[] m_baseBombMaterials = new Material[2];

    [SerializeField]
    private Animator m_animator = null;

    private MaterialPropertyBlock m_materialPropertyBlock;

    private int m_holdingPlayerNum = 0;

    private float m_explosionTimer = 0.0f;

    private bool m_isIgnited = false;
    private bool m_isGrounded = false;
    private bool m_isNotPlayer = false;

    private float m_elapsedTime = 0;

    private bool m_isActiveFlashMaterial = false;

    private Color m_visibilityMaterial   = new Color(1, 1, 1, 0.6f);
    private Color m_invisibilityMaterial = new Color(1, 1, 1, 0);

    private static readonly int[] FlushSpan = new int[] { 0, 3, 6 };

    private const float SpanLimit = 1.0f;

    private const int FlushMaterialIndex = 1;

    protected BombData m_bombData = null;
    public BombState currentState { get; private set; }

    public Rigidbody Bombbody { get { return m_bombbody; } private set { value = m_bombbody; } }
    public SphereCollider BombCollider { get { return m_bombCollider; } private set { value = m_bombCollider; } }

    public enum BombState
    {
        Idle,
        Dropped,
        Holding,
        Throw,
        Explosion,
        Rolling
    }

    private void Awake()
    {
        currentState = BombState.Dropped;
    }

    private void Update()
    {
        if (!m_isIgnited)
        {
            return;
        }

        StartExplosionCountDown();
        if(m_animator != null)
        {
            m_animator.Play("bomb_anime");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState != BombState.Throw)
        {
            return;
        }

        var isHitPlayer = collision.gameObject.CompareTag(TagData.GetTag(TagData.Names.Player));
        var isThrowingPlayer = collision.gameObject.name == TagData.GetTag(TagData.Names.Player) + (m_holdingPlayerNum + 1);
        // プレイヤーに接触していて且つ、投げた本人以外であれば
        if (isHitPlayer && !isThrowingPlayer)
        {
            CauseAnExplosion();
        }

        var isHitStage = collision.gameObject.CompareTag(TagData.GetTag(TagData.Names.Ground));
        if (isHitStage)
        {
            currentState = BombState.Rolling;
            m_isGrounded = true;
        }
    }

    /// <summary>
    /// 爆弾の爆発を引き起こす処理をします
    /// </summary>
    public void CauseAnExplosion()
    {
        currentState = BombState.Explosion;

        var instanceObj = Instantiate(m_explosionPrefab, this.transform.position, Quaternion.identity);

        var blastManager = instanceObj.GetComponent<ExplosionManager>();
        blastManager.UpdateBombData(m_bombData); // 爆弾データ更新
        blastManager.OnPlayExplosionEffect();    // 爆風エフェクト再生

        Destroy(this.gameObject);                // 爆弾のオブジェクトを破壊する
    }

    /// <summary>
    /// 手持ち状態の初期化を行います
    /// </summary>
    private void InitializeHoldingState(int playerNum)
    {
        m_holdingPlayerNum = playerNum;

        m_bombbody.useGravity = false;
        m_bombbody.velocity = Vector3.zero;
        m_bombbody.angularVelocity = Vector3.zero;

        m_bombCollider.enabled = false;
    }

    /// <summary>
    /// 投げ状態に初期化を行います
    /// </summary>
    private void InitializeThrowState()
    {
        m_bombbody.useGravity = true;
        m_bombCollider.enabled = true;
    }

    /// <summary>
    /// 現在の爆弾が何の種類か確認し取得する
    /// </summary>
    protected void SetBombData(BombData.BombType type)
    {
        m_bombData = BombManager.Instance.GetBombData(type);

        m_bombbody.mass = m_bombData.Params.BombMass;
        m_bombbody.angularDrag = m_bombData.Params.AngularDrag;
    }

    protected abstract void CalculateThrowMovement(Vector3 throwingDirection);

    /// <summary>
    /// 手持ち状態の処理を実行します
    /// </summary>
    public void OnHolding(int holdingPlayerNumber)
    {
        currentState = BombState.Holding;
        InitializeHoldingState(m_holdingPlayerNum);

        // 爆弾に火を付け着火させます。
        m_isIgnited = true;
    }

    /// <summary>
    /// 投げ状態の処理を実行します
    /// </summary>
    public void OnThrow(Vector3 direction)
    {
        currentState = BombState.Throw;
        InitializeThrowState();

        CalculateThrowMovement(direction);
    }

    ///<summary>
    ///タイマーを開始します
    /// </summary>
    private void StartExplosionCountDown()
    {
        m_explosionTimer += Time.deltaTime;
        var remainingTime = m_bombData.Params.ExplosionDelayTime - m_explosionTimer;

        SwitchCurrentFlashMaterial(remainingTime);

        if (m_explosionTimer >= m_bombData.Params.ExplosionDelayTime)
        {
            m_isIgnited = false;
            m_explosionTimer = 0.0f;
            CauseAnExplosion();
        }
    }

    ///<summary>
    ///フラッシュマテリアルを回数によって変更
    /// </summary>
    private void SwitchCurrentFlashMaterial(float remainingTime)
    {
        if (remainingTime > 2)
        {
            SetFlashMaterial(FlushSpan[0]);
        }
        else if (remainingTime > 1)
        {
            m_bombRenderer.material = m_baseBombMaterials[0];
            SetFlashMaterial(FlushSpan[1]);
        }
        else if (remainingTime > 0)
        {
            m_bombRenderer.material = m_baseBombMaterials[1];
            SetFlashMaterial(FlushSpan[2]);
        }
    }

    ///<summary>
    ///フラッシュマテリアルの切り替えタイミングを計算
    /// </summary>
    private void SetFlashMaterial(float changeSpan)
    {
        m_elapsedTime += Time.deltaTime;
        float repeatSpan = SpanLimit / (float)(changeSpan + changeSpan);
        if (m_elapsedTime >= repeatSpan)
        {
            SetActiveFlashMaterial();
            m_elapsedTime = 0;
        }
    }

    ///<summary>
    ///フラッシュマテリアルの表示、非表示
    /// </summary>
    private void SetActiveFlashMaterial()
    {
        if(!m_isActiveFlashMaterial)
        {
            m_materialPropertyBlock = new MaterialPropertyBlock();
            m_materialPropertyBlock.SetColor("_BaseColor", m_visibilityMaterial);
            m_bombRenderer.SetPropertyBlock(m_materialPropertyBlock, FlushMaterialIndex);
            m_isActiveFlashMaterial = true;
            return;
        }
        else
        {
            m_materialPropertyBlock = new MaterialPropertyBlock();
            m_materialPropertyBlock.SetColor("_BaseColor", m_invisibilityMaterial);
            m_bombRenderer.SetPropertyBlock(m_materialPropertyBlock, FlushMaterialIndex);
            m_isActiveFlashMaterial = false;
            return;
        }
    }
}
