using UnityEngine;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
    [SerializeField] Image HPBar;

    EnemyData data;

    LineSample lineSample;

    float maxHp = 1f;
    float hp;

    int previousWaveSign;

    public event System.Action Defeated;
    private bool isDefeated;

    public void Init(LineSample lineSample, EnemyData enemyData)
    {
        data = enemyData;
        this.lineSample = lineSample;
        transform.position = new Vector3(Constants.Width / 2, lineSample.playerMedium.laneY, 0);
        this.lineSample.WaveMoved += OnWaveMoved;

        this.previousWaveSign = 0;


        //仮
        maxHp = data.maxHp;
        hp = maxHp;

        lineSample.enemyMedium.SetPattern(data);

    }
    private void OnWaveMoved()
    {
        if (isDefeated) return;

        float currentWavePower = lineSample.playerMedium.wavePowers[^1];
        if (currentWavePower != 0)
        {
            if (previousWaveSign == 0)
            {
                hp -= 0.5f;
                previousWaveSign = currentWavePower > 0 ? 1 : -1;
            }
            else
            {
                if (currentWavePower > 1f && previousWaveSign == -1)
                {
                    hp -= 0.5f;
                    previousWaveSign = 1;
                }
                else if (currentWavePower < -1f && previousWaveSign == 1)
                {
                    hp -= 0.5f;
                    previousWaveSign = -1;
                }
            }

            hp -= Constants.Width / Constants.Division;

            HPBar.fillAmount = hp / maxHp;
            if (hp <= 0)
            {
                isDefeated = true;
                lineSample.enemyMedium.nextEnemyWaveList.Clear();

                Defeated?.Invoke();
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
