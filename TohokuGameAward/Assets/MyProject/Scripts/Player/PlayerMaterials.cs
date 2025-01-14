using UnityEngine;

public class PlayerMaterials : MonoBehaviour
{
    [SerializeField]
    private Material[] m_playerMaterial = new Material[4];

    [SerializeField]
    private SkinnedMeshRenderer[] m_playerMesh = new SkinnedMeshRenderer[6];

    private int m_playerCount = 0;

    private int m_playerMeshCount = 0;

    private string[] m_playerName = {"Player1", "Player2", "Player3", "Player4" };

    private void Start()
    {
        m_playerCount = m_playerMaterial.Length;
        m_playerMeshCount = m_playerMesh.Length;

        for (int i = 0; i < m_playerCount; i++)
        if(this.gameObject.name == m_playerName[i])
            {
                SetMaterrial(i);
            }
    }

    private void SetMaterrial(int playerNum)
    {
        for(int i = 0; i < m_playerMeshCount; i++)
        {
            m_playerMesh[i].material = m_playerMaterial[playerNum];
        }
    }
}
