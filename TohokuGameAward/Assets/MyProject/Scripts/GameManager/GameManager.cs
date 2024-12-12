using UnityEngine;
//using TMPro;

public class GameManager : MonoBehaviour
{
    //[SerializeField]
    //private TextMeshProUGUI m_gameSetText = null;

    //[SerializeField]
    //private string m_winText = null;

    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private StageData m_stageData = null;

    private void Update()
    {
        DestroyPlayerIFOutOfStage();

        //if (m_playerManager.GetOnlyOnePlayer())
        //{
        //    ActiveGameSetText();
        //}

        //デバッグ用 スペースボタンでデストロイ
        DestroyPlayers();
    }

    private void DestroyPlayerIFOutOfStage()
    {
        for (int i = 0; i < m_playerManager.PlayerCount.Length; i++)
        {
            if (IsPlayerOut(m_playerManager.PlayerCount[i]))
            {
                m_playerManager.SwitchDeadFlug(i,true); 
                Destroy(m_playerManager.PlayerCount[i]);
                m_playerManager.PlayerCount[i] = null;
            }
        }
    }

    private bool IsPlayerOut(GameObject targetPlayer)
    {
        GameObject target = targetPlayer;

        if(targetPlayer == null)
        {
            return false;
        }

        if (target.transform.position.x < m_stageData.Size.LeftLimit ||
           target.transform.position.x > m_stageData.Size.RightLimit ||
           target.transform.position.y < m_stageData.Size.Bottom)
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
    private void DestroyPlayers()
    {
        string tagName = "Player";
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(GameObject.FindWithTag(tagName));
        }
    }
}