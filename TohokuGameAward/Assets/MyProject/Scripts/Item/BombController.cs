using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_bombRigidbody;

    [Header("投げる力")]
    [SerializeField]
    public float m_throwForce = 1f;
    [Header("斜め投げの角度")]
    [SerializeField]
    public float m_throwAngle = 45f;
    [Header("回転速度")]
    [SerializeField]
    public float m_rotationSpeed = 10f;


    public void Throw()
    {
        m_bombRigidbody.velocity = Vector3.zero;
        m_bombRigidbody.rotation = Quaternion.identity;
        // 斜め上に投射するための方向ベクトルを作成
        float radianAngle = m_throwAngle * Mathf.Deg2Rad; // 角度をラジアンに変換
        Vector3 throwDirection = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0); // 斜め方向

        // 爆弾を斜め上に投げる
        m_bombRigidbody.AddForce(throwDirection * m_throwForce, ForceMode.Impulse);
    }

    public void Rowling()
    {
        // Rigidbodyの速度を取得
        Vector3 velocity = m_bombRigidbody.velocity;

        // 移動速度に基づいて回転を計算
        float speed = velocity.magnitude;

        // z軸の回転を速度に応じて設定
        float rotationAmount = speed * m_rotationSpeed * Time.deltaTime;

        // 現在の回転に加算
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z - rotationAmount);
    }
}

