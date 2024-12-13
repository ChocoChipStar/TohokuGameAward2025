using UnityEngine;

public class TagManager : MonoBehaviour
{
    public static TagManager Instance { get; private set; }
    // 「Player」タグは必ず配列の末尾にすること
    public static readonly string[] NameLists = new string[] { "Ground", "Wall", "Bomb", "Detected", "Player" };

    public enum Type
    {
        Ground,
        Wall,
        Bomb,
        Detected,
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

    public string GetTagName(Type name)
    {
        return NameLists[(int)name];
    }

    /// <summary>
    /// 指定ゲームオブジェクトが指定タグ名と一致するか調べます
    /// </summary>
    public bool SearchedTagName(GameObject gameObject, Type verifyTag0, Type? verifyTag1 = null, Type? verifyTag2 = null)
    {
        if (gameObject.CompareTag(NameLists[(int)verifyTag0]))
        {
            return true;
        }

        if(gameObject.CompareTag(NameLists[(int)verifyTag1]) && verifyTag1 != null)
        {
            return true;
        }

        if (gameObject.CompareTag(NameLists[(int)verifyTag2]) && verifyTag2 != null)
        {
            return true;
        }

        return false;
    }
}
