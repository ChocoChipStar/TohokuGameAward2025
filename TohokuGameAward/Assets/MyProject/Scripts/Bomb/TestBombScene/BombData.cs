using System.Runtime.CompilerServices;
using UnityEngine;

// 爆弾のデータをScriptableObjectで管理する
[CreateAssetMenu(fileName = "NewBombData", menuName = "ScriptableObjects/BombData", order = 1)]
public class BombData : ScriptableObject
{
    // 爆弾のジャンルを定義する列挙型
    public enum BombGenre
    {
        [InspectorName("ノーマル")]
        Normal,
        [InspectorName("インパルス")]
        Impulse,
        [InspectorName("ミニ")]
        Mini
    }

    [SerializeField]
    private BombGenre m_bombType;

    [SerializeField, Header("爆発までの時間[s]")]
    private float m_explosionTime = 0.0f;

    [SerializeField, Header("爆風に当たったときに吹っ飛ぶ力の強さ")]
    private float m_blastPower = 0.0f;

    [SerializeField, Header("爆弾の爆風範囲")]
    private float m_blastRange = 0.0f;

    [SerializeField, Header("回転の中心点")]
    private float m_bombPivot = -1.0f;

    [SerializeField, Header("爆弾の数")]
    private int m_bombCount = 0;

    [SerializeField, Header("爆弾が複数の場合、出したい爆弾")]
    private GameObject m_otherBombObj = null;

    public BombGenre BombType { get { return m_bombType; } private set { value = m_bombType; } }
    public float ExplosionTime { get { return m_explosionTime; } private set { value = m_explosionTime; } }
    public float BlastPower { get { return m_blastPower; } private set { value = m_blastPower; } }
    public float BlastRange { get { return m_blastRange; } private set { value = m_blastRange; } }
    public float BombPivot { get { return m_bombPivot; } private set { value = m_bombPivot; } }
    public int BombCount { get { return m_bombCount; } private set { value = m_bombCount; } }
    public GameObject OtherBombObj { get { return m_otherBombObj; } private set { value = m_otherBombObj; } }
}