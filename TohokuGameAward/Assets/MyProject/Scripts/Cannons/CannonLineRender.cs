using UnityEngine;

public class CannonLineRender : MonoBehaviour
{
    [SerializeField]
    private GameObject m_bombPrefab = null;

    [SerializeField]
    private CannonBombShoot m_cannonBombShoot = null;

    [SerializeField]
    private LineRenderer m_lineRenderer = null;

    [SerializeField]
    private int m_lineRenderCount = 0;

    [SerializeField]
    private Vector3 m_shootVelocity = Vector3.zero;

    [SerializeField]
    private Vector3 m_lineStartPosition = Vector3.zero;



    private float m_lineLength = 2.0f;

    private float m_moveSpeed = 10.0f;

    private bool m_isRenderShootLine = false;

    [SerializeField]
    private Vector3 m_moveVector = Vector3.zero;

    private void Start()
    {
        m_moveVector = transform.position;
    }

    private void Update()
    {
        CubeMover();

        if (Input.GetKey(KeyCode.Space))
        {
            GetRenderParabola();
        }
    }

    private void GetRenderParabola()
    {
        m_shootVelocity = m_cannonBombShoot.ShootVelocity;
        m_lineStartPosition = m_cannonBombShoot.ShootInitialPosition;
        m_lineRenderer.positionCount = m_lineRenderCount;

        float timeStep = m_lineLength / m_lineRenderCount;
        bool draw = false;
        float hitTime = short.MaxValue;
        for (int i = 0; i < m_lineRenderCount; i++)
        {
            // 線の座標を更新
            float renderTime = timeStep * i;
            float endTime = renderTime + timeStep;
            Debug.Log(renderTime);
            Debug.Log(endTime);
            SetLineRendererPosition(i, renderTime);

            // 衝突判定
            if (!draw)
            {
                hitTime = GetArcHitTime(renderTime, endTime);
                if (hitTime != short.MaxValue)
                {
                    draw = true; // 衝突したらその先の放物線は表示しない
                }
            }
        }
    }

    private void SetLineRendererPosition(int index, float renderTime)
    {
        m_lineRenderer.SetPosition(index, GetRenderPointPosition(renderTime));
    }

    private float GetArcHitTime(float renderTime, float endTime)
    {
        // Linecastする線分の始終点の座標
        Vector3 startPosition = GetRenderPointPosition(renderTime);
        Vector3 endPosition = GetRenderPointPosition(endTime);

        // 衝突判定
        RaycastHit hitInfo;
        if (Physics.Linecast(startPosition, endPosition, out hitInfo))
        {
            // 衝突したColliderまでの距離から実際の衝突時間を算出
            float distance = Vector3.Distance(startPosition, endPosition);
            Debug.Log(distance);
            return 0;
            //return renderTime + (endTime - renderTime) * (hitInfo.distance / distance);
        }
        return short.MaxValue;
    }

    private Vector3 GetRenderPointPosition(float time)
    {
        return (m_lineStartPosition + ((m_shootVelocity * time) + (0.5f * time * time) * Physics.gravity));
    }

    private void CubeMover()
    {
        if (Input.GetKey(KeyCode.W))
        {
            m_moveVector.y += m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_moveVector.x -= m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_moveVector.y += -m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_moveVector.x += m_moveSpeed * Time.deltaTime;
        }
        this.transform.position = m_moveVector;
    }
}
