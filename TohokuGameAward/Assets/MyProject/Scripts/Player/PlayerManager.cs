using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private static int m_selfNumber = 0;

    public static int SelfNumber => m_selfNumber;
}
