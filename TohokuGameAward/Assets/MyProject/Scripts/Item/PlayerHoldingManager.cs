using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_item;      //éËéùÇøÉAÉCÉeÉÄ

    private Bomb m_bomb;
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
            m_bomb.ThrowBomb();
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
            m_bomb = other.gameObject.GetComponent<Bomb>();

            if(!m_bomb.isThrown)
            {
                m_item = other.gameObject;
            }

        }
    }
}
