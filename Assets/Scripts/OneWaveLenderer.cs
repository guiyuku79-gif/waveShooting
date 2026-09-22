
using UnityEngine;
using System.Linq;

public class OneWaveLenderer : MonoBehaviour
{
    LineRenderer line;

    Medium medium;

    Medium oppositeMedium;
    public int id;


    public void Init(Medium medium, Medium oppositeMedium, int id)
    {
        this.medium = medium;
        this.oppositeMedium = oppositeMedium;
        this.id = id;

        line = GetComponent<LineRenderer>();
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;

        line.startColor = medium.WaveColor;
        line.endColor = medium.WaveColor;
    }

    void Update()
    {
        line.positionCount = medium.waveIds.Count(x => x == id);

        int i = 0;
        for (int j = 0; j < medium.waveIds.Count; j++)
        {
            if (medium.waveIds[j] != id) continue;

            if (oppositeMedium.waveIds[j] != 0)
            {
                line.SetPosition(i, new Vector3((j - Constants.Division / 2) * Constants.Width / Constants.Division,
                                 medium.wavePowers[j] + oppositeMedium.wavePowers[j], 0));
            }
            else
            {
                line.SetPosition(i, new Vector3((j - Constants.Division / 2) * Constants.Width / Constants.Division,
                                 medium.wavePowers[j], 0));
            }

            i++;
        }
    }
}
