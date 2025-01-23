using UnityEngine;

public class HumanoidMaterials : MonoBehaviour
{
    [SerializeField]
    private InputData m_inputData = null;

    [SerializeField]
    private Material[] m_playerMaterial = new Material[InputData.PlayerMax];

    [SerializeField]
    private SkinnedMeshRenderer[] m_playerMesh = new SkinnedMeshRenderer[HumanoidMeshMax];

    private int m_playerMeshCount = 0;
    private const int HumanoidMeshMax = 6;

    private void Start()
    {
        m_playerMeshCount = m_playerMesh.Length;
        SetMaterial(m_inputData.SelfNumber);
    }

    private void SetMaterial(int playerNum)
    {
        for(int i = 0; i < m_playerMeshCount; i++)
        {
            m_playerMesh[i].material = m_playerMaterial[playerNum];
        }
    }

    public void SwitchMaterial()
    {
        for (int i = 0; i < m_playerMeshCount; i++)
        {
            m_playerMesh[i].enabled = !m_playerMesh[i].enabled;
        }
    }

    public void FinishInvincible()
    {
        for (int i = 0; i < m_playerMeshCount; i++)
        {
            m_playerMesh[i].enabled = true;
        }
    }
}