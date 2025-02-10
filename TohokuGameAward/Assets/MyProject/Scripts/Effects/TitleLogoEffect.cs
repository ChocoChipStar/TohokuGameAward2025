using System.Collections;
using UnityEngine;

public class TitleLogoEffect : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_randomLogo = null;

    [SerializeField]
    private Sprite[] m_logoImages = null;

    [SerializeField]
    private float m_delayTime = 0;

    private bool m_appeared = false;

    private void OnEnable()
    {
        StartCoroutine(AppearLogoDelay());
    }

    private IEnumerator AppearLogoDelay()
    {
        yield return new WaitForSeconds(m_delayTime);

        if (!m_appeared)
        {
            int num = Random.Range(0, m_logoImages.Length);

            m_randomLogo.sprite = m_logoImages[num];
            m_randomLogo.gameObject.SetActive(true);
            m_appeared = true;
        }
    }
}
