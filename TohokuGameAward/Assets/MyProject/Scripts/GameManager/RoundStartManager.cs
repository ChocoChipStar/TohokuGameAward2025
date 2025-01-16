using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoundStartManager : MonoBehaviour
{
    [SerializeField]
    private FadeManager m_fadeManager = null;

    [SerializeField]
    private MonoBehaviour[] m_gameStartScript = null;

    [SerializeField]
    private GameObject[] m_gameStartObject = null;

    [SerializeField]
    private MonoBehaviour[] m_playerObject = null;

    [SerializeField]
    private GameObject[] m_playerIconObject = null;

    [SerializeField]
    private Image[] m_playerIconImage = null;

    [SerializeField]
    private Sprite[] m_playerIcon = null;

    [SerializeField]
    private GameObject[] m_roundUI = null;

    [SerializeField]
    private float m_UISpeed = 0.0f;

    [SerializeField]
    private float m_showTime = 0.0f;

    private Vector3 m_UIPosition = new Vector3(-1920.0f, 540.0f, 0.0f);
    private Vector3 m_originPosition = new Vector3(960.0f, 540.0f, 0.0f);
    private Vector3 m_UItransform = Vector3.zero;

    private int m_playerCount = 2;
    private int m_cannonCount = 0;

    private bool[] m_isCannonObject = new bool[4];

    private bool m_isStart = false;
    private bool m_isPlayerRand = false;
    private bool m_isTeamSet = false;

    private enum RoundUI
    {
        PlayerTeam1,
        PlayerTeam2,
        ready,
        Start,
        GameSet
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
            m_roundUI[i].transform.position = m_UIPosition;
        }
        for (int i = 0; i < m_playerIconObject.Length; i++)
        {
            m_playerIconObject[i].SetActive(false);
        }
        m_isStart = false;
        m_isPlayerRand = false;
        m_UItransform = UIMoveSpeed(m_roundUI[0].transform.position);
        StartCoroutine(WaitOperation());
    }

    private void Update()
    {
        if (m_fadeManager.IsFinishFadeIn && m_isStart)
        {
            m_roundUI[(int)RoundUI.PlayerTeam1].gameObject.SetActive(true);
            RoundFadeInUI(m_roundUI[(int)RoundUI.PlayerTeam1].gameObject.transform.position);
        }

        if (m_isPlayerRand)
        {
            PlayerRand();
        }

        if (m_isTeamSet)
        {
            TeamSet();
        }
    }

    private void RoundFadeInUI(Vector3 UI)
    {
        m_roundUI[(int)RoundUI.PlayerTeam1].gameObject.transform.position= Vector3.MoveTowards(
          UI, m_originPosition, m_UItransform.x);
    }

    public void PlayerSet(GameObject player, bool isCannons, int Num)
    {
        m_isCannonObject[Num] = isCannons;
        if (isCannons)
        {
            m_playerObject[Num] = player.GetComponent<CannonMover>();
        }
        else
        {
            m_playerObject[Num] = player.GetComponent<PlayerMover>();
        }
        m_playerObject[Num].enabled = false;
    }

    private void PlayerRand()
    {
        for(int i = 0; i < m_playerObject.Length; i++)
        {
            var randum = Random.Range(0, m_playerObject.Length);
            m_playerIconImage[i].sprite = m_playerIcon[randum];
        }
    }

    private void TeamSet()
    {
        for (int i = 0; i < m_playerObject.Length; i++)
        {
            if (m_playerObject[i] == null)
            {
                break;
            }
            if (m_isCannonObject[i])
            {
                m_playerIconImage[m_cannonCount].sprite = m_playerIcon[i];
                m_cannonCount++;
            }
            else
            {
                m_playerIconImage[m_playerCount].sprite = m_playerIcon[i];
                m_playerCount++;
            }
        }
        m_isTeamSet = false;
    }

    private Vector3 UIMoveSpeed(Vector3 tarms)
    {
        var speed = Time.deltaTime * ((this.transform.position.x - tarms.x) / m_UISpeed);
        tarms.x = speed;
        return tarms;
    }

    IEnumerator WaitOperation()
    {
        yield return new WaitForSeconds(m_showTime);
        m_isStart = true;

        yield return new WaitForSeconds(m_showTime);
        m_isStart = false;
        m_isPlayerRand = true;
        for(int i = 0; i < m_playerIconObject.Length; i++)
        {
            m_playerIconObject[i].SetActive(true);
        }

        yield return new WaitForSeconds(m_showTime);
        m_isPlayerRand = false;
        m_isTeamSet = true;

        yield return new WaitForSeconds(m_showTime);
        for (int i = 0; i < m_playerIconObject.Length; i++)
        {
            m_playerIconObject[i].SetActive(false);
        }
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

        yield return new WaitForSeconds(1);
        m_roundUI[(int)RoundUI.Start].SetActive(false);

        for (int i = 0; i < m_gameStartScript.Length; i++)
        {
            m_gameStartScript[i].enabled = true;
        }
        for (int i = 0; i < m_gameStartObject.Length; i++)
        {
            m_gameStartObject[i].SetActive(true);
        }
        for (int i = 0; i < m_playerObject.Length; i++)
        {
            if (m_playerObject[i] == null)
                break;
            m_playerObject[i].enabled=true;
        }
    }
}
