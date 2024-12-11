using UnityEngine;

public class DroneDestroy : MonoBehaviour
{
    private bool m_isDestroy = false;

    private GameObject m_crown = null;

    private Rigidbody m_crownRigidbody = null;

    private void Update()
    {
        if(m_isDestroy)
        {
            LeaveCrown();
        }
    }

    private void LeaveCrown()
    {
        foreach(Transform child in this.transform)
        {
            if (child.CompareTag(TagData.GetTag(TagData.Names.Crown)))
            { 
                m_crown = child.gameObject; 
                break;
            }
        }

        if (m_crown != null)
        {
            var crown = m_crown.gameObject.GetComponent<CrownFalling>();
            crown.SetCrownFall();
            crown.SetActiveDetectCollider();
     
            // 子オブジェクトを親から切り離す
            m_crown.transform.SetParent(null);
        }

        Destroy(this.gameObject);
    }

    public void SetDestroy()
    {
        //ドローンが爆弾か爆風に当たった時に爆弾側で呼び出します。
        m_isDestroy = true;
    }
}
