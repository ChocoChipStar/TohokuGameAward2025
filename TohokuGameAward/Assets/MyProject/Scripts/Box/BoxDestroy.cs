using UnityEngine;

public class BoxDestroy : MonoBehaviour
{
    [SerializeField]
    private StageData m_stageData = null;

    private BoxFactory m_boxFactory = null;

    private int m_thisIndex = 0;

    private void Start()
    {
        m_boxFactory = GetComponentInParent<BoxFactory>();
    }

    void Update()
    {
        if(IsOverBorderLine())
        {
            m_boxFactory.SetRespawnDelay(m_thisIndex);
            DestroyBox();
        }
    }

    public void SetBoxIndex(int i)
    {
        m_thisIndex = i;
    }

    private void DestroyBox()
    {
        Destroy(this.gameObject);
    }

    private bool IsOverBorderLine()
    {
        var targetObj = this.gameObject;

        var isTargetOverLeftBorder = targetObj.transform.position.x < m_stageData.Border.Left;
        var isTargetOverRightBorder = targetObj.transform.position.x > m_stageData.Border.Right;
        if (isTargetOverLeftBorder || isTargetOverRightBorder)
        {
            // 両サイドのボーダーラインを越えていたら
            return true;
        }

        var isTargetOverBottomBorder = targetObj.transform.position.y < m_stageData.Border.Bottom;
        var isTargetOverTopBorder = targetObj.transform.position.y > m_stageData.Border.Top;
        if (isTargetOverBottomBorder || isTargetOverTopBorder)
        {
            // 上下のボーダーラインを越えていたら
            return true;
        }

        return false;
    }
}
