using System.Collections.Generic;
using System;
using UnityEngine;
using System.Data.Common;
using UnityEngine.UIElements; //Debug.Log用

public class EnemyMedium : Medium
{
    public EnemyMedium(Color32 color, float laneY)
    : base(color, laneY)
    {
    }

    private int enemyActionId = 0;

    public void MakeSinWave(float waveLength, float amplitude, float waveCount)
    {
        nextEnemyWaveList.Add((Constants.EnemyAction.WaveStart, 0));
        for (int i = 0; i < division / width * waveLength * waveCount; i++)
        {
            nextEnemyWaveList.Add((Constants.EnemyAction.Wave, Mathf.Sin(2 * Mathf.PI * i * width / division / waveLength) * amplitude));
        }
    }

    private float TriangleDef(float x)
    {
        x = x % 4f;
        if (x <= 1f) return x;
        else if (x <= 3f) return 2 - x;
        else return x - 4;
    }

    public void MakeTriangleWave(float waveLength, float amplitude, float waveCount)
    {
        nextEnemyWaveList.Add((Constants.EnemyAction.WaveStart, 0));
        for (int i = 0; i < division / width * waveLength * waveCount; i++)
        {
            nextEnemyWaveList.Add((Constants.EnemyAction.Wave, TriangleDef(4 * i * width / division / waveLength) * amplitude));
        }
    }

    public void MakeConstantWave(float waveLength, float amplitude)
    {
        nextEnemyWaveList.Add((Constants.EnemyAction.WaveStart, 0));
        for (int i = 0; i < division / width * waveLength; i++)
        {
            nextEnemyWaveList.Add((Constants.EnemyAction.Wave, amplitude));
        }
    }

    public void MakeWait(float waveLength)
    {
        for (int i = 0; i < division / width * waveLength; i++)
        {
            nextEnemyWaveList.Add((Constants.EnemyAction.Wait, 0));
        }
    }

    public void SetPattern(EnemyData data)
    {
        nextEnemyWaveList = data.CreateWaveList();
        enemyActionId = 0;
    }

    public void LeftWaveMove(bool isPressed, float wavePower, int id)
    {
        for (int i = 0; i < division - 1; i++)
        {
            wavePowers[i] = wavePowers[i + 1];
            waveIds[i] = waveIds[i + 1];
        }
        wavePowers[division - 1] = wavePower;
        waveIds[division - 1] = isPressed ? id : 0;
    }

    public List<int> WaveMove()
    {
        List<int> newIds = new List<int>();
        if (nextEnemyWaveList.Count == 0)
        {
            LeftWaveMove(false, 0, -1);
        }
        else
        {
            var nextWave = nextEnemyWaveList[enemyActionId];

            switch (nextWave.action)
            {
                case Constants.EnemyAction.Wait:
                    LeftWaveMove(false, 0, -1);
                    break;
                case Constants.EnemyAction.WaveStart:
                    nextWaveId++;
                    newIds.Add(nextWaveId);

                    enemyActionId++;
                    LeftWaveMove(true, nextEnemyWaveList[enemyActionId].displacement, nextWaveId);
                    break;
                case Constants.EnemyAction.Wave:
                    LeftWaveMove(true, nextWave.displacement, nextWaveId);
                    break;
            }

            enemyActionId++;
            if (enemyActionId >= nextEnemyWaveList.Count) enemyActionId = 0;
        }

        return newIds;
    }

    public void Move(List<(int id, float displacement, int x)> newWaveSources)
    {
        for (int i = 0; i < division - 1; i++)
        {
            wavePowers[i] = wavePowers[i + 1];
            waveIds[i] = waveIds[i + 1];
        }
        wavePowers[division - 1] = 0;
        waveIds[division - 1] = 0;

        foreach (var waveSources in newWaveSources)
        {
            wavePowers[waveSources.x] = waveSources.displacement;
            waveIds[waveSources.x] = waveSources.id;
        }
    }

}