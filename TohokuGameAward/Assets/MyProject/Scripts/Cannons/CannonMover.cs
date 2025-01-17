using UnityEngine;
using UnityEngine.Splines;

public class CannonMover : MonoBehaviour
{
    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private SplineAnimate m_splineAnimate = null;

    [SerializeField]
    private SplineContainer m_gameObject = null;

    [SerializeField]
    private Canvas m_canvas = null;



    [SerializeField, Range(0.0f, 100.0f)]
    private float m_cannonPosition = 0.0f;

    //ループ抑制用移動域
    private const float m_moveMax = 0.97f;
    private const float m_moveMin = 0.03f;

    private const float m_oneFrame = 60.0f;
    private const float m_percentage = 100.0f;

    private const float m_canvasRotation = 270.0f;

    public float CannonPosition { get { return m_cannonPosition; } private set { } }


    // Update is called once per frame
    void Update()
    {
        if (CannonCanMove())
        {
            CannonMoveOperation();
        }

        CanvasRotationFix();
    }

    //大砲の移動処理
    private void CannonMoveOperation()
    {
        if (m_inputData.WasPressedActionButton(InputData.ActionsName.RightRail, m_inputData.SelfNumber)
         && m_moveMax > m_cannonPosition)
        {
            m_cannonPosition += CannonSpeedPercent(m_cannonData.Params.MoveSpeed);
        }
        if (m_inputData.WasPressedActionButton(InputData.ActionsName.LeftRail, m_inputData.SelfNumber)
         && m_moveMin < m_cannonPosition)
        {
            m_cannonPosition -= CannonSpeedPercent(m_cannonData.Params.MoveSpeed);
        }

        m_splineAnimate.NormalizedTime = m_cannonPosition;
    }

    public void FixCannonPosition(float pos)
    {
        m_cannonPosition = pos;
        m_splineAnimate.NormalizedTime = m_cannonPosition;
    }

    //大砲が移動可能かどうかを調べる
    private bool CannonCanMove()
    {
        if(m_inputData.WasPressedActionButton(InputData.ActionsName.RightRail, m_inputData.SelfNumber)
          || m_inputData.WasPressedActionButton(InputData.ActionsName.LeftRail, m_inputData.SelfNumber))
        {
            return true;            
        }
        return false;
    }

    //大砲移動速度計算
    private float CannonSpeedPercent(float speed)
    {
        var speedPerSecond = (1.0f / m_oneFrame);
        var speedPerLength = (1.0f / m_percentage);
        return speedPerSecond * (speed * speedPerLength);
    }

    //大砲出現時の初期化等
    public void CreateStart(int playerNum)
    {
        m_splineAnimate.Container = m_gameObject;
        m_cannonPosition = m_cannonData.Positions.StartPosition[playerNum];
        m_splineAnimate.NormalizedTime = m_cannonData.Positions.StartPosition[playerNum];
    }

    private void CanvasRotationFix()
    {
        var rotation = m_canvas.transform.rotation;
        rotation.x = m_canvasRotation - this.transform.rotation.x;
        m_canvas.transform.rotation = rotation;
    }
}
