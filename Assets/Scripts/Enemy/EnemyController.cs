using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Image HPBar;

    EnemyData data;

    LineSample lineSample;

    public event System.Action Defeated;
    private bool isDefeated;


    private EnemyActionRunner actionRunner;

    private EnemyHealth health;

    public int positionX => actionRunner.PositionX;

    public void Init(LineSample lineSample, EnemyData enemyData, int nextWaveId)
    {
        data = enemyData;
        this.lineSample = lineSample;

        actionRunner = new EnemyActionRunner(
            data.CreateWaveList(),
            Constants.Division - 1,
            nextWaveId);

        transform.localPosition = new Vector3(-Constants.Width / 2 + positionX * Constants.Width / Constants.Division, 0, 0);

        health = new EnemyHealth(data.maxHp, Constants.Width / Constants.Division);

        isDefeated = false;
        HPBar.fillAmount = health.HpRatio;

        this.lineSample.WaveMoved += OnWaveMoved;
    }

    private void OnWaveMoved()
    {
        if (!GameStateManager.Instance.IsPlaying || isDefeated)
        {
            return;
        }

        // 行動結果を見た目に反映する
        transform.localPosition = new Vector3(
            -Constants.Width / 2
                + positionX * Constants.Width / Constants.Division,
            actionRunner.currentDisplacement,
            0);

        // 敵の位置に届いた波を読み取り、消費する
        float wavePower =
            lineSample.playerMedium.wavePowers[positionX];

        lineSample.playerMedium.DestroyOneDivision(positionX);

        // ダメージ計算を任せ、結果を表示する
        health.ApplyWave(wavePower);
        HPBar.fillAmount = health.HpRatio;

        if (health.IsDead)
        {
            Defeat();
        }
    }
    private void Defeat()
    {
        if (isDefeated) return;

        // 通知より先に設定し、二重通知を防ぐ
        isDefeated = true;

        Defeated?.Invoke();
        Destroy(gameObject);
    }



    public EnemyActionResult WaveMove()
    {

        // 撃破済みの敵は波を出さない
        if (isDefeated)
        {
            return EnemyActionResult.NoWave(positionX);
        }

        return actionRunner.Step();
    }


    private void OnDestroy()
    {
        if (lineSample != null)
        {
            lineSample.WaveMoved -= OnWaveMoved;
            lineSample.UnregisterEnemy(this);
        }

    }
}
