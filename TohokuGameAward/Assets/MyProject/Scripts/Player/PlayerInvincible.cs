using UnityEngine;

public class PlayerInvincible : MonoBehaviour
{
    [SerializeField]
    private PlayerMaterials m_materials = null;

    [SerializeField]
    private PlayerData m_playerData = null;

    private float m_invincibleTime = 0.0f;
    private float m_materialsTime = 0.0f;

    private bool m_isInvincible = false;

    public bool IsInvincible {  get { return m_isInvincible; } }

    private void Update()
    {
        if (!m_isInvincible)
        {
            return;
        }

        if (m_invincibleTime < 0.0f)
        {
            PlayerInvincibleEnd();
        }

        m_invincibleTime -= Time.deltaTime;
        m_materialsTime += Time.deltaTime * m_playerData.Params.InvinceibleSpeed;
        if ((int)m_materialsTime % 2 == 1)
        {
            m_materialsTime = 0.0f;
            m_materials.PlayerInvincibleMesh();
        }
    }


    public void PlayerInvincibleTime()
    {
        m_isInvincible = true;
        m_invincibleTime = m_playerData.Params.InvinceibleTime;
        m_materialsTime = 0.0f;
    }

    private void PlayerInvincibleEnd()
    {
        m_materials.PlayerInvincibleMeshEnd();
        m_isInvincible = true;
    }
}
