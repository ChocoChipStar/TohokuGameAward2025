using UnityEngine;

public class WallDetector : MonoBehaviour
{
    private bool m_isDetected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (TagManager.Instance.SearchedTagName(other.gameObject,TagManager.Type.Wall))
        {
            m_isDetected = true;
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (TagManager.Instance.SearchedTagName(other.gameObject, TagManager.Type.Wall))
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
