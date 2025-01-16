using UnityEngine;

public class PointItemManager : MonoBehaviour
{
    [SerializeField]
    private int m_score = 0;

    private bool m_isGot = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null)
        {
            return; 
        }

        if (TagManager.Instance.SearchedTagName(other.transform.parent.gameObject, TagManager.Type.Player) && !m_isGot)
        {
            PlayerManager playerManager = other.gameObject.GetComponentInParent<PlayerManager>();
            PointManager pointManager = this.transform.parent.GetComponent<PointManager>();

            m_isGot = true;
            int playerIndex = playerManager.GetPlayerIndex(other.transform.parent.gameObject);
            pointManager.AddScore(playerIndex, m_score); 
            Destroy(this.gameObject);
        }
    }
}
