using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    // �V���O���g���̃C���X�^���X
    public static BombManager Instance { get; private set; }

    public List<BombData> allBombData;

    void Awake()
    {
        // ���łɃC���X�^���X�����݂��Ă���ꍇ�͔j��
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[�����ׂ��ł��ێ�����ꍇ
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