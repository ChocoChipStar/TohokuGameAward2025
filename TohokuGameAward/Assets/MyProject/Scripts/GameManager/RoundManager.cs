using System.Collections;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private GameTimer m_timer = null;

    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private BombDestroyer m_bombDestroyer = null;

    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private FadeManager m_fadeManager = null;

    [SerializeField]
    private ControllerUI m_controllerUI = null;

    [SerializeField]
    private float m_showTime = 0.0f;

    [SerializeField]
    private float m_showStartTime = 0.0f;

    [SerializeField]
    private float m_bombDestroyTime = 0.0f;

    private bool m_isStart = false;
    private bool m_isShuffle = false;
    private bool m_isTeamSet = false;
    private bool m_isFinish = false;
    private bool m_isRoundStart = false;

    public bool IsRoundStart { get { return m_isRoundStart; } }

    public static int CurrentRound { get; private set; }

    public enum RoundState
    {
        One,
        Two,
        Max
    }

    private void Awake()
    {
        m_controllerUI.ChangeAllActiveUI();
        m_controllerUI.ChangeAllPlayerIcon(false);

        m_controllerUI.MoveRoundUI(CurrentRound);

        Invoke("WaitOperation", m_showTime);
    }

    private void Update()
    {
        if (m_fadeManager.IsFinishFadeIn && m_isStart)
        {
            m_controllerUI.MoveToTeamSelectionUI();
            if (m_controllerUI.IsMoveDone)
            {
                m_isStart = false;
                StartCoroutine(RoundStart());
            }
        }

        if (m_isShuffle)
        {
            m_controllerUI.IconShuffler();
        }

        if (m_isTeamSet)
        {
            m_controllerUI.TeamSet();
            if (m_controllerUI.IsTeamSet)
            {
                m_isTeamSet = false;
            }
        }

        if(m_timer.IsTimeLimit && !m_isFinish)
        {
            StartCoroutine(RoundFinish());
        }
    }    

    private void WaitOperation()
    {
        m_isStart = true;        
    }

    /// <summary>
    /// ゲームが開始されるまでのUIをコントロールする処理です。
    /// </summary>
    private IEnumerator RoundStart()
    {
        m_controllerUI.DrawPlayerIcon(true);
        if (CurrentRound == (int)RoundState.One)
        {
            m_isShuffle = true;
            yield return new WaitForSeconds(m_showTime);
            m_isShuffle = false;
        }
        m_isTeamSet = true;

        yield return new WaitForSeconds(m_showTime);
        m_controllerUI.DrawPlayerIcon(false);
        m_controllerUI.ChangeAllActiveUI();
        m_controllerUI.ChangeUI((int)ControllerUI.RoundUI.Ready, true);
        
        yield return new WaitForSeconds(m_showTime);
        m_controllerUI.ChangeUI((int)ControllerUI.RoundUI.Ready, false);
        m_controllerUI.ChangeUI((int)ControllerUI.RoundUI.Start, true);
        m_playerManager.SetMovement(true);

        yield return new WaitForSeconds(m_showStartTime);
        m_controllerUI.ChangeUI((int)ControllerUI.RoundUI.Start, false);

        m_isRoundStart = true;
    }

    /// <summary>
    /// タイマーが0になった後のUI、ラウンド切り替えの処理です。
    /// </summary>
    private IEnumerator RoundFinish()
    {
        m_controllerUI.ChangeUI((int)ControllerUI.RoundUI.GameSet, true);
        m_isFinish = true;
        m_playerManager.SetMovement( false);
        m_controllerUI.ActiveFinishEffect(true);

        yield return new WaitForSeconds(m_bombDestroyTime);
        m_bombDestroyer.DestoroyBomb();

        yield return new WaitForSeconds(m_showTime);

        if (CurrentRound == 0)
        {
            SwitchNextRound();
        }
        else
        {
            SwitchResultScene();
        }
    }

    private void SwitchNextRound()
    {
        CurrentRound++;
        m_sceneChanger.TransitionSpecifiedScene(SceneChanger.SceneName.Main);
    }

    private void SwitchResultScene()
    {
        CurrentRound = 0;
        m_sceneChanger.TransitionNextScene();
    }
}