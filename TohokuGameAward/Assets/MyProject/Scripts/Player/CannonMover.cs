using UnityEngine;
using UnityEngine.Splines;

public class CannonMover : MonoBehaviour
{
    [SerializeField]
    private PlayerInputData m_inputData = null;

    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private SplineAnimate m_splineAnimate = null;

    [SerializeField]
    private SplineContainer m_gameObject = null;

    [SerializeField, Range(0.0f, 100.0f)]
    private float m_cannonPosition = 0.0f;

    //ループ抑制用移動域
    private const float m_moveMax = 0.97f;
    private const float m_moveMin = 0.03f;

    private const float m_oneFrame = 60.0f;
    private const float m_percentage = 100.0f;

    public float CannonPosition { get { return m_cannonPosition; } private set { } }


    // Update is called once per frame
    void Update()
    {
        if (CannonCanMove())
        {
            CannonMoveOperation();
        }
    }

    //大砲の移動処理
    private void CannonMoveOperation()
    {
        if (m_inputData.WasPressedButton(PlayerInputData.ActionsName.RightRail, m_inputData.SelfNumber)
         && m_moveMax > m_cannonPosition)
        {
            m_cannonPosition += CannonSpeedPercent(m_cannonData.Params.MoveSpeed);
        }
        if (m_inputData.WasPressedButton(PlayerInputData.ActionsName.LeftRail, m_inputData.SelfNumber)
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
        if(m_inputData.WasPressedButton(PlayerInputData.ActionsName.RightRail, m_inputData.SelfNumber)
          || m_inputData.WasPressedButton(PlayerInputData.ActionsName.LeftRail, m_inputData.SelfNumber))
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
}
