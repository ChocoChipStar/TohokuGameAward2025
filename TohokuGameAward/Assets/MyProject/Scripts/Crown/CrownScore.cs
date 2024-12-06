using UnityEngine;

public class CrownScore : MonoBehaviour
{
    [SerializeField]
    private int m_crownScore = 0;

    public int GetScore()
    {
        return m_crownScore;
    }
}
