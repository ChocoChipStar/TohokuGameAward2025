using UnityEngine;

public class Dorone_Moover : MonoBehaviour
{
    [SerializeField]
    private float m_leftEndOfTheStage_x = 0.0f;

    [SerializeField]
    private float m_rightEndOfTheStage_x = 0.0f;

    [SerializeField]
    private float m_speed = 0.0f;

    private Vector3 m_currentPos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        DoroneMove();
    }

    private void DoroneMove()
    {
        m_currentPos = this.transform.position;

        if(IsAtEndOfTheStage())
        {
            m_speed *= -1;
        }

        m_currentPos.x += m_speed;

        this.transform.position = m_currentPos;
    }

    private bool IsAtEndOfTheStage()
    {
        if(m_currentPos.x < m_leftEndOfTheStage_x ||
           m_rightEndOfTheStage_x < m_currentPos.x)
        {
            return true;
        }

        return false;
    }
}
