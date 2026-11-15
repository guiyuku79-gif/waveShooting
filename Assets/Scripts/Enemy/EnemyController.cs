using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
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


    private EnemyActionRunner actionRunner;

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

        previousWaveSign = 0;
        isDefeated = false;

        maxHp = data.maxHp;
        hp = maxHp;

        this.lineSample.WaveMoved += OnWaveMoved;
    }

    private void OnWaveMoved()
    {
        if (!GameStateManager.Instance.IsPlaying || isDefeated)
        {
            return;
        }
        transform.localPosition = new Vector3(-Constants.Width / 2 + positionX * Constants.Width / Constants.Division,
        actionRunner.currentDisplacement, 0);

        float currentWavePower = lineSample.playerMedium.wavePowers[positionX];
        lineSample.playerMedium.DestroyOneDivision(positionX);
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

                Defeated?.Invoke();
                Destroy(gameObject);
            }
        }
    }



    public (
        bool hasWave,
        int id,
        float displacement,
        int x,
        List<int> newIds) WaveMove()
    {

        // 撃破済みの敵は波を出さない
        if (isDefeated)
        {
            return (false, 0, 0f, positionX, new List<int>());
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
