using UnityEngine;

public class SoundFade : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField, Header("何パーセントずつ音を減らすか")]
    private float m_gainPercentage = 0.0f;

    private float m_substractValue = 0.0f;

    private bool m_isFadeOut = false;

    private void Start()
    {
        m_substractValue = ConvertVolumePercentage(m_audioSource.volume, m_gainPercentage);
    }
    
    private void FixedUpdate()
    {
        if(!m_isFadeOut)
        {
            return;
        }

        m_audioSource.volume += -m_substractValue;
    }

    private float ConvertVolumePercentage(float value, float percentage = 1.0f)
    {
        return (value / 100.0f) * percentage;
    }

    public void OnPlayFade()
    {
        m_isFadeOut = true;
    }
}
