using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField]
    private BombData[] allBombData = new BombData[(int)BombData.BombMax - 1];
    
    public static BombManager Instance { get; private set; }

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

    public BombData GetBombData(BombData.BombType type)
    {
        foreach (var bombData in allBombData)
        {
            if (bombData.Type == type)
            {
                return bombData;
            }
        }
        return null;
    }
}