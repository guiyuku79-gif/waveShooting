using UnityEngine;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
    [SerializeField] Image HPBar;

    LineSample lineSample;

    float maxHp = 5;
    float hp;

    float priviousWavePower;
    float priviousWaveDuration;

    public void Init(LineSample lineSample, int lane)
    {
        this.lineSample = lineSample;
        hp = maxHp;
        transform.position = new Vector3(Constants.Width / 2, Constants.laneYs[lane], 0);

        this.lineSample.WaveMoved += OnWaveMoved;

        this.priviousWavePower = 0;
        this.priviousWaveDuration = 0;

        //仮
        lineSample.enemyMedium.MakeTriangleWave(2, 1, 0.5f);
        lineSample.enemyMedium.MakeWait(2);
        lineSample.enemyMedium.MakeTriangleWave(2, 1, 0.5f);
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
