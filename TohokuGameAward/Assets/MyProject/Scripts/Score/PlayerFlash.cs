using System.Collections.Generic;
using UnityEngine;

public class PlayerFlash : MonoBehaviour
{
    [SerializeField]
    private SurviveScoreManager m_surviveScoreManager = null;

    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private GameObject[] m_particlePrefabs = null;

    [SerializeField]
    private Vector3 m_offsetPos = Vector3.zero;

    private GameObject[] m_humanoid = new GameObject[HumanoidManager.HumanoidMax];

    private List<GameObject> m_spawnedParticle_one = new List<GameObject>();

    private List<GameObject> m_spawnedParticle_two = new List<GameObject>();

    private void Start()
    {
        m_humanoid = m_playerManager.HumanoidInstances;
    }

    public void OnPerticle(int HumanoidNum,int scoreLevel)
    {
        Transform parentTransform = m_humanoid[HumanoidNum].transform;

        GameObject particle = Instantiate(m_particlePrefabs[scoreLevel], parentTransform.position + m_offsetPos, Quaternion.identity, parentTransform);

        var particleSystem = particle.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        //プレイヤーごとに分けてパーティクルを保存します。
        if(HumanoidNum == 0)
        {
            m_spawnedParticle_one.Add(particle);
        }
        else
        {
            m_spawnedParticle_two.Add(particle);
        }
    }

    public void OffPerticle(int HumanoidNum)
    {
        if (m_spawnedParticle_one == null)
        {
            return;
        }


        if (HumanoidNum == 0)
        {
            for (int i = 0; i < m_spawnedParticle_one.Count; i++)
            {
                Destroy(m_spawnedParticle_one[i]);

            }
            m_spawnedParticle_one.Clear();
        }
        else
        {
            for (int i = 0; i < m_spawnedParticle_two.Count; i++)
            {
                Destroy(m_spawnedParticle_two[i]);

            }
            m_spawnedParticle_two.Clear();
        }
    }
}