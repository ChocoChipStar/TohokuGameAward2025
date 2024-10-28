using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] m_audioSource;

    [SerializeField]
    private AudioClip[] m_SoundEffectName;

    public enum SoundEffectName
    {
        TestSE
    }

    public void OnPlayOneShot(SoundEffectName seNum)
    {
        m_audioSource[(int)seNum].PlayOneShot(m_SoundEffectName[(int)seNum]);
    }


}
