using System.Collections.Generic;
using UnityEngine;

public class BombColliderScaleChange : MonoBehaviour
{
    [SerializeField]
    private ExplosionData m_explosionData = null;

    [SerializeField]
    private GameObject m_explosionEffect = null;

    [SerializeField]
    private Collider m_bombCollider = null;

    [SerializeField]
    private Rigidbody m_bombRigidbody = null;

    [SerializeField]
    private SkinnedMeshRenderer m_bombMeshRenderer = null;

    [SerializeField]
    private bool m_isShoot = false;

    [SerializeField]
    private bool m_isScaleChange = false;

    private const float m_scaleConstant = 0.1f;

    private List<BoxFlyController> m_boxController = new List<BoxFlyController>();

    private List<ObjectShake> m_objectShake =new List<ObjectShake>();

    private void Update()
    {
        if (m_isShoot)
        {
            ScaleChange();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            BoxInExplosion(other.gameObject);
        }
        var parent = other.gameObject.transform.gameObject;

        var HumanoidMover = parent.GetComponentInParent<HumanoidMover>();
        if (HumanoidMover == null)
        {
            return;
        }

        var HumanoidBlow = parent.GetComponentInParent<HumanoidBlow>();
        HumanoidBlow.InitializeStartBlow(transform.position, HumanoidMover);
    }

    private void StartEffect()
    {
        m_isShoot = true;
        m_isScaleChange = true;
        m_bombCollider.isTrigger = true;
        m_bombRigidbody.useGravity = false;
        m_bombRigidbody.velocity = Vector3.zero;
        m_bombMeshRenderer.enabled = false;
        this.transform.localScale = new Vector3(m_explosionData.Effect.ScaleMin, m_explosionData.Effect.ScaleMin, m_scaleConstant);
        var bomb = Instantiate(m_explosionEffect, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// 何かにぶつかると徐々に大きくなり消える。
    /// </summary>
    private void ScaleChange()
    {
        if (m_isScaleChange)
        {
            var scale = transform.localScale.x;
            scale += (m_explosionData.Effect.ScaleMax / m_explosionData.Effect.ScaleChangeTime) * Time.deltaTime;
            this.transform.localScale = new Vector3(scale, scale, m_scaleConstant);

            if(scale >= m_explosionData.Effect.ScaleMax)
            {
                m_isScaleChange = false;
            }
        }
        else
        {
           ExplosionDestructor();
           Destroy(this.gameObject, m_explosionData.Effect.ScaleMaxTime);
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
