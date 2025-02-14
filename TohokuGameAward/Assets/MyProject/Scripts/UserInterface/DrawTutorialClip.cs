using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DrawTutorialClip : MonoBehaviour
{
    [SerializeField]
    private ScreenFade m_screenFade = null;

    [SerializeField]
    private VideoPlayer m_videoPlayer = null;

    [SerializeField]
    private VideoClip[] m_tutorialClips = new VideoClip[] { };

    [SerializeField]
    private Image m_thumbnailImage = null;

    private int m_currentIndex = 0;
    private int m_clipMax = 0;

    private bool m_isFirstClipStart = false;

    public bool IsIndexMax { get; private set; }

    private void Start()
    {
        m_clipMax = m_tutorialClips.Length - 1;
        m_videoPlayer.clip = null;
    }

    private void Update()
    {
        if(!m_screenFade.IsFinishFadeIn)
        {
            return;
        }
        
        if(!m_isFirstClipStart)
        {
            m_isFirstClipStart = true;
            m_videoPlayer.clip = m_tutorialClips[0];
            m_videoPlayer.Play();
            Invoke("DelayDisabledThumbnailImage", 0.2f);
        }
    }

    private void DelayDisabledThumbnailImage()
    {
        m_thumbnailImage.enabled = false;
    }

    public void SwitchNextImage()
    {
        m_currentIndex++;
        if(m_currentIndex >= m_clipMax)
        {
            m_currentIndex = m_clipMax;
            IsIndexMax = true;
        }

        m_videoPlayer.clip = m_tutorialClips[m_currentIndex];
        m_videoPlayer.Play();
    }

    public void SwitchPreviousImage()
    {
        m_currentIndex--;
        if(m_currentIndex <= 0)
        {
            m_currentIndex = 0;
        }

        m_videoPlayer.clip = m_tutorialClips[m_currentIndex];
        m_videoPlayer.Play();
    }
}
