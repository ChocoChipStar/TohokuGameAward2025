using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] m_audioSource;

    [SerializeField]
    private AudioClip[] m_MusicName;

    public enum MusicName
    {
        TestBGM
    }

    public void OnPlay(MusicName musicNum)
    {
        m_audioSource[(int)musicNum].clip = m_MusicName[(int)musicNum];
        m_audioSource[(int)musicNum].Play();
    }


}
