using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField]
    private List<AudioClip> m_audioClips = new List<AudioClip>();

    [SerializeField]
    private float m_audioFadeRate = 0.0f;

    private float m_substractValue = 0.0f;
    private float m_initialVolume = 0.0f;

    private bool m_isFade = false;

    public static BackGroundMusicManager Instance = null;

    public enum MusicName
    {
        TeamAndRoll,
        MainBGM,
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

        m_initialVolume = m_audioSource.volume;
        AudioVolumeConvertPercentage();
    }

    private void FixedUpdate()
    {
        if(!m_isFade)
        {
            return;
        }

        VolumeFade();
    }

    private void AudioVolumeConvertPercentage()
    {
        m_substractValue = (m_audioSource.volume / 100.0f) * m_audioFadeRate;
    }

    private void VolumeFade()
    {
        if(m_audioSource.volume > 0.0f)
        {
            m_audioSource.volume += -m_substractValue;
        }
        else
        {
            m_isFade = false;
            m_audioSource.clip = null;
            m_audioSource.Stop();
            m_audioSource.volume = m_initialVolume;
        }
    }

    public void OnPlayOneShot(MusicName musicNum)
    {
        m_audioSource.clip = m_audioClips[(int)musicNum];
        m_audioSource.PlayOneShot(m_audioClips[(int)musicNum]);
    }

    public void StartVolumeFadeOut()
    {
        m_isFade = true;
    }
}
