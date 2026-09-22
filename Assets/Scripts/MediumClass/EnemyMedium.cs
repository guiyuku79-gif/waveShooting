using System.Collections.Generic;
using System.Linq;
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

    List<(int id, float displacement)> nextEnemyWaveList = new();

    public void MakeSinWave(float waveLength, float amplitude, float wavecount)
    {

        for (int i = 0; i < division / width * waveLength * wavecount; i++)
        {
            nextEnemyWaveList.Add((nextWaveId, Mathf.Sin(2 * Mathf.PI * i * width / division / waveLength) * amplitude));
        }
        nextWaveId++;
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
            if (waveIds.IndexOf(nextEnemyWaveList[0].id) == -1) newIds.Add(nextEnemyWaveList[0].id);
            LeftWaveMove(true, nextEnemyWaveList[0].displacement, nextEnemyWaveList[0].id);

            nextEnemyWaveList.RemoveAt(0);
        }

        return newIds;
    }

}