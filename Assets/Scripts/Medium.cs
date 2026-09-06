using System.Collections.Generic;
public class Medium
{
    private int division;
    public List<float> wavePowers;
    public List<int> waveIds;

    public int nextWaveId;

    public Medium()
    {
        division = LineSample.Instance.division;
        nextWaveId = 1;

        wavePowers = new List<float>();
        waveIds = new List<int>();

        for (int i = 0; i < division; i++)
        {
            wavePowers.Add(0f);
            waveIds.Add(0);
        }
    }

    public void LeftWaveMove(float wavePower)
    {
        for (int i = 0; i < division - 1; i++)
        {
            wavePowers[i] = wavePowers[i + 1];
            waveIds[i] = waveIds[i + 1];
        }
        wavePowers[division - 1] = wavePower;
        waveIds[division - 1] = nextWaveId;
    }

    public void RightWaveMove(float wavePower)
    {
        for (int i = division - 1; i > 0; i--)
        {
            wavePowers[i] = wavePowers[i - 1];
            waveIds[i] = waveIds[i - 1];
        }
        waveIds[0] = nextWaveId;
        wavePowers[0] = wavePower;
    }


}