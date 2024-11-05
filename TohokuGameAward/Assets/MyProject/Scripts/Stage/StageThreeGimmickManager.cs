using UnityEngine;

public class StageThreeGimmickManager : MonoBehaviour
{
    [SerializeField]
    float m_speedOfRotate = 0.0f;

    private void FixedUpdate()
    {
        RotateStage();
    }

    private void RotateStage()
    {
        this.transform.Rotate(0, 0, m_speedOfRotate);
    }
}
