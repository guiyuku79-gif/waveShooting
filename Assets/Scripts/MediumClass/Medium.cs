using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using System.Data.Common;
using UnityEngine.AdaptivePerformance; //Debug.Log用

public class Medium
{
    public int division;
    public float width;

    public float laneY;
    public List<float> wavePowers;
    public List<int> waveIds;

    public int nextWaveId;

    public int nextWaveIdForSeparate;

    public Color32 WaveColor { get; private set; }

    public Dictionary<int, float> previousWaveSigns;

    public List<(Constants.EnemyAction action, float displacement)> nextEnemyWaveList = new();


    public Medium(Color32 color, float laneY)
    {
        this.division = Constants.Division;
        this.width = Constants.Width;
        this.laneY = laneY;

        WaveColor = color;

        nextWaveId = 1;
        nextWaveIdForSeparate = 1000000;

        wavePowers = new List<float>();
        waveIds = new List<int>();

        for (int i = 0; i < division; i++)
        {
            wavePowers.Add(0f);
            waveIds.Add(0);
        }

        previousWaveSigns = new Dictionary<int, float>();
    }

    public List<int> ToUniqueIds()
    {
        List<int> idsSet = waveIds.Distinct().ToList();
        idsSet.Remove(0);

        return idsSet;
    }

    public float WavePowerCheck(Medium opposite)
    {
        float wavePowerSum = 0;
        List<int> idsSet = ToUniqueIds();

        //前回の減少率を記憶しておく
        Dictionary<int, float> newWavePowers = new Dictionary<int, float>();
        foreach (int ids in idsSet)
        {
            float originalPowerSum = 0;
            float changedPowerSum = 0;
            for (int i = 0; i < division; i++)
            {
                if (waveIds[i] != ids) continue;

                originalPowerSum += Math.Abs(wavePowers[i]);
                changedPowerSum += Math.Abs(wavePowers[i] + opposite.wavePowers[i]);
            }

            float powerRatio = 1f;
            if (originalPowerSum != 0) powerRatio = changedPowerSum / originalPowerSum;
            // float powerRatio = changedPowerSum / originalPowerSum;

            newWavePowers.Add(ids, powerRatio);

            if (!previousWaveSigns.TryGetValue(ids, out float previousRatio))
            {
                continue;
            }

            if (previousRatio <= 0.35f && previousRatio <= powerRatio)
            {
                wavePowerSum += DestructiveInterference(ids, opposite);
            }

        }

        previousWaveSigns = newWavePowers;

        return wavePowerSum;
    }

    public void DestroyOneDivision(int index)
    {
        wavePowers[index] = 0f;
        waveIds[index] = 0;
    }

    public void DestroyTooSmallWave(List<int> expectIndexes)
    {
        foreach (int id in ToUniqueIds())
        {
            bool isNearCharacter = false;
            foreach (int index in expectIndexes)
            {
                if (waveIds[index] == id) isNearCharacter = true;
            }
            if (waveIds[0] == id || waveIds[division - 1] == id) isNearCharacter = true;
            int count = waveIds.Count(x => x == id);
            if (Constants.ShortestWaveLength >= count * Constants.Width / Constants.Division && !isNearCharacter)
            {
                for (int i = 0; i < division; i++)
                {
                    if (waveIds[i] == id)
                    {
                        waveIds[i] = 0;
                        wavePowers[i] = 0;
                    }
                }
            }
        }
    }
    private float DestructiveInterference(int id, Medium opposite)
    {
        float wavePowerSum = 0;
        for (int i = 0; i < division; i++)
        {
            if (waveIds[i] == id)
            {
                waveIds[i] = 0;
                wavePowers[i] = 0;

                opposite.waveIds[i] = 0;
                opposite.wavePowers[i] = 0;

                wavePowerSum += Constants.Width / Constants.Division;
            }
        }
        return wavePowerSum;
    }

    public List<int> ReassignSeparatedWaveId(bool reverse = false)
    {
        List<int> newIds = new List<int>();
        List<int> uniqueIds = ToUniqueIds();
        int step = reverse ? -1 : 1;

        foreach (int id in uniqueIds)
        {
            int index = reverse
                ? waveIds.LastIndexOf(id)
                : waveIds.IndexOf(id);

            if (index == -1)
                continue;

            // 走査方向で最初の連続部分は、元の ID を残す
            while (index >= 0 && index < waveIds.Count
                   && waveIds[index] == id)
            {
                index += step;
            }

            bool changed = false;

            // その先にある同じ ID を、新しい ID にする
            while (index >= 0 && index < waveIds.Count)
            {
                if (waveIds[index] == id)
                {
                    waveIds[index] = nextWaveIdForSeparate;
                    changed = true;
                }

                index += step;
            }

            if (changed)
            {
                newIds.Add(nextWaveIdForSeparate);
                nextWaveIdForSeparate++;
            }
        }

        return newIds;
    }
}