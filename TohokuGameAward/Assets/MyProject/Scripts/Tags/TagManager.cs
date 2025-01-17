using UnityEngine;

public class TagManager : MonoBehaviour
{
    public static TagManager Instance { get; private set; }
    // 「Player」タグは必ず配列の末尾にすること
    public static readonly string[] NameLists = new string[] { "Ground", "Wall", "Bomb","Explosion", "Detected", "Stage", "Drone", "Crown", "Cannon", "Player" };

    public enum Type
    {
        Ground,
        Wall,
        Bomb,
        Explosion,
        Detected,
        Stage,
        Drone,
        Crown,
        Cannon,
        Player
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// 指定タグが存在する場合のみタグが一致するか調べる
    /// </summary>
    private bool IsExistTag(GameObject gameObject, Type? tag)
    {
        return tag != null && gameObject.CompareTag(NameLists[(int)tag]);
    }

    public string GetTagName(Type name)
    {
        return NameLists[(int)name];
    }

    /// <summary>
    /// 指定ゲームオブジェクトが指定タグ名と一致するか調べます
    /// </summary>
    public bool SearchedTagName(GameObject gameObject, Type verifyTag0, Type? verifyTag1 = null, Type? verifyTag2 = null)
    {
        return IsExistTag(gameObject, verifyTag0) || IsExistTag(gameObject,verifyTag1) || IsExistTag(gameObject,verifyTag2);
    }
}
