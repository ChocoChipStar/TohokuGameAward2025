using Unity.VisualScripting;
using UnityEngine;

public class PlayerProduceBomb : MonoBehaviour
{
    [SerializeField]
    private PlayerInputData m_playerInputDeta = null;

    [SerializeField]
    private PlayerData m_playerDeta = null;

    [SerializeField]
    private PlayerMover m_playerMover = null;

    [SerializeField]
    private PlayerPickup m_playerPickup = null;

    [SerializeField]
    private BlowMover m_blowMover = null;

    [SerializeField]
    private GameObject m_bombObject = null;

    [SerializeField]
    private Vector3 m_generatePosition = Vector3.zero;

    [SerializeField, Header("一時出来に追加")]
    private Canvas m_canvas = null;

    private float m_generateTime = 0.0f;

    private bool m_isGenerating = false;

    public bool isGenerating { get { return m_isGenerating;} }

    private void Update()
    {
        if (CanGenerateBomb())
        {
            GenerateBomb();
        }
        else
        {
            m_canvas.enabled = false;   //デバッグ用
            m_isGenerating = false;
            m_generateTime = 0.0f;
        }
    }

    /// <summary>
    /// ボムを生成するまでの時間を計測します。
    /// </summary>
    private void GenerateBomb()
    {
        m_canvas.enabled = true;    //デバッグ用
        m_isGenerating = true;
        m_generateTime += Time.deltaTime;
        if(m_generateTime >= m_playerDeta.Params.BombGenereareTime)
        {
            BringBomb();
        }
    }

    /// <summary>
    /// ボムを出現させ、プレイヤーに持たせます。
    /// </summary>
    private void BringBomb()
    {
        Vector3 playerPosition = this.transform.position + m_generatePosition;
        Instantiate(m_bombObject, playerPosition, Quaternion.identity);
        m_generateTime = 0.0f;
    }

    /// <summary>
    /// ボムが生成できるか調べます
    /// </summary>
    /// <returns>true->爆弾生成可能 </returns>false->不可能
    private bool CanGenerateBomb()
    {
        if(m_playerInputDeta.WasPressedButton(PlayerInputData.ActionsName.Produce, m_playerInputDeta.SelfNumber)
        && !m_playerPickup.IsPuckUp 
        && !m_blowMover.IsBlow 
        && m_playerMover.IsGrounded)
        {
            return true;
        }
        return false;
    }
}
