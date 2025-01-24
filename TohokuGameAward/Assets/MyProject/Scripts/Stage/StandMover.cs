using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class StandMover : MonoBehaviour
{
    [SerializeField,Header("足場の移動速度")]
    private float m_moveSpeed = 0.0f;

    [SerializeField,Header("足場移動ループの有無")]
    private ReactiveProperty<bool> m_isLoop = new ReactiveProperty<bool>(false);

    [SerializeField,Header("足場を回転の有無")]
    private ReactiveProperty<bool> m_isCircular = new ReactiveProperty<bool>(false);

    [SerializeField,Header("足場回転の半径")]
    private float m_circularRadius = 0.0f;

    [SerializeField]
    private GameObject m_modelObj = null;

    [SerializeField]
    private GameObject m_endPosObj = null;

    [SerializeField]
    private GameObject m_middlePosStorageObj = null;

    private int m_middlePosCounter = 0;

    private bool m_isTurn = false;

    private Vector2 m_startPos = Vector2.zero;
    private Vector2 m_currentTargetPos = Vector2.zero;

    private ReactiveProperty<Vector3> m_circularCenterPos = new ReactiveProperty<Vector3>();

    private List<GameObject> m_middlePosObj = new List<GameObject>();

    private const int MoveStartPosIndex = -1;

    private const float NearlyValue = 0.05f;
    private const float RotationSpeedRate = 50.0f;

    private void Awake()
    {
        m_isLoop.Subscribe(msg => UpdateRadius()).AddTo(this);
        m_isCircular.Subscribe(msg => UpdateRadius()).AddTo(this);
        m_circularCenterPos.Subscribe(msg => UpdateRadius()).AddTo(this);
    }

    private void Start()
    {
        InitializePositions();
    }

    private void Update()
    {
        if(m_isCircular.Value)
        {
            m_circularCenterPos.Value = new Vector3(m_startPos.x, m_circularRadius, 0.0f);
            var angleAxis = Quaternion.AngleAxis(360 / (m_moveSpeed / RotationSpeedRate) * Time.deltaTime, new Vector3(0,0,1));

            var position = m_modelObj.transform.position;

            position += -m_circularCenterPos.Value;
            position = angleAxis * position;
            position += m_circularCenterPos.Value;

            m_modelObj.transform.position = position;

            return;
        }

        m_modelObj.transform.position = GetNextFramePos();
    }

    private void InitializePositions()
    {
        m_startPos = this.transform.position;
        m_circularCenterPos.Value = new Vector3(m_startPos.x, m_circularRadius, 0.0f);

        // 中間地点が存在しない場合は初期ターゲットを終点にし、returnする
        var middlePosCount = m_middlePosStorageObj.transform.childCount;
        if (middlePosCount == 0)
        {
            m_currentTargetPos = m_endPosObj.transform.position;
            return;
        }

        // 中間地点が存在する場合はリストに格納していく
        for (int i = 0; i < middlePosCount; i++)
        {
            m_middlePosObj.Add(m_middlePosStorageObj.transform.GetChild(i).gameObject);
        }

        // 初期ターゲットを中間地点の一つ目に設定
        m_currentTargetPos = m_middlePosObj[0].transform.position;
    }

    private void UpdateRadius()
    {
        m_modelObj.transform.position = m_startPos;
    }

    private Vector2 GetNextFramePos()
    {
        SetCurrentTarget();
        return Vector2.MoveTowards(m_modelObj.transform.position, m_currentTargetPos, m_moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 現在の移動先（ターゲット座標）を取得する処理を行います
    /// </summary>
    private void SetCurrentTarget()
    {
        if(Vector2.Distance((Vector2)m_modelObj.transform.position, m_currentTargetPos) >= NearlyValue)
        {
            // 現在地点がターゲット座標より離れていれば引き続きターゲットを変えないで返す
            return;
        }
        SwitchNextMoveTarget();
    }

    /// <summary>
    /// 移動先（ターゲット座標）を次点に切り替える処理を行います
    /// </summary>
    private void SwitchNextMoveTarget()
    {
        var moveEndPosIndex = m_middlePosObj.Count;

        SwitchCounter(moveEndPosIndex);
        if (m_middlePosCounter == moveEndPosIndex)
        {
            SetNextPosition(m_endPosObj.transform.position, true);
            return;
        }
        
        if(m_middlePosCounter == MoveStartPosIndex)
        {
            SetNextPosition(m_startPos, false);
            return;
        }

        SetNextPosition(m_middlePosObj[m_middlePosCounter].transform.position, m_isTurn);
    }

    private void SetNextPosition(Vector2 position, bool isTurn)
    {
        m_currentTargetPos = position;
        m_isTurn = isTurn;
    }

    private void SwitchCounter(int moveEndPosIndex)
    {
        if (m_isLoop.Value)
        {
            // 終点まで移動したらループ時はスタート位置に移動するようにする
            if (m_middlePosCounter == moveEndPosIndex)
            {
                m_middlePosCounter = MoveStartPosIndex; 
                return;
            }
        }

        if(m_isTurn) 
        {
            // 終点まで来たらカウントを減らしていく
            m_middlePosCounter--;
            return;
        }
        m_middlePosCounter++;
    }
}
