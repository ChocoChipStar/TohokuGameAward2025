using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGravity : MonoBehaviour
{

    private Rigidbody m_itemRb;
    

    // Start is called before the first frame update
    void Start()
    {
        m_itemRb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 itemTransform = this.transform.position;
        itemTransform.y -= 0.1f;
        this.transform.position = itemTransform;
    }
    void OnCollisionEnter(Collision collision)
    {

        // èdóÕÇóLå¯Ç…Ç∑ÇÈ
        m_itemRb.useGravity = true;
        
       
    }
}
