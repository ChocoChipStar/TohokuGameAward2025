using UnityEngine;

public static class TagData
{
    // 「Player」タグは必ず配列の末尾にすること
    public static readonly string[] TagList = new string[] { "Ground", "Wall", "Bomb", "Detected", "Drone", "Crown" ,"Player" };

    public enum Names
    {
        Ground,
        Wall,
        Bomb,
        Detected,
        Drone,
        Crown,
        Player
    }

    public static string GetTag(Names tagName)
    {
        return TagList[(int)tagName];
    }
}
