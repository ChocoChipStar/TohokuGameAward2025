using UnityEngine;
//using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private StageData m_stageData = null;

    [SerializeField]
    private EffectManager m_effectManager = null;

    [SerializeField]
    private SoundEffectManager m_soundEffectManager = null;

    [SerializeField]
    private Vector3 m_penartyPos = Vector3.zero;

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
        for (int i = 0; i < m_playerManager.PlayerCount.Length; i++)
        {
            if (m_playerManager.IsDead[i])
            {
                continue;
            }

            if (IsPlayerOut(m_playerManager.PlayerCount[i], i))
            {
                OutOfStage(i);
            }
        }
    }
    private void OutOfStage(int playerNum)
    {
        m_playerManager.SwitchDeadFlug(playerNum, true);
        m_playerManager.DisablePhysics(playerNum);
        m_effectManager.OnPlayStageOutEffect(m_playerManager.PlayerCount[playerNum].transform.position, EffectManager.EffectType.StageOut);
        m_soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.StageOut);
        m_playerManager.PlayerCount[playerNum].transform.position = m_penartyPos;
    }

    private bool IsPlayerOut(GameObject targetPlayer, int num)
    {
        GameObject target = targetPlayer;

        if(targetPlayer == null || m_playerManager.IsCannon[num] == true)
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