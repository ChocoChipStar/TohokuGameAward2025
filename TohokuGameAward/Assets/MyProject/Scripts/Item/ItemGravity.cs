using UnityEngine;

public class ItemGravity : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_itemRb = null;

    [SerializeField]
    private float m_fallingSpeed = 0.1f;

    private void FixedUpdate()
    {
        Vector3 itemTransform = this.transform.position;
        itemTransform.y -= m_fallingSpeed;
        this.transform.position = itemTransform;
    }
    void OnCollisionEnter(Collision collision)
    {
        m_itemRb.useGravity = true;
    }
}
