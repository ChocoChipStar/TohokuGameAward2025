using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName = "ScriptableObjects/ScoreData")]
public class ScoreData : ScriptableObject
{
    [SerializeField]
    private ParamsData m_paramsData = new ParamsData();

    public ParamsData Params { get { return m_paramsData; } private set { m_paramsData = value; } }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("爆弾がヒューマノイドに当たった時に追加される大砲側のスコア")]
        private int m_hitHumanoidScore = 0;

        [SerializeField, Header("爆弾に当たった時に減るヒューマノイド側のスコア")]
        private int m_deathScore = 0;

        [SerializeField, Header("ヒューマノイド側が王冠を獲得した時のスコア（シルバー）")]
        private int m_silverScore = 0;

        [SerializeField, Header("ヒューマノイド側が王冠を獲得した時のスコア（ゴールド）")]
        private int m_goldScore = 0;

        [SerializeField, Header("ヒューマノイド側が王冠を獲得した時のスコア（ダイヤモンド）")]
        private int m_diamondScore = 0;

        public int HitHumanoidScore { get { return m_hitHumanoidScore; } private set { m_hitHumanoidScore = value; } }
        public int DeathScore { get { return m_deathScore; } private set { m_deathScore = value; } }
        public int SilverScore { get { return m_silverScore; } private set { m_silverScore = value; } }
        public int GoldScore { get { return m_goldScore; } private set { m_goldScore = value; } }
        public int DiamondScore { get { return m_diamondScore; } private set { m_diamondScore = value; } }
    }
}