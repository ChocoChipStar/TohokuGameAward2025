using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerPrefab = null;

    private const int PlayerMax = 4;

    private static readonly float[] StartPos = new float[] { -4.5f, -1.5f, 1.5f, 4.5f };

    private void Awake()
    {
        for (int i = 0; i < PlayerMax; i++)
        {
            var instance = Instantiate(m_playerPrefab, new Vector3(StartPos[i], 0.0f, 0.0f), Quaternion.identity);

            instance.name = "Player" + (i + 1);
            instance.transform.SetParent(this.transform);

            var selfData = instance.gameObject.GetComponent<SelfData>();
            if(selfData != null)
            {
                selfData.SetNumber(i);
            }
        }
    }
}
