using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerPrefab = null;

    //private int m_lastTimeCount = 0;

    //private Dictionary<Gamepad, bool> m_padCurrent = new Dictionary<Gamepad, bool>();

    //private List<int> m_randomIndex = new List<int>();

    //private const int MaxNumberRollCount = 1000;
    private const int PlayerMax = 4;

    private static readonly float[] StartPos = new float[] { -4.5f, -1.5f, 1.5f, 4.5f };

    private void Awake()
    {
        //NonOverlappingRandomValue();

        //var padCount = Gamepad.all.Count;
        for (int i = 0; i < PlayerMax; i++)
        {
            var instance = Instantiate(m_playerPrefab, new Vector3(StartPos[i], 0.0f, 0.0f), Quaternion.identity);

            instance.name = "Player" + (i + 1);
            instance.transform.SetParent(this.transform);

            var inputData = instance.gameObject.GetComponent<InputData>();
            if(inputData != null)
            {
                inputData.SetNumber(i);
            }
            //if (i > padCount)
            //{
            //    instance.gameObject.SetActive(false);
            //    continue;
            //}

            //m_padCurrent[Gamepad.all[i]] = true;
        }
    }

    //private void OnEnable()
    //{
    //    // OnEnableでオブジェクト有効化時にも呼び出される
    //    // 基本的にはコントローラーの接続、切断、変更が起こった際に自動呼出しされる(独立自動呼出し)
    //    InputSystem.onDeviceChange += CheckDeviceConnection;
    //}

    //private void OnDisable()
    //{
    //    InputSystem.onDeviceChange -= CheckDeviceConnection;
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="device"> InputDevice型（Gamepad,Keyboard,Mouse）など指定可能 </param>
    ///// <param name="change"> デバイスの接続状態や設定の変更がされたことを示す列挙型 </param>
    //private void CheckDeviceConnection(InputDevice device, InputDeviceChange change)
    //{
    //    // InputDevice型の中から今回の引数がGamepad型か調べる
    //    if(device is Gamepad gamepad)
    //    {
    //        switch (change)
    //        {
    //            case InputDeviceChange.Disconnected:
    //                InitializeController(gamepad, false);
    //                break;
                
    //            case InputDeviceChange.Reconnected:
    //                InitializeController(gamepad, true);
    //                break;
    //        }
    //    }
    //}

    //private void InitializeController(Gamepad gamepad, bool isReconnected)
    //{
    //    var number = GetChangeDeviceControllerIndex(gamepad);
    //    if(isReconnected)
    //    {
    //        m_padCurrent[gamepad] = true;
    //        ReconnectedController(number + 1);
    //        return;
    //    }

    //    m_padCurrent[gamepad] = false;
    //    DisconnectedController(number + 1);
    //}

    //private int GetChangeDeviceControllerIndex(Gamepad gamepad)
    //{
    //    var gamepadList = m_padCurrent.ToList();
    //    for(int i = 0; i < gamepadList.Count; i++)
    //    {
    //        if (gamepadList[i].Key == gamepad)
    //        {
    //            return i;
    //        }
    //    }

    //    return -1;
    //}

    //private void ReconnectedController(int playerNum)
    //{
    //    Debug.Log(playerNum + "P Reconnected Controller");
    //}

    //private void DisconnectedController(int playerNum)
    //{
    //    Debug.Log(playerNum + "P Disconnected Controller");
    //}

    //private void NonOverlappingRandomValue()
    //{
    //    for(int i = 0; i < MaxNumberRollCount; i++)
    //    {
    //        if(m_randomIndex.Count >= PlayerMax)
    //        {
    //            return;
    //        }

    //        var randomValue = Random.Range(0, PlayerMax);
    //        if(!m_randomIndex.Contains(randomValue))
    //        {
    //            m_randomIndex.Add(randomValue);
    //        }
    //    }
    //}
}
