using UnityEngine;

public class BlowMover : MonoBehaviour
{
    [SerializeField]
    private HumanoidRespawn m_humanoidRespawn = null;

    [SerializeField]
    private ExplosionData m_explosionData = null;

    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private PlayerAnimator m_humanoidAnimator = null;

    [SerializeField]
    private Rigidbody m_humanoidRigidbody = null;

    private HumanoidMover m_humanoidMover = null;

    private Vector3 m_playerGetoutVelocity = Vector3.zero;

    private bool m_isBlow = false;

    private void Update()
    {
        if (!m_isBlow)
        {
            return;
        }

        if (!PlayerBlowChecker())
        {
            CanTransitionAnimation();
        }

        GetOutMover();
    }

    /// <summary>
    /// 爆風で触れた物体が吹き飛ぶ方向を計算します
    /// </summary>
    private Vector3 GetExplosionDirection(Vector3 targetPos)
    {
        var direction = transform.position - targetPos;
        float xDirection = direction.x;
        float yDirection = Mathf.Abs(direction.x) + Mathf.Abs(direction.y);
        return new Vector3(xDirection, yDirection, 0.0f).normalized;
    }

    /// <summary>
    /// 爆発を受けた後の処理を実行します。
    /// </summary>
    public void GetExplosion(Rigidbody rigidbody, Vector3 BombPos, Collider collider, HumanoidMover mover)
    {
        if (rigidbody == null || m_isBlow)
        {
            return;
        }

        m_isBlow = true;
        m_playerGetoutVelocity = GetExplosionDirection(BombPos);
        SetHumanoid(rigidbody, mover);
        SetActiveHumanoid(collider);
        StaetAnimation(collider);
    }

    private void SetHumanoid(Rigidbody rigidbody, HumanoidMover mover)
    {
        m_humanoidRigidbody = rigidbody;
        m_humanoidMover = mover;
    }

    private void SetActiveHumanoid(Collider collider)
    {
        collider.enabled = false;
        m_humanoidRigidbody.useGravity = false;
    }

    private void StaetAnimation(Collider collider)
    {
        var parentObj = collider.transform.parent.gameObject;
        if (TagManager.Instance.SearchedTagName(parentObj, TagManager.Type.Player))
        {
            m_humanoidMover.GetExplosion(true);

            // 吹き飛びアニメーション開始
            m_humanoidAnimator.ChangeTopState(PlayerAnimator.TopState.Blow);
            m_humanoidAnimator.ChangeUnderState(PlayerAnimator.UnderState.Blow);
        }
    }

    private void CanTransitionAnimation()
    {
        m_humanoidMover.GetExplosion(false);
        m_isBlow = false;
        m_humanoidRigidbody.useGravity = true;
        var collider = m_humanoidRigidbody.gameObject.GetComponentInChildren<Collider>();
        collider.enabled = true;

        // アニメーション遷移可能状態にする
        m_humanoidAnimator.TransferableState(top: PlayerAnimator.TopState.Blow);
        m_humanoidAnimator.TransferableState(under: PlayerAnimator.UnderState.Blow);
    }

    private void GetOutMover()
    {
        this.transform.position += m_playerGetoutVelocity * Time.deltaTime;
    }

    private bool PlayerBlowChecker()
    { 
        if (m_humanoidRespawn.IsDead[m_inputData.SelfNumber])
        {
            return false;
        }
        return true;
    }
}