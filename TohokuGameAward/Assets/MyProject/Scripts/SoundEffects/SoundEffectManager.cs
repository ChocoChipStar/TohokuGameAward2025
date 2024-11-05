using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BackGroundMusicManager;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] m_audioSources;

    [SerializeField]
    private AudioClip[] m_audioClips;

    public enum SoundEffectName
    {
        TestSE
    }
    public void OnPlayOneShot(SoundEffectName seNum)
    {
        m_audioSources[(int)seNum].PlayOneShot(m_audioClips[(int)seNum]);
    }
    public void OnPlay(SoundEffectName seNum)
    {
        m_audioSources[(int)seNum].clip = m_audioClips[(int)seNum];
        m_audioSources[(int)seNum].loop = true;
        m_audioSources[(int)seNum].Play();
        
    }
    public void OnStop(SoundEffectName seNum)
    {
      
        m_audioSources[(int)(seNum)].Stop();
    }
}
