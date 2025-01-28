using UnityEngine;
//using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private HumanoidRespawn m_humanoidRespawn = null;

    [SerializeField]
    private PenartyPointOparator m_penartyOperator = null;

    [SerializeField]
    private PointManager m_pointManager = null;

    [SerializeField]
    private PointData m_pointData = null;

    [SerializeField]
    private StageData m_stageData = null;

    [SerializeField]
    private EffectManager m_effectManager = null;

    [SerializeField]
    private SoundEffectManager m_soundEffectManager = null;

    [SerializeField]
    private Vector3 m_penartyPos = Vector3.zero;

    private bool[] isDeadPoint = new bool[4];

    public bool[] IsDeadPoint {  get { return isDeadPoint; }set { isDeadPoint = value; } }

    private void Update()
    {
        OnPlayerIFOutOfStage();
    }

    private void OnPlayerIFOutOfStage()
    {
        for (int i = 0; i < InputData.PlayerMax; i++)
        {
            if (m_humanoidRespawn.IsDead[i])
            {
                continue;
            }

            if (IsPlayerOut(m_playerManager.Instances[i], i))
            {
                OutOfStage(i);
            }
        }
    }
    private bool IsPlayerOut(GameObject targetPlayer, int num)
    {
        GameObject target = targetPlayer;

        if(targetPlayer == null || TagManager.Instance.SearchedTagName(m_playerManager.Instances[num],TagManager.Type.Cannon))
        {
            return false;
        }

        if (target.transform.position.x < m_stageData.Size.LeftLimit ||
           target.transform.position.x > m_stageData.Size.RightLimit ||
           target.transform.position.y < m_stageData.Size.Bottom     ||
           target.transform.position.y > m_stageData.Size.UpLimit)
        { 
            //ステージ外に出ていればtrue
            return true; 
        }

        return false;
    }

    private void OutOfStage(int playerNum)
    {
        if (m_penartyOperator.DeadPenartyInterVal[playerNum] < 0)
        {
            InitPenalty(playerNum);
        }

        OutOfStageProsessing(playerNum);
    }

    /// <summary>
    /// ステージ外にHumanoidが出た時の演出やフラグに関する処理を行います。
    /// </summary>
    /// <param name="playerNum"></param>
    private void OutOfStageProsessing(int playerNum)
    {
        m_humanoidRespawn.SwitchDeadFlug(playerNum, true);
        m_playerManager.DisablePhysics(playerNum);
        m_effectManager.OnPlayEffect(m_playerManager.Instances[playerNum].transform.position, EffectManager.EffectType.StageOut);
        m_soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.StageOut);
        m_playerManager.Instances[playerNum].transform.position = m_penartyPos;
    }
    /// <summary>
    /// ステージ外にHumanoidが出た時のペナルティに関する処理を行います。
    /// </summary>
    /// <param name="playerNum"></param>
    void InitPenalty(int playerNum)
    {
        m_penartyOperator.SetPenaltyPoint(playerNum);
        m_penartyOperator.InitPenartyInterVal(playerNum);
    }
}