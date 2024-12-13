using UnityEngine;

public class CrownFalling : MonoBehaviour
{
    [SerializeField]
    private float m_fallSpeed = 0.0f;

    [SerializeField]
    private GameObject m_detectCollider = null;

    private Vector3 m_currentPos = Vector3.zero;

    private bool m_isFall = false;

    private Rigidbody m_rigidBody = null;

    private void FixedUpdate()
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
        if (TagManager.Instance.SearchedTagName(other.gameObject, TagManager.Type.Ground))
        {
            m_isFall = false;
        }
    }

    public void SetCrownFall()
    {
        //ドローン側でドローンがデストロイされるときに呼び出します。
        m_rigidBody = GetComponent<Rigidbody>();
        m_isFall = true;
    }

    public void SetActiveDetectCollider()
    {
        //ドローン側でドローンがデストロイされるときに呼び出します。
        m_detectCollider.SetActive(true);
    }
}
