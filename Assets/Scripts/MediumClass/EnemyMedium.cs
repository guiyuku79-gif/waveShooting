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

    public void MakeSinWave(float waveLength, float amplitude, float wavecount)
    {

        for (int i = 0; i < division / width * waveLength * wavecount; i++)
        {
            nextEnemyWaveList.Add((nextWaveId, Mathf.Sin(2 * Mathf.PI * i * width / division / waveLength) * amplitude));
        }
        nextWaveId++;
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
        for (int i = 0; i < division / width * waveLength * waveCount; i++)
        {
            nextEnemyWaveList.Add((nextWaveId, TriangleDef(4 * i * width / division / waveLength) * amplitude));
        }
        nextWaveId++;
    }

    public void MakeConstantWave(float waveLength, float amplitude)
    {
        for (int i = 0; i < division / width * waveLength; i++)
        {
            nextEnemyWaveList.Add((nextWaveId, amplitude));
        }
        nextWaveId++;
    }

    public void MakeWait(float Length)
    {
        for (int i = 0; i < division / width * Length; i++)
        {
            nextEnemyWaveList.Add((0, 0));
        }
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
            var nextWave = nextEnemyWaveList[0];

            if (nextWave.id == 0)
            {
                LeftWaveMove(false, 0, -1);
            }
            else
            {
                if (!waveIds.Contains(nextWave.id))
                {
                    newIds.Add(nextWave.id);
                }
                LeftWaveMove(true, nextWave.displacement, nextEnemyWaveList[0].id);

            }

            nextEnemyWaveList.RemoveAt(0);
        }

        return newIds;
    }

}