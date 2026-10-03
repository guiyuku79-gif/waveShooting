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

    //敵の行動用
    [NonSerialized] public List<(Constants.EnemyAction action, float displacement)> nextEnemyWaveList = new();
    public int enemyActionId;

    //波のIDを指定する用
    private int nextWaveId;
    public int positionX;

    public void Init(LineSample lineSample, EnemyData enemyData, int nextWaveId)
    {
        data = enemyData;
        this.lineSample = lineSample;
        this.nextWaveId = nextWaveId;
        //仮
        positionX = Constants.Division - 30;

        //localPositionで設定しないとワールド座標になる
        transform.localPosition = new Vector3(-Constants.Width / 2 + positionX * Constants.Width / Constants.Division, 0, 0);


        this.lineSample.WaveMoved += OnWaveMoved;

        this.previousWaveSign = 0;

        maxHp = data.maxHp;
        hp = maxHp;

        //消す予定
        lineSample.enemyMedium.SetPattern(data);

        nextEnemyWaveList = data.CreateWaveList();
        enemyActionId = 0;
    }
    private void OnWaveMoved()
    {
        transform.localPosition = new Vector3(-Constants.Width / 2 + positionX * Constants.Width / Constants.Division,
                                                 lineSample.enemyMedium.wavePowers[^1], 0);
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
        {
            lineSample.WaveMoved -= OnWaveMoved;
            lineSample.UnregisterEnemy(this);
        }

    }

    //波のID 波の強さ 波のX座標　新しい波のID
    public (int id, float displacement, int x, List<int> newIds) WaveMove()
    {
        List<int> newIds = new List<int>();
        if (nextEnemyWaveList.Count == 0)
        {
            return (-1, 0, 0, newIds);
        }
        else
        {
            var nextWave = nextEnemyWaveList[enemyActionId];

            enemyActionId++;
            if (enemyActionId >= nextEnemyWaveList.Count) enemyActionId = 0;

            switch (nextWave.action)
            {
                case Constants.EnemyAction.Wait:
                    return (-1, 0, 0, newIds);

                case Constants.EnemyAction.WaveStart:
                    nextWaveId++;
                    newIds.Add(nextWaveId);

                    enemyActionId++;
                    return (nextWaveId, nextEnemyWaveList[enemyActionId - 1].displacement, positionX, newIds);

                case Constants.EnemyAction.Wave:
                    return (nextWaveId, nextWave.displacement, positionX, newIds);
            }


        }
        return (-1, 0, 0, newIds);
    }
}
