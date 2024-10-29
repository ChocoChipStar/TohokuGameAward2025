using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] m_audioSources;

    [SerializeField]
    private AudioClip[] m_audioClips;

    public enum MusicName
    {
        TestBGM
    }

    public void OnPlay(MusicName musicNum)
    {
        m_audioSources[(int)musicNum].clip = m_audioClips[(int)musicNum];
        m_audioSources[(int)musicNum].Play();
    }



}
