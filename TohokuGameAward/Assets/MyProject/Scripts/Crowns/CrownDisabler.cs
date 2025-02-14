using UniRx;
using UnityEngine;

public class CrownDisabler : MonoBehaviour
{
    [SerializeField]
    GameTimer m_gameTimer = null;

    private bool m_isCantCrownGet = false;

    public bool IsCantCrownGet { get { return m_isCantCrownGet; } }

    void Start()
    {
        //Whereはifとほぼ同じで、Where()の()内がtrueの時だけ続きの処理に進みます。
        //Subscribe(_ =>{m_isCantCrownGet = true;})の_は「引数を使わないラムダ式」という意味です。
        m_gameTimer.IsTimeLimit.Where(value => value).Subscribe(_ =>{m_isCantCrownGet = true;}).AddTo(this);
    }

}
