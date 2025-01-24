using UnityEngine;

public class CannonMaterials : MonoBehaviour
{
    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private Material[] m_CannonBaseMaterial = new Material[InputData.PlayerMax];

    [SerializeField]
    private Material[] m_CannonCleannessMaterial = new Material[InputData.PlayerMax];

    [SerializeField]
    private Material[] m_CannonPlayerMaterial = new Material[InputData.PlayerMax];

    [SerializeField]
    private SkinnedMeshRenderer[] m_cannonBaseMesh = new SkinnedMeshRenderer[BaseMeshMax];

    [SerializeField]
    private SkinnedMeshRenderer[] m_cannonCleannessMesh = new SkinnedMeshRenderer[CleannessMeshMax];

    [SerializeField]
    private SkinnedMeshRenderer[] m_cannonHumanoidMesh = new SkinnedMeshRenderer[HumanoidMeshMax];

    private const int BaseMeshMax = 3;
    private const int CleannessMeshMax = 1;
    private const int HumanoidMeshMax = 6;


    private void Start()
    {
        SetBaseMaterial(m_inputData.SelfNumber);
        SetCleannessMaterial(m_inputData.SelfNumber);
        SetPlayerMaterial(m_inputData.SelfNumber);
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
        for (int i = 0; i < m_cannonHumanoidMesh.Length; i++)
        {
            m_cannonHumanoidMesh[i].material = m_CannonPlayerMaterial[playerNum];
        }
    }
}
