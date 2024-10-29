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

    // �C���X�y�N�^�[�ŃW���������w�肷��
    public BombGenre bombGenre;

    [Header("�����܂ł̎���[s]")]
    public float time = 3.0f;

    [Header("�����ɓ��������Ƃ��ɐ�����ԗ͂̋���")]
    public float power = 1;

    [Header("�����̓�����͈�")]
    public float size = 2;

    [Header("��]�̒��S�_")]
    public float pivot = -1;

    [Header("���e�̐�")]
    public int count = 0;

    [Header("���e�������̏ꍇ�A�o���������e")]
    public GameObject bomb;
}