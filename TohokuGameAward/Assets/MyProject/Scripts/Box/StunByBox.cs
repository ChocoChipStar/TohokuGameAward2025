using System.Collections.Generic;
using UnityEngine;

public class StunByBox : MonoBehaviour
{
    [SerializeField]
    private float m_stanTime = 0;

    private BoxFlyController m_boxFlyController = null;

    private List<HumanoidMover> m_humanoidMover = new List<HumanoidMover>();

    private List<float> m_currentStanTime = new List<float>();

    void Start()
    {
        m_boxFlyController = GetComponent<BoxFlyController>();
    }

    private void Update()
    {
        StanTimer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collisionの親のタグがPlayerだったら）
        if (m_boxFlyController.IsFlying)
        {
            Stan(collision.gameObject);
        }
    }

    private void Stan(GameObject stanPlayer)
    {
        if(!stanPlayer.gameObject.CompareTag("Humanoid"))
        {
            return;
        }

        HumanoidMover hm = stanPlayer.GetComponent<HumanoidMover>();
        hm.StunStart(m_stanTime);
        m_humanoidMover.Add(hm);
        m_currentStanTime.Add(m_stanTime);
        //hm.SetOperable(false);
    }

    private void StanTimer()
    {
        if(m_currentStanTime.Count == 0)
        {
            return;
        }

        for (int i = 0; i < m_currentStanTime.Count; i++)
        {
            if(0 < m_currentStanTime[i])
            {
                m_currentStanTime[i] -= Time.deltaTime;
            }
            else
            {
                endStan(i);
                m_humanoidMover.RemoveAt(i);
                m_currentStanTime.RemoveAt(i);
            }
        }
    }

    private void endStan(int Num)
    {
        m_humanoidMover[Num].SetOperable(true);
    }
}