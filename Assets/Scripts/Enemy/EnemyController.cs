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

    private float currentDisplacement;

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
        positionX = Constants.Division - 1;
        //localPositionで設定しないとワールド座標になる
        transform.localPosition = new Vector3(-Constants.Width / 2 + positionX * Constants.Width / Constants.Division, 0, 0);


        this.lineSample.WaveMoved += OnWaveMoved;

        this.previousWaveSign = 0;

        maxHp = data.maxHp;
        hp = maxHp;

        nextEnemyWaveList = data.CreateWaveList();
        enemyActionId = 0;

        currentDisplacement = 0f;
    }

    private void OnWaveMoved()
    {
        if (!GameStateManager.Instance.IsPlaying || isDefeated)
        {
            return;
        }
        transform.localPosition = new Vector3(-Constants.Width / 2 + positionX * Constants.Width / Constants.Division,
        currentDisplacement, 0);
        if (isDefeated) return;

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

    private (Constants.EnemyAction action, float displacement) ReadNextAction()
    {
        var action = nextEnemyWaveList[enemyActionId];

        enemyActionId = (enemyActionId + 1) % nextEnemyWaveList.Count;

        return action;
    }


    public (
        bool hasWave,
        int id,
        float displacement,
        int x,
        List<int> newIds) WaveMove()
    {
        List<int> newIds = new();

        currentDisplacement = 0f;

        // 行動がない敵や撃破済みの敵は波を出さない
        if (isDefeated || nextEnemyWaveList.Count == 0)
        {
            return (false, 0, 0f, positionX, newIds);
        }

        var action = ReadNextAction();

        // WaveStart は開始マーカー。
        // 同じ更新内で、その直後の最初の波データも読む。
        if (action.action == Constants.EnemyAction.WaveStart)
        {
            if (nextEnemyWaveList[enemyActionId].action
                != Constants.EnemyAction.Wave)
            {
                Debug.LogError(
                    "WaveStartの直後にはWaveが必要です。",
                    this);

                return (false, 0, 0f, positionX, newIds);
            }

            nextWaveId++;
            newIds.Add(nextWaveId);

            action = ReadNextAction();
        }

        switch (action.action)
        {
            case Constants.EnemyAction.Wave:
                currentDisplacement = action.displacement;

                return (
                    true,
                    nextWaveId,
                    currentDisplacement,
                    positionX,
                    newIds);

            case Constants.EnemyAction.Move:
                positionX = (int)Mathf.Clamp(
                    positionX + (int)action.displacement,
                    30,
                    Constants.Division - 1);

                break;

            case Constants.EnemyAction.Wait:
                break;
        }

        return (false, 0, 0f, positionX, newIds);
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
