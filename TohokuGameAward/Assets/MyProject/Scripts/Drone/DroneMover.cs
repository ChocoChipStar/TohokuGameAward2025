﻿using UnityEngine;

public class DroneMover : MonoBehaviour
{
    [SerializeField]
    private float m_leftEndOfTheStage_x = 0.0f;

    [SerializeField]
    private float m_rightEndOfTheStage_x = 0.0f;

    [SerializeField]
    private float m_speed = 0.0f;

    private Vector3 m_currentPos = Vector3.zero;

    void Update()
    {
        DroneMove();
    }

    private void DroneMove()
    {
        m_currentPos = this.transform.position;

        if(IsAtEndOfTheStage())
        {
            m_speed *= -1;
        }

        m_currentPos.x += m_speed * Time.deltaTime;

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
