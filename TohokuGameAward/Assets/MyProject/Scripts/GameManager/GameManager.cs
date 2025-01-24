using UnityEngine;
//using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

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

        //if (m_playerManager.GetOnlyOnePlayer())
        //{
        //    ActiveGameSetText();
        //}
    }

    private void OnPlayerIFOutOfStage()
    {
        for (int i = 0; i < m_playerManager.Instances.Length; i++)
        {
            //if (m_playerManager.IsDead[i])
            //{
            //    continue;
            //}

            if (IsPlayerOut(m_playerManager.Instances[i], i))
            {
                OutOfStage(i);
                
            }
        }
    }
    private void OutOfStage(int playerNum)
    {
        //if (m_pointManager.DeadPointInterVal[playerNum] < 0)
        //{
        //    m_pointManager.IsDeadPoint[playerNum] = true;
        //    m_pointManager.DeadPointInterVal[playerNum] = m_pointData.Params.DeadPointInterval;
        //}
        
        //m_playerManager.SwitchDeadFlug(playerNum, true);
        //m_playerManager.DisablePhysics(playerNum);
        m_effectManager.OnPlayStageOutEffect(m_playerManager.Instances[playerNum].transform.position, EffectManager.EffectType.StageOut);
        m_soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.StageOut);
        m_playerManager.Instances[playerNum].transform.position = m_penartyPos;
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

    //private void ActiveGameSetText()
    //{
    //    if (m_gameSetText.gameObject.activeSelf)
    //    {
    //        return;
    //    }

    //    foreach (GameObject player in m_playerManager.PlayerCount)
    //    {
    //        if (player != null)
    //        {
    //            string winnerName = player.name;
    //            m_gameSetText.text = winnerName + m_winText;
    //            m_gameSetText.gameObject.SetActive(true);
    //            return;
    //        }
    //    }
    //}

    /// <summary>
    /// デバッグ用　スペースボタンでデストロイ
    /// </summary>
}