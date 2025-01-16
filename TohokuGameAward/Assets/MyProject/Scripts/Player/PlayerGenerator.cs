using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerPrefab = null;

    private int m_lastTimeCount = 0;

    private Gamepad[] m_padCurrent = new Gamepad[PlayerMax];

    private List<int> m_randomIndex = new List<int>();

    private ReactiveProperty<int> m_currentPadCount = new ReactiveProperty<int>();

    private const int MaxNumberRollCount = 1000;
    private const int PlayerMax = 4;

    private static readonly float[] StartPos = new float[] { -4.5f, -1.5f, 1.5f, 4.5f };

    private void Awake()
    {
        NonOverlappingRandomValue();

        var padCount = Gamepad.all.Count;
        for (int i = 0; i < PlayerMax; i++)
        {
            var instance = Instantiate(m_playerPrefab, new Vector3(StartPos[i], 0.0f, 0.0f), Quaternion.identity);
            
            instance.name = "Player" + (i + 1);
            instance.transform.SetParent(this.transform);

            if(padCount >= i)
            {
                instance.gameObject.SetActive(false);
            }

            if (Gamepad.all[i] == null)
            {
                continue;
            }
            m_padCurrent[i] = Gamepad.all[i];
        }

        m_currentPadCount.Value = Gamepad.all.Count;
        m_currentPadCount.Subscribe(msg => A()).AddTo(this);
    }

    private void Update()
    {
        //m_currentPadCount.Value = Gamepad.;
    }

    private void A()
    {
        for(int i = 0; i < PlayerMax; i++)
        {
            if (Gamepad.all[i] == null)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }

            m_padCurrent[i] = Gamepad.all[i];
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void NonOverlappingRandomValue()
    {
        for(int i = 0; i < MaxNumberRollCount; i++)
        {
            if(m_randomIndex.Count >= PlayerMax)
            {
                return;
            }

            var randomValue = Random.Range(0, PlayerMax);
            if(!m_randomIndex.Contains(randomValue))
            {
                m_randomIndex.Add(randomValue);
            }
        }
    }
}
