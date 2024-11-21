using UnityEngine;

public class WallDetector : MonoBehaviour
{
    private bool m_isDetected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagData.GetTag(TagData.Names.Wall)))
        {
            m_isDetected = true;
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TagData.GetTag(TagData.Names.Wall)))
        {
            m_isDetected = false;
        }
    }

    public bool IsHitWall()
    {
        if(m_isDetected)
        {
            return true;
        }

        return false;
    }
}
