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

    // インスペクターでジャンルを指定する
    public BombGenre bombGenre;

    [Header("爆発までの時間[s]")]
    public float time = 3.0f;

    [Header("爆風に当たったときに吹っ飛ぶ力の強さ")]
    public float power = 1;

    [Header("爆発の当たる範囲")]
    public float size = 2;

    [Header("回転の中心点")]
    public float pivot = -1;

    [Header("爆弾の数")]
    public int count = 0;

    [Header("爆弾が複数の場合、出したい爆弾")]
    public GameObject bomb;
}