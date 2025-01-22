using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    private MonoBehaviour[] m_gameStartScript = null;

    [SerializeField]
    private GameObject[] m_gameStartObject = null;

    [SerializeField]
    private MonoBehaviour[] m_playerObject = null;

    [SerializeField]
    private Image[] m_playerIconImage = null;

    [SerializeField]
    private Sprite[] m_playerNumberImage = null;

    [SerializeField]
    private GameObject[] m_roundUI = null;

    [SerializeField]
    private GameObject[] m_finishUI = null;

    [SerializeField]
    private float m_UISpeed = 0.0f;

    [SerializeField]
    private float m_showTime = 0.0f;

    private Vector3 m_UIPosition = new Vector3(-1920.0f, 0.0f, 0.0f);
    private Vector3 m_originPosition = new Vector3(960.0f, 540.0f, 0.0f);
    private Vector3 m_UItransform = Vector3.zero;

    private int m_playerCount = 2;
    private int m_cannonCount = 0;

    private bool[] m_isCannonObject = new bool[4];

    private bool m_isStart = false;
    private bool m_isShuffle = false;
    private bool m_isTeamSet = false;
    private bool m_isFinish = false;

    private enum RoundUI
    {
        TeamSelectionRoundOne,
        TeamSelectionRoundTwo,
        ready,
        Start,
        GameSet
    }

    public static int CurrentRound { get; private set; }

    public enum RoundState
    {
        One,
        Two,
        Max
    }

    private void Awake()
    {
        for (int i = 0; i < m_gameStartScript.Length; i++)
        {
            m_gameStartScript[i].enabled = false;
        }
        for (int i = 0; i < m_gameStartObject.Length; i++)
        {
            m_gameStartObject[i].SetActive(false);
        }
        for (int i = 0; i < m_roundUI.Length; i++)
        {
            m_roundUI[i].SetActive(false);
        }
        for (int i = 0; i < m_playerIconImage.Length; i++)
        {
            m_playerIconImage[i].enabled = false;
        }
        m_isStart = false;
        m_isShuffle = false;
        if(CurrentRound == (int)RoundState.One)
        {
            m_UItransform = UIMoveSpeed(m_roundUI[(int)RoundState.One].transform.position);
        }
        else
        {
            m_UItransform = UIMoveSpeed(m_roundUI[(int)RoundState.Two].transform.position);
        }

        StartCoroutine(WaitOperation());
    }

    private void Update()
    {
        if (m_fadeManager.IsFinishFadeIn && m_isStart)
        {
            
            MoveToTeamSelectionUI(m_originPosition,50.0f);
        }

        if (m_isShuffle)
        {
            IconShuffler();
        }

        if (m_isTeamSet)
        {
            TeamSet();
        }

        if(m_timer.IsTimeLimit && !m_isFinish)
        {
            StartCoroutine(RoundFinish());
        }
    }

    private void MoveToTeamSelectionUI(Vector3 targetPos, float speed)
    {
        var newPos = Vector3.zero;
        var currentPos = Vector3.zero;
        switch (CurrentRound)
        {
            case (int)RoundState.One:
                m_roundUI[(int)RoundUI.TeamSelectionRoundOne].gameObject.SetActive(true);
                currentPos = m_roundUI[(int)RoundUI.TeamSelectionRoundOne].gameObject.GetComponent<RectTransform>().position;
                newPos = Vector3.MoveTowards(currentPos, targetPos, m_UItransform.x);
                m_roundUI[(int)RoundUI.TeamSelectionRoundOne].gameObject.transform.position = newPos;
                break;

            case (int)RoundState.Two:
                m_roundUI[(int)RoundUI.TeamSelectionRoundTwo].gameObject.SetActive(true);
                currentPos = m_roundUI[(int)RoundUI.TeamSelectionRoundTwo].gameObject.GetComponent<RectTransform>().position;
                newPos = Vector3.MoveTowards(currentPos, targetPos, m_UItransform.x);
                m_roundUI[(int)RoundUI.TeamSelectionRoundTwo].gameObject.transform.position = newPos;
                break;
        }

        if(Vector3.Distance(currentPos,targetPos) <= 0.3f)
        {
            m_isStart = false;
            StartCoroutine(RoundStart());
        }
    }

    /// <summary>
    /// プレイヤーの顔アイコンをシャッフルする処理を行います
    /// </summary>
    private void IconShuffler()
    {
        for(int i = 0; i < m_playerObject.Length; i++)
        {
            var randomValue = Random.Range(0, InputData.PlayerMax);
            m_playerIconImage[i].sprite = m_playerNumberImage[randomValue];
        }
    }

    private void TeamSet()
    {
        for (int i = 0; i < PlayerManager.AlphaTeamNumber.Count; i++)
        {
            var alphaNum = PlayerManager.AlphaTeamNumber[i];
            m_playerIconImage[i].sprite = m_playerNumberImage[alphaNum];
        }

        for (int i = 0; i < PlayerManager.BravoTeamNumber.Count; i++)
        {
            var bravoNum = PlayerManager.BravoTeamNumber[i];
            m_playerIconImage[i + 2].sprite = m_playerNumberImage[bravoNum];
        }

        m_isTeamSet = false;
    }

    private Vector3 UIMoveSpeed(Vector3 tarms)
    {
        var speed = (this.transform.position.x - tarms.x) * m_UISpeed;
        tarms.x = speed;
        return tarms;
    }

    IEnumerator WaitOperation()
    {
        yield return new WaitForSeconds(m_showTime);
        m_isStart = true;        
    }

    private IEnumerator RoundStart()
    {
        DrawPlayerIcon(true);
        if (CurrentRound == (int)RoundState.One)
        {
            m_isShuffle = true;
            yield return new WaitForSeconds(m_showTime);
            m_isShuffle = false;
        }

        m_isTeamSet = true;
        yield return new WaitForSeconds(m_showTime);
        DrawPlayerIcon(false);

        for (int i = 0; i < m_roundUI.Length; i++)
        {
            m_roundUI[i].SetActive(false);
        }
        m_roundUI[(int)RoundUI.ready].SetActive(true);
        m_roundUI[(int)RoundUI.ready].transform.position = m_originPosition;

        yield return new WaitForSeconds(m_showTime);
        m_roundUI[(int)RoundUI.ready].SetActive(false);
        m_roundUI[(int)RoundUI.Start].SetActive(true);
        m_roundUI[(int)RoundUI.Start].transform.position = m_originPosition;

        for(int i = 0; i < InputData.PlayerMax; i++)
        {
            m_playerManager.SetMovement(i, true);
        }

        yield return new WaitForSeconds(1);
        m_roundUI[(int)RoundUI.Start].SetActive(false);

        InitializeGameStart();

        for (int i = 0; i < m_playerObject.Length; i++)
        {
            if (m_playerObject[i] == null)
                break;
            m_playerObject[i].enabled = true;
        }
    }

    private void DrawPlayerIcon(bool isActive)
    {
        
        for (int i = 0; i < m_playerIconImage.Length; i++)
        {
            m_playerIconImage[i].enabled = isActive;
        }
    }

    private IEnumerator RoundFinish()
    {
        m_roundUI[(int)RoundUI.GameSet].SetActive(true);
        m_roundUI[(int)RoundUI.GameSet].transform.position = m_originPosition;
        m_isFinish = true;

        for (int i = 0; i < InputData.PlayerMax; i++)
        {
            m_playerManager.SetMovement(i, false);
        }

        for(int i = 0; i < m_finishUI.Length; i++)
        {
            m_finishUI[i].SetActive(true);
        }

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

    /// <summary>
    /// ゲームスタート時の初期化を行います
    /// </summary>
    private void InitializeGameStart()
    {
        for (int i = 0; i < m_gameStartScript.Length; i++)
        {
            m_gameStartScript[i].enabled = true;
        }

        for(int i = 0; i < m_gameStartObject.Length; i++)
        {
            m_gameStartObject[i].SetActive(true);
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
