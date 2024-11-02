using UnityEngine;

public static class TagData
{
    // 「Player」タグは必ず配列の末尾にすること
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
