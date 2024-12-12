using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData")]
public class StageData : ScriptableObject
{
    [SerializeField]
    private StageSize m_stageSize = new StageSize();

    public StageSize Size {  get { return m_stageSize; } private set { value = m_stageSize; } }
}

[System.Serializable]
public class StageSize
{
    [SerializeField, Header("ステージの底(y座標)")]
    private float m_bottom = 0.0f;

    [SerializeField, Header("ステージの左端(x座標)")]
    private float m_leftLimit = 0.0f;

    [SerializeField, Header("ステージの右端(x座標)")]
    private float m_rightLimit = 0.0f;

    public float Bottom { get { return m_bottom; } private set { m_bottom = value; } }

    public float LeftLimit { get { return m_leftLimit; } private set { m_leftLimit = value; } }

    public float RightLimit { get { return m_rightLimit; } private set { m_rightLimit = value; } }
}
