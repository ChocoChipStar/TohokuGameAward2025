using UnityEngine;
using UnityEngine.UI;
using static RoundManager;

public class ControllerUI : MonoBehaviour
{
    [SerializeField]
    private Image[] m_playerIconImage = new Image[4];

    [SerializeField]
    private Sprite[] m_playerNumberImage = new Sprite[4];

    [SerializeField]
    private GameObject[] m_roundUI = new GameObject[5];

    [SerializeField]
    private GameObject[] m_finishEffectObject = new GameObject[5];

    [SerializeField]
    private float m_roundUISpeed = 0.0f;

    private Vector3 m_roundUIposition = Vector3.zero;
    private Vector3 m_originPosition = new Vector3(960.0f, 540.0f, 0.0f);

    private bool m_isMoveDone = false;
    private bool m_isTeamSet = false;

    public bool IsMoveDone { get { return m_isMoveDone; } }
    public bool IsTeamSet { get { return m_isTeamSet; }  }

    public enum RoundUI
    {
        TeamSelectionRoundOne,
        TeamSelectionRoundTwo,
        Ready,
        Start,
        GameSet
    }

    public void ChangeUI(int num, bool isActive)
    {
        m_roundUI[num].SetActive(isActive);
        if(isActive)
        {
            m_roundUI[num].transform.position = m_originPosition;
        }
    }

    public void ChangeAllActiveUI()
    {
        for (int i = 0; i < m_roundUI.Length; i++)
        {
            m_roundUI[i].SetActive(false);
        }
    }

    public void ChangeAllPlayerIcon(bool isActive)
    {
        for(int i = 0; i < m_playerIconImage.Length; i++)
        {
            m_playerIconImage[i].enabled = isActive;
        }
    }

    public void ActiveFinishEffect(bool isActive)
    {
        for (int i = 0; i < m_finishEffectObject.Length; i++)
        {
            m_finishEffectObject[i].SetActive(isActive);
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
        var speed = (this.transform.position.x - tarms.x) * m_roundUISpeed;
        tarms.x = speed;
        return tarms;
    }

    public void MoveToTeamSelectionUI()
    {
        var newPos = Vector3.zero;
        var currentPos = Vector3.zero;
        NewMethod(newPos, currentPos);
    }

    private void NewMethod(Vector3 newPos, Vector3 currentPos)
    {
        m_roundUI[CurrentRound].gameObject.SetActive(true);
        currentPos = m_roundUI[(int)RoundUI.TeamSelectionRoundTwo].gameObject.GetComponent<RectTransform>().position;
        newPos = Vector3.MoveTowards(currentPos, m_originPosition, m_roundUIposition.x);
        m_roundUI[CurrentRound].gameObject.transform.position = newPos;
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