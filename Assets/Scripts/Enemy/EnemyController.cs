using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Image HPBar;

    EnemyData data;


    public event System.Action Defeated;
    private bool isDefeated;
    public bool IsDefeated => isDefeated;


    private EnemyActionRunner actionRunner;

    private EnemyHealth health;

    public int positionX => actionRunner.PositionX;

    public void Init( EnemyData enemyData, int nextWaveId)
    {
        data = enemyData;

        actionRunner = new EnemyActionRunner(
            data.CreateWaveList(),
            Constants.Division - 1,
            nextWaveId);

        transform.localPosition = new Vector3(-Constants.Width / 2 + positionX * Constants.Width / Constants.Division, 0, 0);

        health = new EnemyHealth(data.maxHp, Constants.Width / Constants.Division);

        isDefeated = false;
        HPBar.fillAmount = health.HpRatio;
    }

    public void ApplyWaveHit(float wavePower)
    {
        if (isDefeated) return;

        // 今回の行動結果を表示する
        transform.localPosition = new Vector3(
            -Constants.Width / 2
                + positionX * Constants.Width / Constants.Division,
            actionRunner.currentDisplacement,
            0);

        // 渡された波の値を使ってダメージを計算する
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

}
