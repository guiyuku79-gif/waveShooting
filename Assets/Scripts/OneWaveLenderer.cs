
using UnityEngine;
using System.Linq;

public class OneWaveLenderer : MonoBehaviour
{
    LineRenderer line;

    Medium medium;
    int id;


    public void Init(Medium medium, int id)
    {
        this.medium = medium;
        this.id = id;

        line = GetComponent<LineRenderer>();
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;

        line.startColor = Color.red;
        line.endColor = Color.red;
    }

    void Update()
    {
        line.positionCount = medium.waveIds.Count(x => x == id);

        int i = 0;
        for (int j = 0; j < medium.waveIds.Count; j++)
        {
            if (medium.waveIds[j] != id) continue;

            line.SetPosition(i, new Vector3((j - Constants.Division / 2) * Constants.Width / Constants.Division,
                                             medium.wavePowers[j], 0));

            i++;
        }
    }
}
