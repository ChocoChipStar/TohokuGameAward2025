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
        for (int i = 0; i < m_playerManager.Instances.Length; i++)
        {
            if (m_humanoidRespawn.IsDead[i])
            {
                continue;
            }

            if (IsOverBorderLine(i))
            {
                OutOfStage(i);
            }
        }
    }

    /// <summary>
    /// プレイヤーがボーダーラインを越えているかを調べます
    /// </summary>
    /// <retuns> true->ボーダーラインを超えている false->超えていない </retuns>
    private bool IsOverBorderLine(int index)
    {
        var targetObj = m_playerManager.Instances[index];
        if (targetObj == null || TagManager.Instance.SearchedTagName(targetObj,TagManager.Type.Cannon))
        {
            return false;
        }

        var isTargetOverLeftBorder = targetObj.transform.position.x < m_stageData.Border.Left;
        var isTargetOverRightBorder = targetObj.transform.position.x > m_stageData.Border.Right;
        if (isTargetOverLeftBorder || isTargetOverRightBorder)
        { 
            // 両サイドのボーダーラインを越えていたら
            return true; 
        }

        var isTargetOverBottomBorder = targetObj.transform.position.y < m_stageData.Border.Bottom;
        var isTargetOverTopBorder = targetObj.transform.position.y > m_stageData.Border.Top;
        if(isTargetOverBottomBorder || isTargetOverTopBorder)
        {
            // 上下のボーダーラインを越えていたら
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