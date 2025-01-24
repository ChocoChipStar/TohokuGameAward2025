using UnityEngine;
using UnityEngine.Splines;

public class CannonMover : MonoBehaviour
{
    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private CannonDistance m_distanceManager = null;

    [SerializeField]
    private SplineAnimate m_splineAnimate = null;

    [SerializeField]
    private SplineContainer m_splineContainer = null;

    [SerializeField, Range(0.0f, 1.0f)]
    private float m_cannonPosition = 0.0f;

    //ループ抑制用移動域
    private const float m_moveMax = 0.98f;
    private const float m_moveMin = 0.02f;

    private const float m_oneFrame = 60.0f;
    private const float m_percentage = 100.0f;


    public float CannonPosition { get { return m_cannonPosition; } private set { } }

    private void Start()
    {
        m_distanceManager = this.GetComponentInParent<CannonDistance>();
    }

    void Update()
    {
        if (CanMove())
        {
            MoveOperation();
        }

        if (m_moveMax < m_cannonPosition)
        {
            StopControl(m_moveMax);
        }
        else if(m_moveMin > m_cannonPosition)
        {
            StopControl(m_moveMin);
        }
    }

    //大砲の移動処理
    private void MoveOperation()
    {
        if (m_inputData.WasPressedActionButton(InputData.ActionsName.RightRail, m_inputData.SelfNumber))
        {
            m_cannonPosition += SpeedPercent(m_cannonData.Params.MoveSpeed);
        }
        if (m_inputData.WasPressedActionButton(InputData.ActionsName.LeftRail, m_inputData.SelfNumber))
        {
            m_cannonPosition -= SpeedPercent(m_cannonData.Params.MoveSpeed);
        }

        m_splineAnimate.NormalizedTime = m_cannonPosition;
    }

    private void StopControl(float pos)
    {
        m_cannonPosition = pos;
        m_splineAnimate.NormalizedTime = pos;
    }

    public void FixCannonPosition(float pos)
    {
        m_cannonPosition = pos;
        m_splineAnimate.NormalizedTime = m_cannonPosition;
    }

    private bool CanMove()
    {
        if(m_inputData.WasPressedActionButton(InputData.ActionsName.RightRail, m_inputData.SelfNumber)
          || m_inputData.WasPressedActionButton(InputData.ActionsName.LeftRail, m_inputData.SelfNumber)
          || !m_distanceManager.IsHitCannon)
        {
            return true;            
        }
        return false;
    }

    /// <summary>
    /// 大砲移動速度計算
    /// </summary>
    private float SpeedPercent(float speed)
    {
        var speedPerSecond = (1.0f / m_oneFrame);
        var speedPerLength = (1.0f / m_percentage);
        return speedPerSecond * (speed * speedPerLength);
    }

    public void InitializeSpline()
    {
        m_splineAnimate.Container = m_splineContainer;
    }
    
    public void InitializePosition(int cannonNum)
    {
        m_cannonPosition = m_cannonData.Positions.StartPosition[cannonNum];
        m_splineAnimate.NormalizedTime = m_cannonData.Positions.StartPosition[cannonNum];
    }
}