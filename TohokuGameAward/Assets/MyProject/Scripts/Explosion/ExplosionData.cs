using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosionData", menuName = "ScriptableObjects/ExplosionData", order = 1)]
public class ExplosionData : ScriptableObject
{
    [SerializeField]
    private ParamsData m_paramsData = null;

    public ParamsData Params { get {  return m_paramsData; } private set { value = m_paramsData; } }

    [System.Serializable]
    public class ParamsData
    {
        [SerializeField, Header("爆風の判定が実際に発生するまでのディレイ")]
        private float m_colliderActivateDelayTime = 0.0f;

        [SerializeField, Header("爆風の持続フレーム数")]
        private int m_durationFrameCount = 0;

        [SerializeField, Header("エフェクト含めすべての再生が終了するまでの時間")]
        private float m_effectEndTime = 0.0f;

        public float ColliderActivateDelayTime { get { return m_colliderActivateDelayTime; } private set { value = m_colliderActivateDelayTime; } }
        public int DurationFrameCount { get { return m_durationFrameCount; } private set { value = DurationFrameCount; } }
        public float EffectEndTime { get { return m_effectEndTime; } private set { value = m_effectEndTime; } }
    }
}
