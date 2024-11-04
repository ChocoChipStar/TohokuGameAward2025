using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    // シングルトンのインスタンス
    public static BombManager Instance { get; private set; }

    public List<BombData> allBombData;

    void Awake()
    {
        // すでにインスタンスが存在している場合は破棄
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンを跨いでも保持する場合
        }
    }

    public BombData GetBombDataByGenre(BombData.BombGenre genre)
    {
        foreach (var bombData in allBombData)
        {
            if (bombData.BombType == genre)
            {
                return bombData;
            }
        }
        return null;
    }
}