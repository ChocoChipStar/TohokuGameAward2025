using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CrownItem : MonoBehaviour
{
    [SerializeField]
    private ScoreData m_scoreData = null;

    [SerializeField]
    private GameObject m_scoreManager = null;

    [SerializeField]
    private Type m_type = new Type();

    private bool m_isUpdateScore = false;

    private int m_score = 0;

    public enum Type
    {
        Silver,
        Gold,
        Diamond
    }

    private void Awake()
    {
        SetSelfScore();
        m_scoreManager = GameObject.Find("ScoreManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null || m_isUpdateScore)
        {
            return; 
        }

        // 接触したヒューマノイドに点数を加算する
        var parentObj = other.transform.parent.gameObject;
        if (TagManager.Instance.SearchedTagName(parentObj, TagManager.Type.Humanoid))
        {
            SoundEffectManager.Instance.OnPlayOneShot(SoundEffectManager.SoundEffectName.Point);
            ScoreManager.Instance.UpdateScore(TeamGenerator.Instance.GetCurrentHumanoidTeamName(), m_score);
            m_isUpdateScore = true;

            GetScoreText getScoreText = m_scoreManager.transform.GetComponent<GetScoreText>();
            getScoreText.ShowScoreEffect(m_score, this.transform.position);

            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 自分自身が何点獲得出来る王冠なのかを調べ、スコアを登録します
    /// </summary>
    private void SetSelfScore()
    {
        var paramsData = m_scoreData.Params;
        switch(m_type)
        {
            case Type.Silver:
                m_score = paramsData.SilverScore;
                break;

            case Type.Gold:
                m_score = paramsData.GoldScore;
                break;

            case Type.Diamond:
                m_score = paramsData.DiamondScore;
                break;

            default:
                break;
        }
    }
}
