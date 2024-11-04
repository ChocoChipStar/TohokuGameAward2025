using System.Collections;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField]
    private PlayerThrow m_playerThrow = null;

    [SerializeField]
    private PlayerInputData m_inputData = null;

    [SerializeField]
    private BoxCollider m_detectionCollider = null;

    private float m_lastFramePosX = 0.0f;

    private bool m_isCantPickUp = false;
    private bool m_isDetected = false;

    private const float DiffDetectionRange = 0.01f;

    public GameObject DetectedItemObj { get; private set; } = null;
    public bool IsRight { get; private set; } = false;
    public bool IsHoldingItem { get; private set; } = false;

    private void Update()
    {
        if(m_isDetected)
        {
            InitializeDetectedItem();
        }

        if(IsHoldingItem)
        {
            HoldingDetectedItem();
        }

        m_lastFramePosX = this.transform.position.x;
    }

    /// <summary>
    /// 拾ったアイテムを持たせるための初期化処理を実行します
    /// </summary>
    private void InitializeDetectedItem()
    {
        IsHoldingItem = true;
        m_isDetected = false;

        var rigidbody = DetectedItemObj.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        var sphereCollider = DetectedItemObj.GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
    }

    /// <summary>
    /// 拾ったアイテムを持たせ続ける処理を実行します
    /// </summary>
    private void HoldingDetectedItem()
    {
        if(m_playerThrow.IsThrow || DetectedItemObj == null)
        {
            IsHoldingItem = false;
            StartCoroutine(m_playerThrow.ResetItemProcess());
            return;
        }

        if (GetDirection())
        {
            var holdingItemPosR = this.transform.position.x + this.transform.localScale.x;
            DetectedItemObj.transform.position = new Vector3(holdingItemPosR, this.transform.position.y, 0.0f);
            return;
        }

        var holdingItemPosL = this.transform.position.x - this.transform.lossyScale.x;
        DetectedItemObj.transform.position = new Vector3(holdingItemPosL, this.transform.position.y, 0.0f);
    }

    /// <summary>
    /// 現在の向きを取得出来ます
    /// </summary>
    /// <returns> true->右向き false->左向き </returns>
    private bool GetDirection()
    {
        var diffValue = this.transform.position.x - m_lastFramePosX;
        if (diffValue >= -DiffDetectionRange && diffValue <= DiffDetectionRange)
        {
            return IsRight;
        }

        if (diffValue > DiffDetectionRange)
        {
            IsRight = true;
        }

        if(diffValue < DiffDetectionRange)
        {
            IsRight = false;
        }

        return IsRight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_isCantPickUp || Bomb.IsPlayerThrown)
        {
            return;
        }

        if (other.gameObject.CompareTag(TagData.NameList[(int)TagData.Number.Item]))
        {
            m_isDetected = true;
            m_isCantPickUp = true;
            DetectedItemObj = other.gameObject;

            var bomb = DetectedItemObj.GetComponent<Bomb>();
            bomb.SetPlayerData(this, m_playerThrow);
        }
    }

    /// <summary>
    /// アイテム拾い処理の初期化を行います
    /// </summary>
    public void InitializedPickUp()
    {
        m_isCantPickUp = false;
    }
}
