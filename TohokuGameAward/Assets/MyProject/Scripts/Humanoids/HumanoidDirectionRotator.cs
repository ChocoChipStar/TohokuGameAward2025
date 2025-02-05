using UniRx;
using UnityEngine;

public class HumanoidDirectionRotator : MonoBehaviour
{
    [SerializeField]
    private Transform m_modelTransform = null;

    [SerializeField]
    private HumanoidAnimator m_animator = null;

    [SerializeField]
    private HumanoidData m_humanoidData = null;

    private float m_lastFramePosX = 0.0f;

    private const float DiffDetectionRange = 0.01f;

    public ReactiveProperty<bool> IsRight { get; private set; } = new ReactiveProperty<bool>(false);

    private void Start()
    {
        IsRight.Subscribe(msg => SetBodyAngle()).AddTo(this);
        IsRight.Subscribe(msg => m_animator.SwitchMirroring()).AddTo(this);
    }

    private void Update()
    {
        SerachDirection();
    }

    /// <summary>
    /// 現在の向きを調べます
    /// </summary>
    private void SerachDirection()
    {
        var diffValue = this.transform.position.x - m_lastFramePosX;
        if (diffValue >= -DiffDetectionRange && diffValue <= DiffDetectionRange)
        {
            m_lastFramePosX = this.transform.position.x;
            return;
        }

        if (diffValue > DiffDetectionRange)
        {
            IsRight.Value = true;
        }

        if (diffValue < DiffDetectionRange)
        {
            IsRight.Value = false;
        }

        m_lastFramePosX = this.transform.position.x;
        return;
    }

    /// <summary>
    /// プレイヤーの動きに合わせて体の角度を変更します
    /// </summary>
    private void SetBodyAngle()
    {
        if(IsRight.Value)
        {
            m_modelTransform.rotation = Quaternion.Euler(0.0f, m_humanoidData.Params.RightBodyAngle, 0.0f);
            return;
        }
        m_modelTransform.rotation = Quaternion.Euler(0.0f, m_humanoidData.Params.LeftBodyAngle, 0.0f);
    }
}
