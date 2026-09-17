using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using System.Data.Common; //Debug.Log用

public class EnemyMedium : Medium
{
    public EnemyMedium(int division)
    : base(division)
    {
        //仮
        for (int i = 0; i < 20; i++)
        {
            nextEnemyWaveList.Add((1, i * 0.1f));
            nextWaveId = 2;
        }
    }

    List<(int id, float displacement)> nextEnemyWaveList = new();

    public void MakeSinWave()
    {
        for (int i = 0; i < 30; i++)
        {
            nextEnemyWaveList.Add((nextWaveId, Mathf.Sin(Mathf.PI * i / 15) * 2));
        }
        nextWaveId++;
    }

    public void LeftWaveMove(bool isPressed, float wavePower)
    {
        for (int i = 0; i < division - 1; i++)
        {
            wavePowers[i] = wavePowers[i + 1];
            waveIds[i] = waveIds[i + 1];
        }
        wavePowers[division - 1] = wavePower;
        waveIds[division - 1] = isPressed ? nextWaveId : 0;
    }

    public void WaveMove()
    {
        if (nextEnemyWaveList.Count == 0)
        {
            LeftWaveMove(false, 0);
        }
        else
        {
            LeftWaveMove(true, nextEnemyWaveList[0].displacement);
            nextEnemyWaveList.RemoveAt(0);
        }
    }

}