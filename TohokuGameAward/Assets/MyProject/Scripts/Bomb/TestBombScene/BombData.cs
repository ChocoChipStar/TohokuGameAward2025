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
        [SerializeField, Header("�������̊p�x")]
        private float m_throwAngleSide = 0.0f;

        [SerializeField, Header("�㓊���̊p�x")]
        private float m_throwAngleUpper = 0.0f;

        [SerializeField, Header("�������̊p�x")]
        private float m_throwAngleUnder = 0.0f;

        public float ThrowAngleSide { get { return m_throwAngleSide; } private set { value = m_throwAngleSide; } }
        public float ThrowAngleUpper { get { return m_throwAngleUpper; } private set { value = m_throwAngleUpper; } }
        public float ThrowAngleUnder { get { return m_throwAngleUnder; } private set { value = m_throwAngleUnder; } }
    }

    [System.Serializable]
    public class StatusData
    {
        [SerializeField, Header("���e�̏d��")]
        private float m_bombMass = 0.0f;

        [SerializeField, Header("��]���C�̋���")]
        private float m_angularDrag = 0.0f;

        [SerializeField, Header("��������܂ł̎���")]
        private float m_explosionTime = 0.0f;

        [SerializeField, Header("�����ɂ�鐁����т̋���")]
        private float m_blastPower = 0.0f;

        [SerializeField, Header("���e�̔����̍L��")]
        private float m_blastRange = 0.0f;

        [SerializeField, Header("��]�̒��S�_")]
        private float m_bombPivot = 0.0f;

        [SerializeField, Header("���e�̐�")]
        private int m_bombCount = 0;

        [SerializeField, Header("���N���̗L��")]
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