using UnityEngine;
using UnityEngine.UI;

public class DrawTutorialImage : MonoBehaviour
{
    [SerializeField]
    private Image m_drawImage = null;

    [SerializeField]
    private Sprite[] m_tutorialSprite = new Sprite[] { };

    private int m_currentIndex = 0;
    private int m_spriteMax = 0;

    public bool IsIndexMax { get; private set; }

    private void Start()
    {
        m_spriteMax = m_tutorialSprite.Length - 1;
    }

    public void SwitchNextImage()
    {
        m_currentIndex++;
        if(m_currentIndex >= m_spriteMax)
        {
            m_currentIndex = m_spriteMax;
            IsIndexMax = true;
        }

        m_drawImage.sprite = m_tutorialSprite[m_currentIndex];
    }

    public void SwitchPreviousImage()
    {
        m_currentIndex--;
        if(m_currentIndex <= 0)
        {
            m_currentIndex = 0;
        }

        m_drawImage.sprite = m_tutorialSprite[m_currentIndex];
    }
}
