using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField]
    private List<AudioClip> m_audioClips = new List<AudioClip>();

    public static SoundEffectManager Instance = null;

    public enum TitleScenePattern
    {
        Start
    }

    public enum MainScenePattern
    {
        Death,
        Explosion,
        Cannon,
        Point,
        LowSurvivePoint,
        MiddleSurvivePoint,
        HigtSurvivePoint,
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

    public void OnPlayOneShot(int patternNum)
    {
        m_audioSource.PlayOneShot(m_audioClips[patternNum]);
    }

    public void OnPlay(int patternNum)
    {
        m_audioSource.clip = m_audioClips[patternNum];
        m_audioSource.loop = true;
        m_audioSource.Play();
    }
    
    public void OnStop()
    {
        m_audioSource.Stop();
    }
}
