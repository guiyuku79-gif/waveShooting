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
    public List<float> wavePowers;
    public List<int> waveIds;

    public int nextWaveId;

    public Color32 WaveColor { get; private set; }

    Dictionary<int, float> priviousWavePowers;


    public Medium(Color32 color)
    {
        this.division = Constants.Division;
        this.width = Constants.Width;

        WaveColor = color;

        nextWaveId = 1;

        wavePowers = new List<float>();
        waveIds = new List<int>();

        for (int i = 0; i < division; i++)
        {
            wavePowers.Add(0f);
            waveIds.Add(0);
        }

        priviousWavePowers = new Dictionary<int, float>();
    }

    public List<int> ToUniqueIds()
    {
        List<int> idsSet = waveIds.Distinct().ToList();
        idsSet.Remove(0);

        return idsSet;
    }

    public void WavePowerCheck(Medium opposite)
    {
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

            newWavePowers.Add(ids, changedPowerSum / originalPowerSum);

            if (!priviousWavePowers.ContainsKey(ids)) continue;

            if (changedPowerSum / originalPowerSum <= 0.35f && priviousWavePowers[ids] <= changedPowerSum / originalPowerSum) DestructiveInterference(ids, opposite);

        }

        priviousWavePowers = newWavePowers;
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

    public List<int> ReNewSeparetedWaveId()
    {
        List<int> newIds = new List<int>();

        foreach (int id in ToUniqueIds())
        {
            int index = waveIds.IndexOf(id);

            if (index == -1)
                continue;

            // 最初の連続した id を飛ばす
            while (index < waveIds.Count && waveIds[index] == id)
            {
                index++;
            }

            // 最初の連続部分より後ろにある id を探す
            while (index < waveIds.Count)
            {
                if (waveIds[index] == id)
                {
                    waveIds[index] += 1000000;

                    if (!newIds.Contains(id + 1000000))
                    {
                        newIds.Add(id + 1000000);
                    }
                }

                index++;
            }
        }

        return newIds;
    }
}