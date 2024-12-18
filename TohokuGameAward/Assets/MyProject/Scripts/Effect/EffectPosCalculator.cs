using UnityEngine;

public class EffectPosCalculator : MonoBehaviour
{

    [SerializeField]
    private GameManager m_gameManager = null;

    [SerializeField]
    private float m_offsetAngle = 90f;

    [SerializeField]
    private float m_offsetBottom = 0.0f;

    [SerializeField]
    private float m_offsetYRight = 0.0f;

    [SerializeField]
    private float offsetYLight = 0.0f;

    /// <summary>
    /// エフェクトが正しく表示される角度を算出します。
    /// </summary>
    /// <returns></returns>
    public float GetAngle(Vector2 start, Vector2 target)
    {
        Vector2 dt = target - start;

        float rad = Mathf.Atan2(dt.y, dt.x);

        float degree = rad * Mathf.Rad2Deg;//ラジアンをオイラー角に変換

        return degree + m_offsetAngle;
    }

    /// <summary>
    /// エフェクトがずれるため補正をかけています。
    /// </summary>
    /// <returns></returns>
    public Vector3 OffsetPos(Vector3 offsetpos,Vector3 position)
    {
        //エフェクトがずれるため補正をかけています
        if (position.x >= 10)
        {

            offsetpos.y = m_offsetYRight;

            return offsetpos;
        }
        if (position.x <= -10)
        {
            offsetpos.y = offsetYLight;
            return offsetpos;
        }

        if (position.y <= -10)
        {
            offsetpos.y = m_offsetBottom;
            return offsetpos;
        }

        offsetpos = Vector2.zero;
        return offsetpos;
    }
}
