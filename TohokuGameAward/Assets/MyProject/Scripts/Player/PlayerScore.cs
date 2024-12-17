using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    private int m_currentScore = 0;

    private void Start()
    {
        m_playerManager = gameObject.GetComponentInParent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Crownに触れたら
        if(TagManager.Instance.SearchedTagName(collider.gameObject, TagManager.Type.Crown))
        {
            PointChecker(collider);
        }
    }

    private void PointChecker(Collider collider)
    {
        //ポイントを追加、playerManagerに反映
        var point = collider.GetComponent<CrownScore>();
        m_currentScore += point.GetScore();
        Destroy(collider.gameObject);
        m_playerManager.ScoreControl(m_currentScore, this.gameObject);
    }
}
