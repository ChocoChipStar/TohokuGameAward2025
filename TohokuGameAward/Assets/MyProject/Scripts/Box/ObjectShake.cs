using System.Collections;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    [SerializeField]
    private bool m_isShake = false;

    [SerializeField]
    private float m_duration = 0.0f;

    [SerializeField]
    private float m_magnitude = 0.0f;

    [SerializeField]
    private bool m_isEndShake = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isShake)
        {
            Shake(m_duration,m_magnitude);
            m_isShake = false;
        }
    }

    private void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = transform.localPosition;

       // var elapsed = 0f;

        while (!m_isEndShake)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            //elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = pos;
    }

    /// <summary>
    ///オブジェクトの振動を開始します。 
    /// </summary>
    public void SetShake()
    {
        m_isShake = true;
    }

    /// <summary>
    /// オブジェクトの振動を停止します。
    /// </summary>
    public void SetEndShake()
    {
        m_isEndShake = true;
    }
}
