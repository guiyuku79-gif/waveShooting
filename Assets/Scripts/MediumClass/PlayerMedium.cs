using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using System.Data.Common; //Debug.Log用

public class PlayerMedium : Medium
{
    public PlayerMedium()
    : base()
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
    public void WaveMove(bool isPressed, float displacement)
    {
        if (isPressed)
        {
            if (!isShooting)
            {
                nextWaveId++;
                isShooting = true;
            }

            RightWaveMove(isPressed, displacement);
        }
        else
        {
            if (isShooting) isShooting = false;

            RightWaveMove(isPressed, 0);
        }
    }

}