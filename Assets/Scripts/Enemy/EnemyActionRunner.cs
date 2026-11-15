using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyActionRunner
{
    private readonly List<(Constants.EnemyAction action, float displacement)> actions;

    private int actionIndex;
    private int nextWaveId;

    public int PositionX { get; private set; }
    public float currentDisplacement { get; private set; }

    public EnemyActionRunner(List<(Constants.EnemyAction action, float displacement)> actions, int initialPositionX, int initialWaveId)
    {
        this.actions = new List<(Constants.EnemyAction action, float displacement)>(actions);

        PositionX = initialPositionX;
        nextWaveId = initialWaveId;
    }

    private (Constants.EnemyAction action, float displacement)
        ReadNextAction()
    {
        var action = actions[actionIndex];

        actionIndex = (actionIndex + 1) % actions.Count;

        return action;
    }

    public EnemyActionResult Step()
    {
        currentDisplacement = 0f;

        if (actions.Count == 0)
        {
            return EnemyActionResult.NoWave(PositionX);
        }

        bool isNewWave = false;
        var action = ReadNextAction();

        // 開始マーカーと最初の波データを同じ更新で読む
        if (action.action == Constants.EnemyAction.WaveStart)
        {
            if (actions[actionIndex].action
                != Constants.EnemyAction.Wave)
            {
                Debug.LogError(
                    "WaveStartの直後にはWaveが必要です。");

                return EnemyActionResult.NoWave(PositionX);
            }

            nextWaveId++;
            isNewWave = true;

            action = ReadNextAction();
        }

        switch (action.action)
        {
            case Constants.EnemyAction.Wave:
                currentDisplacement = action.displacement;

                return EnemyActionResult.Emit(
                    waveId: nextWaveId,
                    displacement: currentDisplacement,
                    positionX: PositionX,
                    isNewWave: isNewWave);

            case Constants.EnemyAction.Move:
                PositionX = (int)Mathf.Clamp(
                    PositionX + (int)action.displacement,
                    30,
                    Constants.Division - 1);

                break;

            case Constants.EnemyAction.Wait:
                break;
        }

        return EnemyActionResult.NoWave(PositionX);
    }
}