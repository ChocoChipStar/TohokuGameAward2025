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
            GivePoint(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// otherObjにm_scoreと同じ値のポイントを与えます。
    /// </summary>
    /// <param name="other"></param>
    private void GivePoint(GameObject otherObj)
    {
        InputData inputData = otherObj.transform.parent.gameObject.GetComponent<InputData>();
        PointManager pointManager = this.transform.parent.GetComponent<PointManager>();

        int playerIndex = inputData.SelfNumber;
        pointManager.AddScore(playerIndex, m_score);
    }
}
