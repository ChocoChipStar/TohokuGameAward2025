using UnityEngine;

public class PointGiverObject : MonoBehaviour
{
    [SerializeField]
    private int m_score = 0;

    [SerializeField]
    private PlayerData m_playerData;

    private bool m_isGot = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null && m_isGot)
        {
            return; 
        }

        if (TagManager.Instance.SearchedTagName(other.transform.parent.gameObject, TagManager.Type.Player))
        {
            m_isGot = true;
            GivePoint(other);
            Destroy(this.gameObject);
        }
    }

    void GivePoint(Collider other)
    {
        PlayerManager playerManager = other.gameObject.GetComponentInParent<PlayerManager>();
        PointManager pointManager = this.transform.parent.GetComponent<PointManager>();

        int playerIndex = playerManager.GetPlayerIndex(other.transform.parent.gameObject);
        pointManager.AddScore(playerIndex, m_score);
    }
}
