using UnityEngine;

public class PointGiverObject : MonoBehaviour
{
    [SerializeField]
    private int m_score = 0;

    [SerializeField]
    private PlayerData m_playerData = null;

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
            var parentObj = other.transform.parent.gameObject;
            GivePoint(parentObj);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// otherObjにm_scoreと同じ値のポイントを与えます。
    /// </summary>
    private void GivePoint(GameObject parentObj)
    {
        InputData inputData = parentObj.gameObject.GetComponent<InputData>();
        PointManager pointManager = this.transform.parent.GetComponent<PointManager>();

        int playerIndex = inputData.SelfNumber;
        pointManager.AddScore(playerIndex, m_score);
    }
}
