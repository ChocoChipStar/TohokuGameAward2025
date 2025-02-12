using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private RoundManager m_roundManager = null;

    [SerializeField]
    private TextMeshProUGUI m_timerText = null;

    [SerializeField]
    private float m_timelimit = 0.0f;

    private bool m_isCountDown = false;

    public ReactiveProperty<bool> IsTimeLimit { get; private set; } = new ReactiveProperty<bool>(false);

    private void Awake()
    {
        IsTimeLimit.Subscribe(msg => StartCoroutine(m_roundManager.InitializeRoundFinish())).AddTo(this);
    }

    private void Update()
    {
        if (IsTimeLimit.Value || !m_isCountDown)
        {
            return;
        }

        CountDown();
    }

    private void CountDown()
    {
        m_timelimit -= Time.deltaTime;
        if (m_timelimit <= 0)
        {
            m_timelimit = 0;
            IsTimeLimit.Value = true;
        }

        m_timerText.text = m_timelimit.ToString("F0");//整数で表示
    }

    public void StartCountDown()
    {
        m_isCountDown = true;
        m_timerText.enabled = true;
    }
}
