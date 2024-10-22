using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_item;      //手持ちアイテム

    private BombManager m_bombManager;
    private BombController m_bombController;

    void Start()
    {
        m_item = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_item == null) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_bombManager.ThrowBomb();
            m_item = null;
        }
        
        if(m_item != null)
        {
            m_item.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_item != null) return;

        if (other.gameObject.tag == "Item")
        {
            m_bombManager = other.gameObject.GetComponent<BombManager>();

            //一度投げられた爆弾でなければ
            if (!m_bombManager.isThrown)
            {
                m_item = other.gameObject;
                m_bombManager.FuseOn();
            }
        }
    }
}
