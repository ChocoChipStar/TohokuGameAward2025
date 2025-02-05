using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager Instance = null;

    [SerializeField]
    private PlayerManager m_playerManager = null;


    /// <summary> 前フレームのコントローラーのハッシュ値リスト </summary>
    private HashSet<string> m_previousFrameHashList = new HashSet<string>();
    
    /// <summary> コントローラー名とプレイヤー番号を紐づけ </summary>
    public Dictionary<string, int> ControllerMap { get; private set; } = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    private void Update()
    {
        UpdateControllerHashList();
    }

    /// <summary>
    /// 接続コントローラー調べ、プレイヤーのアクティブ状態を切り替えます
    /// </summary>
    private void UpdateControllerHashList()
    {
        var currentHashList = new HashSet<string>();

        // 現在接続中のコントローラーを取得する
        foreach(var gamepad in Gamepad.all)
        {
            currentHashList.Add(gamepad.name);

            // 再接続または、新しいコントローラーが接続されてたら
            if(!m_previousFrameHashList.Contains(gamepad.name))
            {
                AssignPlayer(gamepad.name);
            }
        }

        // 切断されたコントローラーを探します
        foreach(var oldHashList in m_previousFrameHashList)
        {
            if(!currentHashList.Contains(oldHashList))
            {
                RemovePlayer(oldHashList);
            }
        }

        // 前フレーム情報を更新する
        m_previousFrameHashList = currentHashList;
    }

    /// <summary>
    /// コントローラーの追加、再接続と同時にプレイヤーのアクティブ状態も切り替えます
    /// </summary>
    /// <param name="newControllerName"> 新しいコントローラー名 </param>
    private void AssignPlayer(string newControllerName)
    {
        // 既に登録差ているコントローラーの場合returnする
        if(ControllerMap.ContainsKey(newControllerName))
        {
            return;
        }

        for (int i = 0; i < PlayerManager.PlayerMax; i++)
        {
            // 空きスロットがあれば新しいコントローラーを割り当てます（最大4スロット = プレイヤー数）
            if (!ControllerMap.ContainsValue(i))
            {
                // コントローラー名とプレイヤー番号をペアで登録する
                ControllerMap[newControllerName] = i;

                m_playerManager.Instances[i].SetActive(true);
                break;
            }
        }
    }

    /// <summary>
    /// コントローラーの切断と同時に、プレイヤーを非アクティブにします
    /// </summary>
    /// <param name="disconnectedControllerName"> 切断されたコントローラー名 </param>
    private void RemovePlayer(string disconnectedControllerName)
    {
        if (ControllerMap.TryGetValue(disconnectedControllerName, out int disconnectedPlayerNum))
        {
            m_playerManager.Instances[disconnectedPlayerNum].SetActive(false);
            ControllerMap.Remove(disconnectedControllerName);
        }
    }
}
