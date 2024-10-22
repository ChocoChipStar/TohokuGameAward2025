using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageThreeGimmickManager : MonoBehaviour
{
    [SerializeField]
    float m_speedOfRotate = 0.1f;

    private void FixedUpdate()
    {
        this.transform.Rotate(0, 0, m_speedOfRotate);
    }
}
