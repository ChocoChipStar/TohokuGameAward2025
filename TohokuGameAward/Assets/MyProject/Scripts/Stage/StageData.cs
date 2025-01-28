using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData")]
public class StageData : ScriptableObject
{
    [SerializeField]
    private BorderData m_borderData = new BorderData();

    public BorderData Border {  get { return m_borderData; } private set { value = m_borderData; } }
}

[System.Serializable]
public class BorderData
{
    [SerializeField, Header("ステージの底(y座標)")]
    private float m_bottomLimit = 0.0f;

    [SerializeField, Header("ステージの上端(x座標)")]
    private float m_topLimit = 0.0f;

    [SerializeField, Header("ステージの左端(x座標)")]
    private float m_leftLimit = 0.0f;

    [SerializeField, Header("ステージの右端(x座標)")]
    private float m_rightLimit = 0.0f;

    public float Top { get { return m_topLimit; } private set { m_topLimit = value; } }
    public float Bottom { get { return m_bottomLimit; } private set { m_bottomLimit = value; } }
    public float Left { get { return m_leftLimit; } private set { m_leftLimit = value; } }
    public float Right { get { return m_rightLimit; } private set { m_rightLimit = value; } }
}
