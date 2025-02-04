using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField]
    private List<AudioClip> m_audioClips = new List<AudioClip>();

    public static BackGroundMusicManager Instance = null;

    public enum MusicName
    {
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
    }

    public void OnPlayOneShot(MusicName musicNum)
    {
        m_audioSource.clip = m_audioClips[(int)musicNum];
        m_audioSource.PlayOneShot(m_audioClips[(int)musicNum]);
    }
}
