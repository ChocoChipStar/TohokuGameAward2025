using System.Runtime.CompilerServices;
using UnityEngine;

// ���e�̃f�[�^��ScriptableObject�ŊǗ�����
[CreateAssetMenu(fileName = "NewBombData", menuName = "ScriptableObjects/BombData", order = 1)]
public class BombData : ScriptableObject
{
    // ���e�̃W���������`����񋓌^
    public enum BombGenre
    {
        [InspectorName("�m�[�}��")]
        Normal,
        [InspectorName("�C���p���X")]
        Impulse,
        [InspectorName("�~�j")]
        Mini
    }

    [SerializeField]
    private BombGenre m_bombType;

    [SerializeField, Header("�����܂ł̎���[s]")]
    private float m_explosionTime = 0.0f;

    [SerializeField, Header("�����ɓ��������Ƃ��ɐ�����ԗ͂̋���")]
    private float m_blastPower = 0.0f;

    [SerializeField, Header("���e�̔����͈�")]
    private float m_blastRange = 0.0f;

    [SerializeField, Header("��]�̒��S�_")]
    private float m_bombPivot = -1.0f;

    [SerializeField, Header("���e�̐�")]
    private int m_bombCount = 0;

    [SerializeField, Header("���e�������̏ꍇ�A�o���������e")]
    private GameObject m_otherBombObj = null;

    public BombGenre BombType { get { return m_bombType; } private set { value = m_bombType; } }
    public float ExplosionTime { get { return m_explosionTime; } private set { value = m_explosionTime; } }
    public float BlastPower { get { return m_blastPower; } private set { value = m_blastPower; } }
    public float BlastRange { get { return m_blastRange; } private set { value = m_blastRange; } }
    public float BombPivot { get { return m_bombPivot; } private set { value = m_bombPivot; } }
    public int BombCount { get { return m_bombCount; } private set { value = m_bombCount; } }
    public GameObject OtherBombObj { get { return m_otherBombObj; } private set { value = m_otherBombObj; } }
}