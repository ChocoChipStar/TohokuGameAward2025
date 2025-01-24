using UnityEngine;

public class CannonCanvasMover : MonoBehaviour
{
    [SerializeField]
    private Canvas m_canvas = null;

    private const float m_canvasRotation = 270.0f;

    void Update()
    {
        if(m_canvas == null)
        {
            return;
        }

        FixCanvasRotation();
    }

    /// <summary>
    /// キャンバスの角度をSpline移動に合わせて修正します。
    /// </summary>
    private void FixCanvasRotation()
    {
        var rotation = m_canvas.transform.rotation;
        rotation.x = m_canvasRotation - this.transform.rotation.x;
        m_canvas.transform.rotation = rotation;
    }
}
