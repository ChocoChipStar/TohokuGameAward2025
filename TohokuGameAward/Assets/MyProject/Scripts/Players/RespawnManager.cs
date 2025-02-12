using System;
using UniRx;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager m_playerManager = null;

    [SerializeField]
    private EffectManager m_effectManager = null;

    [SerializeField]
    private SoundEffectManager m_soundEffectManager = null;

    [SerializeField]
    private SurviveScoreManager m_surviveScoreManager = null;

    [SerializeField]
    private HumanoidData m_humanoidData = null;

    [SerializeField]
    private ScoreData m_scoreData = null;

    [SerializeField]
    private StageData m_stageData = null;

    private int m_deathHumanoidNum = 0;

    private float[] m_elapsedAfterDeathTime = new float[HumanoidManager.HumanoidMax];

    private HumanoidMover m_humanoidMover = null;

    private ReactiveProperty<bool>[] IsDead = new ReactiveProperty<bool>[HumanoidManager.HumanoidMax];

    private static readonly Vector3 CameraCenterPos = new Vector3(0.0f, 4.0f, 0.0f);
    private static readonly Vector3 DeathPlayerPos = new Vector3(0.0f, -100.0f, 0.0f);

    private void Awake()
    {
        for(int i = 0; i< (int)HumanoidManager.HumanoidMax; i++)
        {
            IsDead[i] = new ReactiveProperty<bool>(false);
            IsDead[i].Subscribe(msg => InitializeDeadPlayer()).AddTo(this);
        }
    }

    private void Update()
    {
        for(int humanoidNum = 0; humanoidNum < (int)HumanoidManager.HumanoidMax; humanoidNum++)
        {
            if (IsHumanoidOverDeathLine(humanoidNum) && !IsDead[humanoidNum].Value)
            {
                m_deathHumanoidNum = humanoidNum;
                IsDead[humanoidNum].Value = true;
            }

            if (!IsDead[humanoidNum].Value)
            {
                continue;
            }
            MeasureElapsedAfterDeathTime(humanoidNum);
        }
    }

    /// <summary>
    /// ヒューマノイドが死の境界線を越えているかを調べます
    /// </summary>
    private bool IsHumanoidOverDeathLine(int index)
    {
        var humanoidPos = m_playerManager.HumanoidInstances[index].transform.position;
        var deathLine = m_stageData.Border;

        var isOverLeftLine = humanoidPos.x < deathLine.Left;
        var isOverRightLine = humanoidPos.x > deathLine.Right;
        if (isOverRightLine || isOverLeftLine) // 両サイドどちらかのボーダーラインを越えていたら
        {
            return true;
        }

        var isOverTopLine = humanoidPos.y > deathLine.Top;
        var isOverBottomLine = humanoidPos.y < deathLine.Bottom;
        if (isOverTopLine || isOverBottomLine)
        {
            return true; // 上下どちらかのボーダーラインを越えていたら
        }
        return false;
    }

    /// <summary>
    /// 死んだヒューマノイドの死後経過時間を測る処理を行います
    /// </summary>
    private void MeasureElapsedAfterDeathTime(int humanoidNum)
    {
        m_elapsedAfterDeathTime[humanoidNum] += Time.deltaTime;
        if (m_elapsedAfterDeathTime[humanoidNum] > m_humanoidData.Params.RespawnTime)
        {
            InitializeRespawnedPlayer(humanoidNum);
        }
    }

    /// <summary>
    /// リスポーンしたプレイヤーの初期化処理を行います
    /// </summary>
    private void InitializeRespawnedPlayer(int humanoidNum)
    {
        // パラメータ初期化
        IsDead[humanoidNum].Value = false;
        m_elapsedAfterDeathTime[humanoidNum] = 0.0f;

        // 物理挙動有効化;
        m_humanoidMover.SetPhysicalOperable(true);

        // リスポーン地点にヒューマノイドの座標を移動
        m_playerManager.HumanoidInstances[humanoidNum].transform.position = m_humanoidData.Positions.RespawnPos[humanoidNum];

        // リスポーン後の無敵時間を開始
        var humanoidInvisible = m_playerManager.HumanoidInstances[humanoidNum].GetComponent<HumanoidInvincible>();
        humanoidInvisible.StartInvincible();

        //生きている時間で入るスコアをセットします。
        m_surviveScoreManager.OffIsDead(humanoidNum);
    }

    /// <summary>
    /// 撃墜されたプレイヤーの初期化処理を行います（エフェクト、サウンド再生）
    /// </summary>
    private void InitializeDeadPlayer()
    {
        if (!IsDead[m_deathHumanoidNum].Value)
        {
            return;
        }

        var deathHumanoid = m_playerManager.HumanoidInstances[m_deathHumanoidNum];
        m_humanoidMover = deathHumanoid.GetComponent<HumanoidMover>();
        m_humanoidMover.SetPhysicalOperable(false);

        // 死んだときのエフェクトを再生
        var humanoidPos = deathHumanoid.transform.position;
        m_effectManager.OnPlayEffect(humanoidPos, ConvertDirectionToAngle(CameraCenterPos - humanoidPos), EffectManager.Type.Death);

        // SE再生
        m_soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.Death);

        // 各チームのスコアの増減
        ScoreManager.Instance.UpdateScore(TeamGenerator.Instance.GetCurrentCannonTeamName(), m_scoreData.Params.HitHumanoidScore);
        ScoreManager.Instance.UpdateScore(TeamGenerator.Instance.GetCurrentHumanoidTeamName(), m_scoreData.Params.DeathScore);

        //生きている時間で入るスコアをリセットします。
        m_surviveScoreManager.SetIsDead(m_deathHumanoidNum);

        deathHumanoid.transform.position = DeathPlayerPos;
    }

    private float ConvertDirectionToAngle(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
