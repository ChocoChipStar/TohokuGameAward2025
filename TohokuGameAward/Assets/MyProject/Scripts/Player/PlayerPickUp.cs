using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static PlayerAnimator;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField]
    private Transform m_leftArmTransform = null;

    [SerializeField]
    private Transform m_rightArmTranform = null;

    [SerializeField]
    private PlayerAnimator m_animator = null;

    [SerializeField]
    private PlayerDirectionRotator m_directionRotator = null;

    [SerializeField]
    private InputData m_inputData = null;

    private Vector3 intermediateArmPos = Vector3.zero;

    private bool m_isPickup = false;
    private bool m_isDetected = false;

    public GameObject DetectedItemObj { get; private set; } = null;
    public bool IsHoldingItem { get; private set; } = false;
    public bool IsPuckUp { get { return m_isPickup; }  }

    private void Update()
    {
        if (DetectedItemObj == null)
        {
            m_animator.TransferableState(TopState.Pickup);
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_isPickup)
        {
            return;
        }

        if (TagManager.Instance.SearchedTagName(other.gameObject,TagManager.Type.Bomb))
        {
            var bombBase = other.gameObject.GetComponentInParent<BombBase>();
            //if(bombBase.currentState == BombBase.BombState.Throw)
            //{
            //    return;
            //}

            m_isDetected = true;
            DetectedItemObj = other.gameObject.transform.parent.gameObject;
            
            m_isPickup = true;
            m_animator.ChangeTopState(TopState.Pickup);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 手持ちアイテムが爆発して無くなっているか、まだ手持ちにアイテムを持っている場合はReturnする
        if(DetectedItemObj == null || IsHoldingItem)
        {
            return;
        }

        var otherObj = other.gameObject;
        if(TagManager.Instance.SearchedTagName(otherObj, TagManager.Type.Ground, TagManager.Type.Wall))
        {
            return;
        }

        var parentObj = otherObj.transform.parent.gameObject.GetInstanceID();
        if (parentObj != DetectedItemObj.GetInstanceID())
        {
            return;
        }

        if (TagManager.Instance.SearchedTagName(otherObj, TagManager.Type.Detected))
        {
            m_animator.TransferableState(TopState.Pickup);
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
        //bombBase.OnHolding(m_inputData.SelfNumber);
    }

    /// <summary>
    /// 拾ったアイテムを持たせ続ける処理を実行します
    /// </summary>
    private void HoldingDetectedItem()
    {
        intermediateArmPos = Vector3.Lerp(m_leftArmTransform.position, m_rightArmTranform.position, 0.5f);
        if (m_directionRotator.IsRight.Value)
        {    
            DetectedItemObj.transform.position = intermediateArmPos;
            return;
        }
        DetectedItemObj.transform.position = intermediateArmPos;
    }

    /// <summary>
    /// アイテムを拾う処理の初期化を行います
    /// </summary>
    public void InitializedPickup()
    {
        IsHoldingItem = false;
    }
}
