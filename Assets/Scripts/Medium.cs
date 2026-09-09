using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using System.Data.Common; //Debug.Log用

public class Medium
{
    private int division;
    public List<float> wavePowers;
    public List<int> waveIds;

    public int nextWaveId;

    public Medium(int division)
    {
        this.division = division;
        nextWaveId = 1;

        wavePowers = new List<float>();
        waveIds = new List<int>();

        for (int i = 0; i < division; i++)
        {
            wavePowers.Add(0f);
            waveIds.Add(0);
        }
    }

    public List<int> ToUniqueIds()
    {
        List<int> idsSet = waveIds.Distinct().ToList();
        idsSet.Remove(0);

        return idsSet;
    }

    public void LeftWaveMove(float wavePower)
    {
        for (int i = 0; i < division - 1; i++)
        {
            wavePowers[i] = wavePowers[i + 1];
            waveIds[i] = waveIds[i + 1];
        }
        wavePowers[division - 1] = wavePower;
        waveIds[division - 1] = wavePower == 0 ? 0 : nextWaveId;
    }

    public void RightWaveMove(float wavePower)
    {
        for (int i = division - 1; i > 0; i--)
        {
            wavePowers[i] = wavePowers[i - 1];
            waveIds[i] = waveIds[i - 1];
        }
        waveIds[0] = wavePower == 0 ? 0 : nextWaveId;
        wavePowers[0] = wavePower;
    }

    public void WavePowerCheck(Medium opposite)
    {
        List<int> idsSet = ToUniqueIds();
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

            if (changedPowerSum / originalPowerSum <= 0.3) DestructiveInterference(ids, opposite);

        }

    }

    public void DestroyTooSmallWave()
    {
        foreach (int id in ToUniqueIds())
        {
            if (waveIds[0] == id || waveIds[division - 1] == id) continue;
            int count = waveIds.Count(x => x == id);
            if (count <= 5)
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
    private void DestructiveInterference(int id, Medium opposite)
    {
        for (int i = 0; i < division; i++)
        {
            if (waveIds[i] == id)
            {
                waveIds[i] = 0;
                wavePowers[i] = 0;

                opposite.waveIds[i] = 0;
                opposite.waveIds[i] = 0;
            }
        }
    }
}