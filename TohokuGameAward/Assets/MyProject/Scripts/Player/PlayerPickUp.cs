using System.Collections;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField]
    private PlayerThrow m_playerThrow = null;

    [SerializeField]
    private PlayerInputData m_inputData = null;

    [SerializeField]
    private BoxCollider m_detectionCollider = null;

    private float m_lastFramePosX = 0.0f;

    private bool m_isPickup = false;
    private bool m_isDetected = false;

    private const float DiffDetectionRange = 0.01f;

    public GameObject DetectedItemObj { get; private set; } = null;
    public bool IsRight { get; private set; } = false;
    public bool IsHoldingItem { get; private set; } = false;

    private void Update()
    {
        if (DetectedItemObj == null)
        {
            m_isPickup = false;
            return;
        }

        if (m_isDetected)
        {
            InitializeDetectedItem();
        }

        if (IsHoldingItem)
        {
            HoldingDetectedItem();
        }

        m_lastFramePosX = this.transform.position.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_isPickup)
        {
            return;
        }

        if (other.gameObject.CompareTag(TagData.GetTag(TagData.Names.Bomb)))
        {
            var bombBase = other.gameObject.GetComponentInParent<BombBase>();
            if(bombBase.currentState == BombBase.BombState.Throw)
            {
                return;
            }

            m_isDetected = true;
            m_isPickup = true;
            DetectedItemObj = other.gameObject.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 手持ちアイテムが爆発して無くなっているか、まだ手持ちにアイテムを持っている場合はReturnする
        if(DetectedItemObj == null || IsHoldingItem)
        {
            return;
        }

        var parentObj = other.gameObject.transform.parent.gameObject.GetInstanceID();
        if (parentObj != DetectedItemObj.GetInstanceID())
        {
            return;
        }

        if (other.gameObject.CompareTag(TagData.GetTag(TagData.Names.Detected)))
        {
            m_isPickup = false;
        }
    }

    /// <summary>
    /// 拾ったアイテムを持たせるための初期化処理を実行します
    /// </summary>
    private void InitializeDetectedItem()
    {
        IsHoldingItem = true;
        m_isDetected = false;

        var bombBase = DetectedItemObj.GetComponent<BombBase>();
        bombBase.OnHolding(m_inputData.SelfNumber);
    }

    /// <summary>
    /// 拾ったアイテムを持たせ続ける処理を実行します
    /// </summary>
    private void HoldingDetectedItem()
    {
        if (GetDirection())
        {
            var holdingItemPosR = this.transform.position.x + this.transform.localScale.x;
            DetectedItemObj.transform.position = new Vector3(holdingItemPosR, this.transform.position.y, 0.0f);
            return;
        }

        var holdingItemPosL = this.transform.position.x - this.transform.localScale.x;
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

        if (diffValue < DiffDetectionRange)
        {
            IsRight = false;
        }

        return IsRight;
    }

    /// <summary>
    /// アイテムを拾う処理の初期化を行います
    /// </summary>
    public void InitializedPickup()
    {
        IsHoldingItem = false;
    }
}
