using System.Collections;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private GameTimer m_timer = null;

    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private SceneChanger m_sceneChanger = null;

    [SerializeField]
    private FadeManager m_fadeManager = null;

    [SerializeField]
    private UIController m_controllerUI = null;

    [SerializeField]
    private GameObject[] m_gameStartObject = null;

    [SerializeField]
    private float m_UISpeed = 0.0f;

    [SerializeField]
    private float m_showTime = 0.0f;

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
        InitializeGameStart(false);
        m_controllerUI.ChangeAllActiveUI(false);
        m_controllerUI.ChangeAllPlayer(false);

        m_isStart = false;
        m_isShuffle = false;

        m_controllerUI.MoveRoundUI(CurrentRound);

        StartCoroutine(WaitOperation());
    }

    private void Update()
    {
        if (m_fadeManager.IsFinishFadeIn && m_isStart)
        {
            m_controllerUI.MoveToTeamSelectionUI(m_UISpeed);
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

    IEnumerator WaitOperation()
    {
        yield return new WaitForSeconds(m_showTime);
        m_isStart = true;        
    }

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
        m_controllerUI.ChangeAllActiveUI(false);
        m_controllerUI.ChangeUI((int)UIController.RoundUI.ready, true);
        
        yield return new WaitForSeconds(m_showTime);
        m_controllerUI.ChangeUI((int)UIController.RoundUI.ready, false);
        m_controllerUI.ChangeUI((int)UIController.RoundUI.Start, true);

        for(int i = 0; i < InputData.PlayerMax; i++)
        {
            m_playerManager.SetMovement(i, true);
        }

        yield return new WaitForSeconds(1);
        m_controllerUI.ChangeUI((int)UIController.RoundUI.Start, false);

        InitializeGameStart(true);
    }

    private IEnumerator RoundFinish()
    {
        m_controllerUI.ChangeUI((int)UIController.RoundUI.GameSet, true);
        m_isFinish = true;

        for (int i = 0; i < InputData.PlayerMax; i++)
        {
            m_playerManager.SetMovement(i, false);
        }

        m_controllerUI.ActiveFinishEffect(true);

        yield return new WaitForSeconds(0.5f);
        var bomb = GameObject.FindGameObjectsWithTag("Bomb");
        for(int i = 0; i < bomb.Length; i++)
        {
            Destroy(bomb[i]);
        }

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
    private void InitializeGameStart(bool isActive)
    {
        m_isRoundStart = isActive;
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