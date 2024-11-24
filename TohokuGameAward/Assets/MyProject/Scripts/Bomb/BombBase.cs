using Unity.VisualScripting;
using UnityEngine;

public abstract class BombBase : MonoBehaviour
{
    [SerializeField]
    private GameObject m_explosionPrefab = null;

    [SerializeField]
    protected Rigidbody m_bombbody = null;

    [SerializeField]
    protected SphereCollider m_bombCollider = null;

    private int holdingPlayerNum = 0;

    private bool m_isGrounded = false;
    private bool m_isNotPlayer = false;

    public BombState currentState { get; private set; }
    
    protected BombData m_bombData = null;

    public Rigidbody Bombbody { get { return m_bombbody; } private set { value = m_bombbody; } }
    public SphereCollider BombCollider { get { return m_bombCollider; } private set { value = m_bombCollider;} }

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
        //Debug.Log(currentState);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState != BombState.Throw)
        {
            return;
        }

        var isHitPlayer = collision.gameObject.CompareTag(TagData.GetTag(TagData.Names.Player));
        var isThrowingPlayer = collision.gameObject.name == TagData.GetTag(TagData.Names.Player) + (holdingPlayerNum + 1);
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
    private void CauseAnExplosion()
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
        holdingPlayerNum = playerNum;

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
        InitializeHoldingState(holdingPlayerNum);

        // 爆弾に火を付け着火させます。
        Invoke(nameof(CauseAnExplosion), m_bombData.Params.ExplosionDelayTime);
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
}
