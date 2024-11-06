using UnityEngine;

[CreateAssetMenu(fileName = "NewBombData", menuName = "ScriptableObjects/BombData", order = 1)]
public class BombData : ScriptableObject
{
    [SerializeField]
    private ThrowData m_throwData = null;

    [SerializeField]
    private StatusData m_statusData = null;

    [System.Serializable]
    public class ThrowData
    {
        [SerializeField, Header("‰¡“Š‚°‚ÌŠp“x")]
        private float m_throwAngleSide = 0.0f;

        [SerializeField, Header("ã“Š‚°‚ÌŠp“x")]
        private float m_throwAngleUpper = 0.0f;

        [SerializeField, Header("‰º“Š‚°‚ÌŠp“x")]
        private float m_throwAngleUnder = 0.0f;

        public float ThrowAngleSide { get { return m_throwAngleSide; } private set { value = m_throwAngleSide; } }
        public float ThrowAngleUpper { get { return m_throwAngleUpper; } private set { value = m_throwAngleUpper; } }
        public float ThrowAngleUnder { get { return m_throwAngleUnder; } private set { value = m_throwAngleUnder; } }
    }

    [System.Serializable]
    public class StatusData
    {
        [SerializeField, Header("”š’e‚Ìd‚³")]
        private float m_bombMass = 0.0f;

        [SerializeField, Header("‰ñ“]–€C‚Ì‹­‚³")]
        private float m_angularDrag = 0.0f;

        [SerializeField, Header("”š”­‚·‚é‚Ü‚Å‚ÌŠÔ")]
        private float m_explosionTime = 0.0f;

        [SerializeField, Header("”š•—‚É‚æ‚é‚«”ò‚Ñ‚Ì‹­‚³")]
        private float m_blastPower = 0.0f;

        [SerializeField, Header("”š’e‚Ì”š•—‚ÌL‚³")]
        private float m_blastRange = 0.0f;

        [SerializeField, Header("‰ñ“]‚Ì’†S“_")]
        private float m_bombPivot = 0.0f;

        [SerializeField, Header("”š’e‚Ì”")]
        private int m_bombCount = 0;

        [SerializeField, Header("‘¦‹N”š‚Ì—L–³")]
        private bool m_isPromptExplosion = false;

        public float BombMass { get { return m_bombMass; } private set { value = m_bombMass; } }
        public float AngularDrag { get { return m_angularDrag; } private set { value = m_angularDrag; } }
        public float ExplosionTime { get { return m_explosionTime; } private set { value = m_explosionTime; } }
        public float BlastPower { get { return m_blastPower; } private set { value = m_blastPower; } }
        public float BlastRange { get { return m_blastRange; } private set { value = m_blastRange; } }
        public float BombPivot { get { return m_bombPivot; } private set { value = m_bombPivot; } }
        public int BombCount { get { return m_bombCount; } private set { value = m_bombCount; } }
        public bool IsPromptExplosion { get { return m_isPromptExplosion; } private set { value = m_isPromptExplosion;} }
    }

    [System.Serializable]
    public class ParamsData
    {

    }
}