using UnityEngine;

public class PlayerMaterials : MonoBehaviour
{
    [SerializeField]
    private Material[] m_playerMaterial = new Material[4];

    [SerializeField]
    private SkinnedMeshRenderer[] m_playerMesh = new SkinnedMeshRenderer[6];

    private int m_playerCount = 0;
    private int m_playerMeshCount = 0;

    private static readonly string PlayerName = "Player";

    private void Start()
    {
        m_playerCount = m_playerMaterial.Length;
        m_playerMeshCount = m_playerMesh.Length;

        for (int i = 0; i < m_playerCount; i++)
        {
            var playerName = PlayerName + (i + 1);
            if (this.gameObject.name == playerName)
            {
                SetMaterial(i);
            }
        }
    }

    private void SetMaterial(int playerNum)
    {
        for(int i = 0; i < m_playerMeshCount; i++)
        {
            m_playerMesh[i].material = m_playerMaterial[playerNum];
        }
    }

    public void PlayerInvincibleMesh()
    {
        for (int i = 0; i < m_playerMeshCount; i++)
        {
            m_playerMesh[i].enabled = !m_playerMesh[i].enabled;
        }
    }

    public void PlayerInvincibleMeshEnd()
    {
        for (int i = 0; i < m_playerMeshCount; i++)
        {
            m_playerMesh[i].enabled = true;
        }
    }
}
