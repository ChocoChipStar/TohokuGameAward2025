using UnityEngine;

public class ShotPointManager : MonoBehaviour
{
    [SerializeField]
    private PointData m_pointData = null;

    [SerializeField]
    private GameObject m_pointManager = null;

    private PointManager m_pointManagerScript  = null;

    private void Start()
    {
        m_pointManagerScript = m_pointManager.gameObject.GetComponent<PointManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        //爆弾用
        if ((!TagManager.Instance.SearchedTagName(other.transform.gameObject, TagManager.Type.Player))
             && !TagManager.Instance.SearchedTagName(other.transform.parent.gameObject, TagManager.Type.Player))
        {
            return;
        }
        if (TagManager.Instance.SearchedTagName(other.transform.gameObject, TagManager.Type.Player))
        {
            PlayerManager playerManager = other.gameObject.GetComponentInParent<PlayerManager>();
            

            int playerIndex = playerManager.GetPlayerIndex(other.transform.gameObject);
            m_pointManagerScript.DecreaseScore(playerIndex, m_pointData.Params.PenaltyPoint);
            m_pointManagerScript.AddCannonPoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //爆風用
        if ((!TagManager.Instance.SearchedTagName(other.transform.gameObject, TagManager.Type.Player))
             && !TagManager.Instance.SearchedTagName(other.transform.parent.gameObject, TagManager.Type.Player))
        {
            return;
        }

        if (TagManager.Instance.SearchedTagName(other.transform.parent.gameObject, TagManager.Type.Player))
        {
            PlayerManager playerManager = other.gameObject.GetComponentInParent<PlayerManager>();

            int playerIndex = playerManager.GetPlayerIndex(other.transform.parent.gameObject);
            m_pointManagerScript.DecreaseScore(playerIndex, m_pointData.Params.PenaltyPoint);
            m_pointManagerScript.AddCannonPoint();
        }
    }
}
