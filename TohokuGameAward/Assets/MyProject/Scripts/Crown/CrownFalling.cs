using UnityEngine;

public class CrownFalling : MonoBehaviour
{
    [SerializeField]
    private float m_fallSpeed = 0.0f;

    [SerializeField]
    GameObject m_detectCollider = null;

    private Vector3 m_currentPos = Vector3.zero;

    private bool m_isFall = false;

    private Rigidbody m_rigidbody = null;

    void FixedUpdate()
    {
        if(m_isFall)
        {
            FallingCrown();
        }
    }

    private void FallingCrown()
    {
        m_currentPos = this.transform.position;
        m_currentPos.y -= m_fallSpeed * Time.deltaTime;
        this.transform.position = m_currentPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag(TagData.GetTag(TagData.Names.Ground)))
        {
            m_isFall = false;
        }
    }

    public void SetCrownFall()
    {
        //ドローン側でドローンがデストロイされるときに呼び出します。
        m_rigidbody = GetComponent<Rigidbody>();
        m_isFall = true;
    }

    public void SetActiveDetectCollider()
    {
        //ドローン側でドローンがデストロイされるときに呼び出します。
        m_detectCollider.SetActive(true);
    }
}
