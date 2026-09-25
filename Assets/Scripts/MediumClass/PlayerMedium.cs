using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using System.Data.Common; //Debug.Log用

public class PlayerMedium : Medium
{
    public PlayerMedium(Color32 color, float laneY)
    : base(color, laneY)
    {
    }

    private bool isShooting = false;

    public void RightWaveMove(bool isPressed, float displacement)
    {
        for (int i = division - 1; i > 0; i--)
        {
            wavePowers[i] = wavePowers[i - 1];
            waveIds[i] = waveIds[i - 1];
        }
        waveIds[0] = isPressed ? nextWaveId : 0;
        wavePowers[0] = displacement;
    }
    public List<int> WaveMove(bool isPressed, float displacement)
    {
        List<int> newIds = new List<int>();

        if (isPressed && !isShooting)
        {
            nextWaveId++;
            newIds.Add(nextWaveId);
        }

        isShooting = isPressed;
        RightWaveMove(isPressed, isPressed ? displacement : 0);

        return newIds;
    }

}