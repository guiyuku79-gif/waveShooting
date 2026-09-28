using UnityEngine;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
    [SerializeField] Image HPBar;

    EnemyData data;

    LineSample lineSample;

    float maxHp = 1f;
    float hp;

    float priviousWavePower;
    float priviousWaveDuration;

    public void Init(LineSample lineSample, EnemyData enemyData)
    {
        data = enemyData;
        this.lineSample = lineSample;
        transform.position = new Vector3(Constants.Width / 2, lineSample.playerMedium.laneY, 0);
        this.lineSample.WaveMoved += OnWaveMoved;

        this.priviousWavePower = 0;
        this.priviousWaveDuration = 0;


        //仮
        maxHp = data.maxHp;
        hp = maxHp;

        lineSample.enemyMedium.SetPattern(data);

    }
    private void OnWaveMoved()
    {
        float currentWavePower = lineSample.playerMedium.waveIds[^1];
        if (currentWavePower != 0)
        {
            if (priviousWavePower * currentWavePower <= 0)
            {
                priviousWaveDuration = 0;
            }
            priviousWavePower = currentWavePower;
            priviousWaveDuration += Constants.Width / Constants.Division;

            hp -= Mathf.Abs(currentWavePower) / (0.5f + priviousWaveDuration) * Constants.Width / Constants.Division;

            HPBar.fillAmount = hp / maxHp;
            if (hp <= 0)
            {
                lineSample.enemyMedium.nextEnemyWaveList.Clear();
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (lineSample != null)
            lineSample.WaveMoved -= OnWaveMoved;
    }
}
