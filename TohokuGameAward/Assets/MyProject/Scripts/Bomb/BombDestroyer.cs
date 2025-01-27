using UnityEngine;

public class BombDestroyer : MonoBehaviour
{
    public void DestoroyBomb()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
