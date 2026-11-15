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

    public (
        bool hasWave,
        int id,
        float displacement,
        int x,
        List<int> newIds) Step()
    {
        List<int> newIds = new();
        currentDisplacement = 0f;

        // 行動がない敵や撃破済みの敵は波を出さない
        if (actions.Count == 0)
        {
            return (false, 0, 0f, PositionX, newIds);
        }

        var action = ReadNextAction();

        // WaveStart は開始マーカー。
        // 同じ更新内で、その直後の最初の波データも読む。
        if (action.action == Constants.EnemyAction.WaveStart)
        {
            if (actions[actionIndex].action
                != Constants.EnemyAction.Wave)
            {
                Debug.LogError(
                    "WaveStartの直後にはWaveが必要です。");

                return (false, 0, 0f, PositionX, newIds);
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
                    PositionX,
                    newIds);

            case Constants.EnemyAction.Move:
                PositionX = (int)Mathf.Clamp(
                    PositionX + (int)action.displacement,
                    30,
                    Constants.Division - 1);

                break;

            case Constants.EnemyAction.Wait:
                break;
        }

        return (false, 0, 0f, PositionX, newIds);
    }
}