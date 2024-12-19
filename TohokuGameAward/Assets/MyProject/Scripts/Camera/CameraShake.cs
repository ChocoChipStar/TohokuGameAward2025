using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float m_duration = 0f;
    [SerializeField]
    private float m_magnitude = 0f;

    private bool m_isShaking = false;

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (m_isShaking)
        {
           yield break;
        }

        m_isShaking = true;
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            transform.position = originalPosition + Random.insideUnitSphere * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
        m_isShaking = false;
    }

    // デバッグ用　スペースキーで揺れる
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shake(m_duration, m_magnitude));
        }
    }
}

