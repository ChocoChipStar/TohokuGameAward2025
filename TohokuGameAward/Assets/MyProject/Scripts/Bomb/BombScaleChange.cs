using System.Collections.Generic;
using UnityEngine;

public class BombScaleChange : MonoBehaviour
{
   // [SerializeField]
   // private BombData m_bombData = null;

    [SerializeField,Header("通常サイズ")]
    private float m_scaleMin = 0.0f;

    [SerializeField,Header("最大サイズ")]
    private float m_scaleMax = 0.0f;

    [SerializeField,Header("最大サイズまで大きくなる時間")]
    private float m_scaleChangeTime = 0.0f;

    [SerializeField, Header("最大サイズを維持する時間")]
    private float m_scaleMaxTime = 0.0f;

    private const float m_scaleConstant = 0.1f;

    [SerializeField]
    private bool m_isShoot = false;
    [SerializeField]
    private bool m_isScaleChange = false;

    [SerializeField]
    BombData m_bombData = null;

    private List<BoxFlyController> m_boxController = new List<BoxFlyController>();

    private List<ObjectShake> m_objectShake =new List<ObjectShake>();

    private void Start()
    {
        this.transform.localScale = new Vector3(m_scaleMin, m_scaleMin, m_scaleConstant);
    }

    private void Update()
    {
        if (m_isShoot)
        {
            ScaleChange();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Box"))
        {
            m_isShoot = true;
            m_isScaleChange = true;

            BoxInExplosion(collider.gameObject);
        }

        var parent = collider.gameObject.transform.parent.gameObject;

        var rigidbody = parent.GetComponentInParent<Rigidbody>();
        if (rigidbody == null)
        {
            return;
        }

        var playerMover = parent.GetComponentInParent<HumanoidMover>();
        if (playerMover == null)
        {
            return;
        }

        m_isShoot = true;
        m_isScaleChange = true;

        var blowMover = parent.GetComponentInParent<BlowMover>();
        blowMover.BlowOfTarget(rigidbody, transform.position, collider, m_bombData, playerMover);
    }

    /// <summary>
    /// 何かにぶつかると徐々に大きくなり消える。
    /// </summary>
    private void ScaleChange()
    {
        if (m_isScaleChange)
        {
            var scale = transform.localScale.x;
            scale += (m_scaleMax / m_scaleChangeTime) * Time.deltaTime;
            this.transform.localScale = new Vector3(scale, scale, m_scaleConstant);

            if(scale >= m_scaleMax)
            {
                m_isScaleChange = false;
            }
        }
        else
        {
           ExplosionDestructor();
           Destroy(this.gameObject, m_scaleMaxTime);
        }
    }

    /// <summary>
    /// 爆発に振れた箱を振動させ、箱が飛ばされる方向の計算を行います。
    /// </summary>
    private void BoxInExplosion(GameObject box)
    {
        var boxController = box.gameObject.GetComponent<BoxFlyController>();
        var objShake = box.gameObject.GetComponent<ObjectShake>();

        boxController.CulculateFlyDirection(this.transform.position);
        objShake.SetShake();

        //Destroyと同時に振動の停止と吹き飛ばしの関数を呼ぶので、
        //コンポーネントをメンバ変数に保存しています。
        m_boxController.Add(boxController);
        m_objectShake.Add(objShake);
    }

    /// <summary>
    /// 爆発の終了に合わせて振動の停止処理と箱を飛ばす処理を行います。
    /// </summary>
    private void ExplosionDestructor()
    {
        foreach (var boxController in m_boxController)
        {
            if (boxController != null)
            {　 
                //箱を飛ばす
                boxController.setBoxFly();
            }
        }

        foreach (var objShake in m_objectShake)
        {
            if(objShake != null)
            { 　
                //振動の停止
                objShake.SetEndShake(); 
            }
        }
    }
}
