using UnityEngine;

public static class TagData
{
    // �uPlayer�v�^�O�͕K���z��̖����ɂ��邱��
    public static readonly string[] NameList = new string[] { "Stage", "Arm", "Bomb", "Item", "Player" };

    public enum TagsNumber
    {
        Stage,
        Arm,
        Bomb,
        Item,
        Player
    }
}
