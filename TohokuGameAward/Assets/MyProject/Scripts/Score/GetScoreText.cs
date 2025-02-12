using TMPro;
using UnityEngine;

public class GetScoreText : MonoBehaviour
{
    [SerializeField]
    private GameObject m_scoreTextPrefab = null;

    [SerializeField]
    private Vector3 m_textOffset = Vector3.zero;

    [SerializeField]
    private float m_moveTime = 0;

    [SerializeField]
    private Vector3 m_moveDistance = Vector3.zero;

    [SerializeField]
    private float m_destroyDelay = 0; 

    [SerializeField]
    private GameObject m_canvas = null;

    private Vector3 m_scorePosition = Vector3.zero;

    void Update()
    {

    }

    public void ShowScoreEffect(int score,Vector3 scorePos)
    {
        Vector3 textPos = Camera.main.WorldToScreenPoint(scorePos + m_textOffset); 

        GameObject scoreText = Instantiate(m_scoreTextPrefab, Vector3.zero, Quaternion.identity, m_canvas.transform);


        TextMeshProUGUI textComponent = scoreText.GetComponent<TextMeshProUGUI>();
        textComponent.text = score.ToString(); 
        textComponent.rectTransform.position = textPos;

        StartCoroutine(MoveScoreText(scoreText));

        Destroy(scoreText, m_destroyDelay);
    }

    private System.Collections.IEnumerator MoveScoreText(GameObject scoreText)
    {
        float timeElapsed = 0f;
        Vector3 initialPosition = scoreText.transform.position;
        Vector3 targetPosition = initialPosition + m_moveDistance; // 設定した分上に移動

        while (timeElapsed < m_moveTime) 
        {
            scoreText.transform.position = Vector3.Lerp(initialPosition, targetPosition, timeElapsed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // 完了後の最終位置
        scoreText.transform.position = targetPosition;
    }
}
