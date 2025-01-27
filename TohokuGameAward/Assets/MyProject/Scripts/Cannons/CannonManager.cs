﻿using Unity.VisualScripting;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField]
    private CannonData m_cannonData = null;

    [SerializeField]
    private GameObject m_bombManager = null;

    [SerializeField]
    private GameObject m_bombPrefab = null;

    [SerializeField]
    private CannonDistance m_distanceManager = null;

    [SerializeField]
    private SoundEffectManager m_soundEffectManager = null;

    private int m_cannonCount = 0;
    public static readonly int CannonMax = 2;

    public GameObject GenerateCannon(GameObject cannon)
    {
        var cannonObject = Instantiate(cannon, Vector3.zero, Quaternion.identity, this.transform);
        InitializeCannon(cannonObject);
        return cannonObject;
    }

    private void InitializeCannon(GameObject cannon)
    {
        CannonMover[] cannonMover = new CannonMover[CannonMax];
        cannonMover[m_cannonCount] = cannon.GetComponent<CannonMover>();
        cannonMover[m_cannonCount].InitializeSpline();
        cannonMover[m_cannonCount].InitializePosition(m_cannonCount);
        m_distanceManager.GetCannonMover(cannonMover[m_cannonCount], m_cannonCount);
        m_cannonCount++;
    }

    public void GenerateBomb(Vector3 position,Vector3 force)
    {
        var bomb = Instantiate(m_bombPrefab,position, Quaternion.identity, m_bombManager.transform);
        var bombRigidbody = bomb.GetComponent<Rigidbody>();
        bombRigidbody.AddForce(force * m_cannonData.Params.ShootSpeed, ForceMode.Impulse);
    }

    public void PlaySoundEffect()
    {
        m_soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.Cannon);
    }
}
