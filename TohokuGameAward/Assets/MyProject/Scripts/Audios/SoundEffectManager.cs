using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField]
    private List<AudioClip> m_audioClips = new List<AudioClip>();

    public static SoundEffectManager Instance = null;

    public enum SoundEffectName
    {
        Death,
        Explosion,
        Cannon,
        Point,
        Ready,
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnPlayOneShot(SoundEffectName seNum)
    {
        m_audioSource.PlayOneShot(m_audioClips[(int)seNum]);
    }

    public void OnPlay(SoundEffectName seNum)
    {
        m_audioSource.clip = m_audioClips[(int)seNum];
        m_audioSource.loop = true;
        m_audioSource.Play();
    }
    
    public void OnStop(SoundEffectName seNum)
    {
        m_audioSource.Stop();
    }
}
