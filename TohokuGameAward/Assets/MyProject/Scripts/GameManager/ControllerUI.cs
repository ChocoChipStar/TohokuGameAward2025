using UnityEngine;
using UnityEngine.UI;
using static RoundManager;

public class ControllerUI : MonoBehaviour
{
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

    private Vector3 m_roundUIposition = Vector3.zero;
    private Vector3 m_originPosition = new Vector3(960.0f, 540.0f, 0.0f);

    private bool m_isMoveDone = false;
    private bool m_isTeamSet = false;

    public bool IsMoveDone { get { return m_isMoveDone; } private set { } }
    public bool IsTeamSet { get { return m_isTeamSet; } private set { } }

    public enum RoundUI
    {
        TeamSelectionRoundOne,
        TeamSelectionRoundTwo,
        ready,
        Start,
        GameSet
    }

    public void ChangeUI(int num, bool isActive)
    {
        m_roundUI[num].SetActive(isActive);
        if(isActive == true)
        {
            m_roundUI[num].transform.position = m_originPosition;
        }
    }

    public void ChangeAllActiveUI(bool isActive)
    {
        for (int i = 0; i < m_roundUI.Length; i++)
        {
            m_roundUI[i].SetActive(isActive);
        }
    }

    public void ChangeAllPlayer(bool isActive)
    {
        for(int i = 0; i < m_playerIconImage.Length; i++)
        {
            m_playerIconImage[i].enabled = isActive;
        }
    }

    public void ActiveFinishEffect(bool isActive)
    {
        for (int i = 0; i < m_finishUI.Length; i++)
        {
            m_finishUI[i].SetActive(isActive);
        }
    }

    public void MoveRoundUI(int round)
    {
        m_roundUIposition = UIMoveSpeed(m_roundUI[round].transform.position);
    }

    public void DrawPlayerIcon(bool isActive)
    {
        for (int i = 0; i < m_playerIconImage.Length; i++)
        {
            m_playerIconImage[i].enabled = isActive;
        }
    }

    private Vector3 UIMoveSpeed(Vector3 tarms)
    {
        var speed = (this.transform.position.x - tarms.x) * m_UISpeed;
        tarms.x = speed;
        return tarms;
    }

    public void MoveToTeamSelectionUI(float speed)
    {
        var newPos = Vector3.zero;
        var currentPos = Vector3.zero;
        switch (CurrentRound)
        {
            case (int)RoundState.One:
                m_roundUI[(int)RoundUI.TeamSelectionRoundOne].gameObject.SetActive(true);
                currentPos = m_roundUI[(int)RoundUI.TeamSelectionRoundOne].gameObject.GetComponent<RectTransform>().position;
                newPos = Vector3.MoveTowards(currentPos, m_originPosition, m_roundUIposition.x);
                m_roundUI[(int)RoundUI.TeamSelectionRoundOne].gameObject.transform.position = newPos;
                break;

            case (int)RoundState.Two:
                m_roundUI[(int)RoundUI.TeamSelectionRoundTwo].gameObject.SetActive(true);
                currentPos = m_roundUI[(int)RoundUI.TeamSelectionRoundTwo].gameObject.GetComponent<RectTransform>().position;
                newPos = Vector3.MoveTowards(currentPos, m_originPosition, m_roundUIposition.x);
                m_roundUI[(int)RoundUI.TeamSelectionRoundTwo].gameObject.transform.position = newPos;
                break;
        }

        if (Vector3.Distance(currentPos, m_originPosition) <= 0.3f)
        {
            m_isMoveDone = true;
        }
    }

    /// <summary>
    /// プレイヤーの顔アイコンをシャッフルする処理を行います
    /// </summary>
    public void IconShuffler()
    {
        for (int i = 0; i < m_playerIconImage.Length; i++)
        {
            var randomValue = Random.Range(0, InputData.PlayerMax);
            m_playerIconImage[i].sprite = m_playerNumberImage[randomValue];
        }
    }

    /// <summary>
    /// 確定したチームにあわせてアイコンをセットします
    /// </summary>
    public void TeamSet()
    {
        for (int i = 0; i < PlayerTeamGenerator.AlphaTeamNumber.Count; i++)
        {
            var alphaNum = PlayerTeamGenerator.AlphaTeamNumber[i];
            m_playerIconImage[i].sprite = m_playerNumberImage[alphaNum];
        }

        for (int i = 0; i < PlayerTeamGenerator.BravoTeamNumber.Count; i++)
        {
            var bravoNum = PlayerTeamGenerator.BravoTeamNumber[i];
            m_playerIconImage[i + 2].sprite = m_playerNumberImage[bravoNum];
        }

        m_isTeamSet = true;
    }
}