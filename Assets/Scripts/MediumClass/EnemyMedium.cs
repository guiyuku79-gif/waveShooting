using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements; //Debug.Log用

public class EnemyMedium : Medium
{
    public EnemyMedium(Color32 color, float laneY)
    : base(color, laneY)
    {
    }

    public void Move(List<(int id, float displacement, int x)> newWaveSources)
    {
        //　既存の波を左へ1区画進める
        for (int i = 0; i < division - 1; i++)
        {
            wavePowers[i] = wavePowers[i + 1];
            waveIds[i] = waveIds[i + 1];
        }

        //右端を空にする
        wavePowers[division - 1] = 0;
        waveIds[division - 1] = 0;

        //敵が出した波の状況を反映する
        foreach (var waveSources in newWaveSources)
        {
            wavePowers[waveSources.x] += waveSources.displacement;
            waveIds[waveSources.x] = waveSources.id;
        }
    }

}