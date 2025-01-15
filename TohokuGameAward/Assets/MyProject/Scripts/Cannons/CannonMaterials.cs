using UnityEngine;

public class CannonMaterials : MonoBehaviour
{
    [SerializeField]
    private Material[] m_CannonBaseMaterial = new Material[4];

    [SerializeField]
    private Material[] m_CannonCleannessMaterial = new Material[4];

    [SerializeField]
    private Material[] m_CannonPlayerMaterial = new Material[4];

    [SerializeField]
    private SkinnedMeshRenderer[] m_cannonBaseMesh = new SkinnedMeshRenderer[3];

    [SerializeField]
    private SkinnedMeshRenderer[] m_cannonCleannessMesh = new SkinnedMeshRenderer[1];

    [SerializeField]
    private SkinnedMeshRenderer[] m_cannonPlayerMesh = new SkinnedMeshRenderer[6];


    private static readonly string PlayerName = "Player";

    private void Start()
    {
        for (int i = 0; i < m_CannonBaseMaterial.Length; i++)
        {
            var playerName = PlayerName + (i + 1);
            if (this.gameObject.name == playerName)
            {
                SetBaseMaterial(i);
                SetCleannessMaterial(i);
                SetPlayerMaterial(i);
            }
        }
    }

    private void SetBaseMaterial(int playerNum)
    {
        for (int i = 0; i < m_cannonBaseMesh.Length; i++)
        {
            m_cannonBaseMesh[i].material = m_CannonBaseMaterial[playerNum];
        }
    }

    private void SetCleannessMaterial(int playerNum)
    {
        for (int i = 0; i < m_cannonCleannessMesh.Length; i++)
        {
            m_cannonCleannessMesh[i].material = m_CannonCleannessMaterial[playerNum];
        }
    }

    private void SetPlayerMaterial(int playerNum)
    {
        for (int i = 0; i < m_cannonPlayerMesh.Length; i++)
        {
            m_cannonPlayerMesh[i].material = m_CannonPlayerMaterial[playerNum];
        }
    }
}
